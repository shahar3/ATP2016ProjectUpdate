using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeLib;
using System.IO;
using System.IO.Compression;
using System.Threading;
using MazeRunner2016.Controls;
using System.Runtime.CompilerServices;

namespace MazeRunner2016
{
    public class Model : IModel
    {
        public event finishedComputing ModelChanged;
        //private delegate void calcInMaze(string mazeName, string parameters, object o);
        private List<string> m_mazesNames;
        private Dictionary<string, Maze3d> m_mazes = new Dictionary<string, Maze3d>();
        private Dictionary<Maze3d, Solution> m_mazesSolution = new Dictionary<Maze3d, Solution>();
        private Dictionary<string, string> m_mazesSolveTime = new Dictionary<string, string>();
        private Dictionary<string, int> m_mazesStatesDeveloped = new Dictionary<string, int>();

        public Model()
        {
            initThreadPool();
            loadMazes();
        }

        private void loadMazes()
        {
        }

        private void initThreadPool()
        {
            int workerThreads = 10;
            int completedThreads = 10;
            ThreadPool.SetMaxThreads(workerThreads, completedThreads);
        }

        public void generateMaze(int x, int y, int z, string mazeName)
        {
            string parameters = string.Format("{0},{1},{2}", x, y, z);
            Run("generate3dMaze", mazeName, parameters);
        }

        public void activateEvent(string commandName, string otherInformation)
        {
            ModelChanged(commandName, otherInformation);
        }

        public void prepareMazesNames()
        {
            m_mazesNames = new List<string>();
            m_mazesNames.AddRange(m_mazes.Keys);
            ModelChanged("getMazesNames", "done");
        }

        public string[] getMazesNames()
        {
            return m_mazesNames.ToArray();
        }

        public object getMaze(string nameOfTheMaze)
        {
            return m_mazes[nameOfTheMaze];
        }

        public void solveMaze(string mazeName, string algoName)
        {
            Run("solveMaze", mazeName, algoName);
        }

        public Solution getSolution(string mazeName)
        {
            return m_mazesSolution[m_mazes[mazeName] as Maze3d];
        }

        internal string getSolvedTime(string mazeName)
        {
            return m_mazesSolveTime[mazeName];
        }

        public void saveMaze(string mazeName, string mazePath)
        {
            Maze3d myMaze = m_mazes[mazeName] as Maze3d;
            try
            {
                using (FileStream fs = new FileStream(mazePath, FileMode.Create))
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
            }
            catch (Exception e)
            {
                ModelChanged("saveMaze", "errorSave");
            }
            ModelChanged("saveMaze", "The maze saved");
        }

        public int getStatesDeveloped(string mazeName)
        {
            return m_mazesStatesDeveloped[mazeName];
        }

