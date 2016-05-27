using ATP2016Project.Controller;
using ATP2016Project.Model.Algorithms.Compression;
using ATP2016Project.Model.Algorithms.MazeGenerators;
using ATP2016Project.Model.Algorithms.Search;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// In this class we do the calculate that we need to help service the user 
/// </summary>
namespace ATP2016Project.Model
{
    class MyModel : IModel
    {
        private IController m_controller;
        private Dictionary<string, IMaze> m_mazes;
        private Dictionary<string, Solution> m_mazesSolution;
        /// <summary>
        /// ctor with controller in parameter
        /// </summary>
        /// <param name="controller">contoller</param>
        public MyModel(IController controller)
        {
            m_controller = controller;
            m_mazes = new Dictionary<string, IMaze>();
            m_mazesSolution = new Dictionary<string, Solution>();
        }
        /// <summary>
        /// generate maze with the diminsion that we give from the user
        /// </summary>
        /// <param name="x">x length</param>
        /// <param name="y">y length</param>
        /// <param name="z">z length</param>
        /// <param name="name">name</param>
        public void generateMaze(int x, int y, int z, string name)
        {
            IMazeGenerator mazeGenerator = new MyMaze3dGenerator();
            m_mazes[name.ToLower()] = mazeGenerator.generate(x, y, z);
        }
        /// <summary>
        /// get the maze from the dictionary
        /// </summary>
        /// <param name="name">maze name</param>
        /// <returns>maze</returns>
        public IMaze getMaze(string name)
        {
            if (m_mazes.ContainsKey(name))
            {
                return m_mazes[name];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// load maze from the path that we give from the user and
        /// save thae maze in the dictionary with name that we give from the user
        /// </summary>
        /// <param name="path">path to load</param>
        /// <param name="name">maze new name</param>
        /// <returns></returns>
        public string loadMaze(string path, string name)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                using (Stream fileStream = new MyCompressorStream(fs, MyCompressorStream.compressionMode.decompress))
                {
                    List<byte> mazeBytes = decompressFromFile(fileStream);
                    IMaze maze = new Maze3d(mazeBytes.ToArray());
                    m_mazes[name] = maze;
                }
            }
            return "Loaded maze successfully";

        }

        /// <summary>
        /// gets a file and decompress the maze from it
        /// </summary>
        /// <param name="fileStream">the compressed file</param>
        /// <returns>list of bytes from the compressed file</returns>
        private static List<byte> decompressFromFile(Stream fileStream)
        {
            List<byte> mazeBytes = new List<byte>();
            byte[] buffer = new byte[100];
            int r = 0;
            while ((r = fileStream.Read(buffer, 0, 100)) != 0)
            {
                for (int i = 0; i < r; i++)
                {
                    mazeBytes.Add(buffer[i]);
                }
                buffer = new byte[100];
            }

            return mazeBytes;
        }
        /// <summary>
        /// save the maze to the path that we give from the user
        /// </summary>
        /// <param name="mazeName">maze name</param>
        /// <param name="path">path</param>
        /// <returns>save or not</returns>
        public string saveMaze(string mazeName, string path)
        {
            if (path[path.Length - 1] != '\\')
            {
                path += @"\";
            }
            string filePath = path + mazeName + ".maze";
            Maze3d myMaze = getMaze(mazeName) as Maze3d;
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    using (Stream mazeStream = new MemoryStream(myMaze.toByteArray()))
                    {
                        using (Stream fileStream = new MyCompressorStream(fs, MyCompressorStream.compressionMode.compress))
                        {
                            byte[] byteArray = new byte[100];
                            int r = 0;
                            while ((r = mazeStream.Read(byteArray, 0, byteArray.Length)) != 0)
                            {
                                fileStream.Write(byteArray, 0, 100);
                                fileStream.Flush();
                                byteArray = new byte[100];
                            }
                        }
                    }
                }
                return "saved the maze " + mazeName + " in " + filePath;
            }
            catch (Exception e)
            {
                return "You need to run the program as adminitrator";
            }

        }
        /// <summary>
        /// get maze size from the doctionary
        /// </summary>
        /// <param name="maze">maze</param>
        /// <returns>the size of the maze</returns>
        public long getMazeSize(IMaze maze)
        {
            long size = 0;
            Maze3d myMaze = maze as Maze3d;
            byte[] mazeBytes = myMaze.toByteArray();
            using (Stream s = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(s, mazeBytes);
                size = s.Length;
            }
            return size;
        }
        /// <summary>
        /// get the file size from the path that we give from the user
        /// </summary>
        /// <param name="filePath">file path</param>
        /// <returns></returns>
        public long getFileSize(string filePath)
        {
            long size = 0;
            size = new FileInfo(filePath).Length;
            return size;
        }
        /// <summary>
        /// check if the algo that we give from the user is valid
        /// </summary>
        /// <param name="algoName">name of the algorithm</param>
        /// <returns>true or false</returns>
        public bool algorithmExist(string algoName)
        {
            return algoName.ToLower() == "bfs" || algoName.ToLower() == "dfs";
        }

        /// <summary>
        /// solve the maze with a specific algorithm that we recieve as parameter.
        /// store the solution in the dictionary
        /// </summary>
        /// <param name="mazeName">the name of the maze</param>
        /// <param name="algoName">the name of the algorithm</param>
        public void solveMaze(string mazeName, string algoName)
        {
            string algorithm = algoName.ToLower();
            ISearchingAlgorithm algo;
            if (algorithm == "bfs")
            {
                algo = new BreadthFirstSearch();
            }
            else //dfs
            {
                algo = new DepthFirstSearch();
            }
            IMaze maze = m_mazes[mazeName.ToLower()];
            ISearchable searchableMaze = new SearchableMaze3d(maze);
            m_mazesSolution[mazeName.ToLower()] = algo.search(searchableMaze);
        }

        /// <summary>
        /// check if the solution of the maze is in our dictionary
        /// and returns true or false
        /// </summary>
        /// <param name="mazeName">the name of the maze</param>
        /// <returns>true or false (if the maze is in our dictionary)</returns>
        public bool solutionExist(string mazeName)
        {
            return m_mazesSolution.ContainsKey(mazeName.ToLower());
        }

        public Solution getSolution(string mazeName)
        {
            return m_mazesSolution[mazeName.ToLower()];
        }

        public string getDir(string path)
        {
            string output = string.Format("Showing the dir of {0}\n", path);
            if (Directory.Exists(path))
            {
                foreach (string dir in Directory.GetDirectories(path))
                {
                    output += dir + "\n";
                }
                foreach (string file in Directory.GetFiles(path))
                {
                    output += file + "\n";
                }
            }
            else
            {
                output = "Directory " + path + " doesn't exist!";
            }
            return output;
        }

        /// <summary>
        /// mark the solution for the maze in the grid
        /// </summary>
        /// <param name="mazeName">the grid</param>
        public void markSolution(string mazeName)
        {
            ISearchable searchableMaze = new SearchableMaze3d(m_mazes[mazeName]);
            //mark the solution in grid
            (searchableMaze as SearchableMaze3d).markSolutionInGrid(m_mazesSolution[mazeName]);
        }

        /// <summary>
        /// clear the path of the solution
        /// </summary>
        /// <param name="mazeName">the maze with the solution path</param>
        public void clearSolution(string mazeName)
        {
            ISearchable searchableMaze = new SearchableMaze3d(m_mazes[mazeName]);
            //mark the solution in grid
            (searchableMaze as SearchableMaze3d).initializeGrid();
        }
    }
}
