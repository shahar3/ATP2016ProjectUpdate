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

namespace ATP2016Project.Model
{
    class MyModel : IModel
    {
        private IController m_controller; //the controller layer object
        private Dictionary<string, IMaze> m_mazes; //the mazes dictionary
        private Dictionary<string, Solution> m_mazesSolution; //the solutions dictionary

        public MyModel(IController controller)
        {
            m_controller = controller;
            m_mazes = new Dictionary<string, IMaze>();
            m_mazesSolution = new Dictionary<string, Solution>();
        }

        public void generateMaze(int x, int y, int z, string name)
        {
            IMazeGenerator mazeGenerator = new MyMaze3dGenerator();
            m_mazes[name.ToLower()] = mazeGenerator.generate(x, y, z);
        }

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

        public string saveMaze(string mazeName, string path)
        {
            if (path[path.Length - 1] != '\\')
            {
                path += @"\";
            }
            string filePath = path + mazeName + ".maze";
            Maze3d myMaze = getMaze(mazeName) as Maze3d;
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

        public long getFileSize(string filePath)
        {
            long size = 0;
            size = new FileInfo(filePath).Length;
            return size;
        }

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
            IMaze maze = m_mazes[mazeName];
            ISearchable searchableMaze = new SearchableMaze3d(maze);
            m_mazesSolution[mazeName] = algo.search(searchableMaze);
        }

        /// <summary>
        /// check if the solution of the maze is in our dictionary
        /// and returns true or false
        /// </summary>
        /// <param name="mazeName">the name of the maze</param>
        /// <returns>true or false (if the maze is in our dictionary)</returns>
        public bool solutionExist(string mazeName)
        {
            return m_mazesSolution.ContainsKey(mazeName);
        }

        public Solution getSolution(string mazeName)
        {
            return m_mazesSolution[mazeName];
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

        public void clearSolution(string mazeName)
        {
            ISearchable searchableMaze = new SearchableMaze3d(m_mazes[mazeName]);
            //mark the solution in grid
            (searchableMaze as SearchableMaze3d).initializeGrid();
        }
    }
}