        public void loadMaze(string mazePath, string mazeNameToSave)
        {
            using (FileStream fs = new FileStream(mazePath, FileMode.Open))
            {
                using (Stream fileStream = new MyCompressorStream(fs, MyCompressorStream.compressionMode.decompress))
                {
                    List<byte> mazeBytes = decompressFromFile(fileStream);
                    IMaze maze = new Maze3d(mazeBytes.ToArray());
                    m_mazes[mazeNameToSave] = maze as Maze3d;
                }
            }
            ModelChanged("loadMaze", "mazeNameToSave");
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



        private void Run(string commandName, string mazeName, string parameters)
        {
            switch (commandName)
            {
                case "generate3dMaze":
                    //this maze exist
                    if (m_mazes.ContainsKey(mazeName))
                    {
                        ModelChanged("isExist", "The name " + mazeName + " already exist");
                    }
                    else//do not exist
                    {
                        RunInThreadPool(commandName, mazeName, parameters);
                    }
                    break;
                case "solveMaze":
                    if (m_mazesSolution.ContainsKey(m_mazes[mazeName] as Maze3d))
                    {
                        ModelChanged("isExist", "The solution for the maze " + mazeName + " already exist");
                    }
                    else
                    {
                        RunInThreadPool(commandName, mazeName, parameters);
                    }
                    break;
            }
        }

        private void RunInThreadPool(string CommandName, string mazeName, string parameters)
        { //10,10,3 
            switch (CommandName)
            {
                case "generate3dMaze":
                    ThreadPool.QueueUserWorkItem
                   (
                        new WaitCallback((generate3dMaze) =>
                        {
                            MazeLib.IMazeGenerator generateMaze = new MyMaze3dGenerator();
                            string[] dimensions = parameters.Split(',');
                            m_mazes[mazeName] = generateMaze.generate(Int32.Parse(dimensions[0]), Int32.Parse(dimensions[1]), Int32.Parse(dimensions[2])) as Maze3d;
                            ModelChanged(CommandName, mazeName);
                        })
                   );
                    break;
                case "solveMaze":
                    ThreadPool.QueueUserWorkItem(
                        new WaitCallback((solveMaze) =>
                        {
                            string algoName = parameters;
                            ISearchable mySearchableMaze = new SearchableMaze3d(m_mazes[mazeName]);
                            string timeToSolve = string.Empty;
                            if (algoName == "BFS")
                            {
                                //
                                ISearchingAlgorithm BFS = new BreadthFirstSearch();
                                m_mazesSolution[m_mazes[mazeName] as Maze3d] = BFS.search(mySearchableMaze, out timeToSolve);
                                getSolutionInfo(mazeName, timeToSolve, BFS);
                            }
                            else
                            {
                                //
                                ISearchingAlgorithm DFS = new DepthFirstSearch();
                                m_mazesSolution[m_mazes[mazeName] as Maze3d] = DFS.search(mySearchableMaze, out timeToSolve);
                                getSolutionInfo(mazeName, timeToSolve, DFS);
                            }
                        })
                    );
                    break;
                default:
                    break;
            }
        }

        private void getSolutionInfo(string mazeName, string timeToSolve, ISearchingAlgorithm algo)
        {
            m_mazesSolveTime[mazeName] = timeToSolve;
            m_mazesStatesDeveloped[mazeName] = algo.statesDeveloped();
            ModelChanged("solveMaze", mazeName);
        }

        public void saveMazesToZip()
        {
            foreach (Maze3d maze in m_mazesSolution.Keys)
            {
                ICompressor compressor = new MyMaze3Dcompressor();
                SearchableMaze3d myMaze = new SearchableMaze3d(maze);
                myMaze.initializeGrid();
                byte[] mazeToSave = compressor.compress(maze.toByteArray());
                //now we want to write the compressed maze to a zip file
                string solutionToSave = m_mazesSolution[maze].ToString();
                string mazeName = m_mazes.FirstOrDefault(x => x.Value == maze).Key;
                createFolder(mazeName);
                writeToFolder(mazeToSave, mazeName + "\\" + mazeName + ".maze");
                writeToFolder(solutionToSave, mazeName + "\\" + mazeName + ".sol");
                addFileToZip(mazeName);
            }
        }

        private void addFileToZip(string mazeName)
        {
            foreach (string file in Directory.GetFiles(mazeName))
            {
                using (FileStream inputStream = new FileStream(file, FileMode.Open))
                {
                    using (FileStream outputStream = new FileStream("Mazes.zip", FileMode.Append,FileAccess.ReadWrite))
                    {
                        using (GZipStream zipStream = new GZipStream(inputStream, CompressionLevel.Optimal))
                        {
                            inputStream.CopyTo(zipStream);
                        }
                    }
                }
            }
        }

        private void createFolder(string mazeName)
        {
            Directory.CreateDirectory(mazeName);
        }

        private void writeToFolder(object objectToSave, string nameToSave)
        {
            byte[] inputSource;
            if (objectToSave is byte[])
            {
                inputSource = objectToSave as byte[];
            }
            else //string
            {
                string stringToConvert = objectToSave as string;
                inputSource = Encoding.ASCII.GetBytes(stringToConvert);
            }
            using (MemoryStream input = new MemoryStream(inputSource))
            {
                using (FileStream outputStream = new FileStream(nameToSave, FileMode.Create))
                {
                    byte[] byteArray = new byte[100];
                    int r = 0;
                    while ((r = input.Read(byteArray, 0, byteArray.Length)) != 0)
                    {
                        outputStream.Write(byteArray, 0, r);
                        outputStream.Flush();
                        byteArray = new byte[100];
                    }
                }
            }
        }
    }
}
