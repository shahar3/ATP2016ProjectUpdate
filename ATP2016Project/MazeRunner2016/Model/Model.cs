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
using Ionic.Zip;

namespace MazeRunner2016
{
    /// <summary>
    /// this class contain functions that activate from the presenter layer
    /// this functions activate functions from the mazeLib and compute the promblems
    /// </summary>
    public class Model : IModel
    {
        //settings fields
        private int m_numberOfThreads;
        private string[] m_createMazeAlgo;
        private string[] m_solveMazeAlgo;
        //envent that activate when we finish computing
        public event finishedComputing ModelChanged;
        private List<string> m_mazesNames;
        //dictionary that contain all the mazes that the user created
        private Dictionary<string, Maze3d> m_mazes = new Dictionary<string, Maze3d>();
        //dictionary that contain all the mazes that the user call for solution
        private Dictionary<Maze3d, Solution> m_mazesSolution = new Dictionary<Maze3d, Solution>();
        private Dictionary<string, string> m_mazesSolveTime = new Dictionary<string, string>();
        private Dictionary<string, int> m_mazesStatesDeveloped = new Dictionary<string, int>();
        private Dictionary<string, Solution> m_mazeNamesSolution = new Dictionary<string, Solution>();

        //zip fields
        private ZipFile zip;
        private List<string> m_folderNames = new List<string>();

        /// <summary>
        /// constructor
        /// </summary>
        public Model()
        {
            initThreadPool();
            loadMazes();
        }

        /// <summary>
        ///this function activated from the presenter and init the settings fields
        ///with details from settings file
        /// </summary>
        /// <param name="numberOfThreads">number of threads</param>
        /// <param name="createMazeAlgo">name of create algorithm</param>
        /// <param name="solveMazeAlgo">name of solve algorithms</param>
        public void injectionSettingsModel(int numberOfThreads, string[] createMazeAlgo, string[] solveMazeAlgo)
        {
            m_numberOfThreads = numberOfThreads;
            m_createMazeAlgo = createMazeAlgo;
            m_solveMazeAlgo = solveMazeAlgo;
        }

        /// <summary>
        /// this function activate from the constuctor and load the mazes 
        /// that save in the exit application 
        /// </summary>
        private void loadMazes()
        {
            //check if exist zip that contain the mazes
            if (!File.Exists("Mazes.zip"))
            {
                return;
            }
            using (ZipFile zip = ZipFile.Read("Mazes.zip"))
            {
                foreach (ZipEntry e in zip)
                {
                    string mazeName = e.FileName.Substring(0, e.FileName.IndexOf('.'));
                    string type = e.FileName.Substring(e.FileName.IndexOf('.') + 1);
                    e.Extract(ExtractExistingFileAction.OverwriteSilently);
                    if (type == "maze")
                    {
                        buildMaze(e.FileName, mazeName);
                    }
                    else //it's a solution
                    {
                        buildSolution(e.FileName, mazeName);
                    }
                }
                buildDictionary();
                clearFolder();
                applySolution();
            }
        }

        /// <summary>
        /// apply solution
        /// </summary>
        private void applySolution()
        {
            foreach (string mazeName in m_mazes.Keys)
            {
                Maze3d maze = m_mazes[mazeName];
                Solution sol = m_mazeNamesSolution[mazeName];
                SearchableMaze3d searchable = new SearchableMaze3d(maze);
                searchable.markSolutionInGrid(sol);
            }
        }

        /// <summary>
        /// this function help us to clear folder
        /// </summary>
        private void clearFolder()
        {
            foreach (string file in Directory.GetFiles(Directory.GetCurrentDirectory()))
            {
                string fileExtension = file.Substring(file.IndexOf('.') + 1);
                if (fileExtension == "maze" || fileExtension == "sol")
                {
                    File.Delete(file);
                }
            }
        }


        /// <summary>
        /// this function build the dictionary that contain maze as keyand solution as value
        /// </summary>
        private void buildDictionary()
        {
            foreach (string mazeName in m_mazes.Keys)
            {
                m_mazesSolution[m_mazes[mazeName]] = m_mazeNamesSolution[mazeName];
            }
        }


        /// <summary>
        /// this function build maze from compressed file
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="mazeName">maze name</param>
        private void buildMaze(string fileName, string mazeName)
        {
            using (FileStream input = new FileStream(fileName, FileMode.Open))
            {
                using (MyCompressorStream output = new MyCompressorStream(input, MyCompressorStream.compressionMode.decompress))
                {
                    List<byte> mazeBytes = decompressFromFile(output);
                    Maze3d maze = new Maze3d(mazeBytes.ToArray());
                    m_mazes[mazeName] = maze;
                }
            }
        }

        /// <summary>
        /// this function build solution from solution iin file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="mazeName"></param>
        private void buildSolution(string fileName, string mazeName)
        {
            using (StreamReader sr = new StreamReader(fileName))
            {
                string sol = sr.ReadToEnd();
                Solution solution = new Solution(sol);
                m_mazeNamesSolution[mazeName] = solution;
            }
        }

        /// <summary>
        /// this function call from the constructor and init the threads pool 
        /// with values from the settings fields
        /// </summary>
        private void initThreadPool()
        {
            int workerThreads = m_numberOfThreads;
            int completedThreads = m_numberOfThreads;
            ThreadPool.SetMaxThreads(workerThreads, completedThreads);
        }

        /// <summary>
        /// this function activated from thegenerate command 
        /// and call to the function Run that run this job in a new thread(in the thread pool)
        /// </summary>
        /// <param name="x">x length</param>
        /// <param name="y">y length</param>
        /// <param name="z">maze name</param>
        /// <param name="mazeName"></param>
        public void generateMaze(int x, int y, int z, string mazeName)
        {
            string parameters = string.Format("{0},{1},{2}", x, y, z);
            Run("generate3dMaze", mazeName, parameters);
        }

        /// <summary>
        /// this function avtivate the event that say to the presenter that 
        /// the model finish
        /// </summary>
        /// <param name="commandName">command name</param>
        /// <param name="otherInformation">other information</param>
        public void activateEvent(string commandName, string otherInformation)
        {
            ModelChanged(commandName, otherInformation);
        }

        /// <summary>
        /// this function prepare the list that contain all mazes names that exist
        /// </summary>
        public void prepareMazesNames()
        {
            m_mazesNames = new List<string>();
            m_mazesNames.AddRange(m_mazes.Keys);
            ModelChanged("getMazesNames", "done");
        }

        /// <summary>
        /// this function return array of mazesnames
        /// </summary>
        /// <returns>names of the mazes</returns>
        public string[] getMazesNames()
        {
            return m_mazesNames.ToArray();
        }

        /// <summary>
        /// this function return specific maze by the name
        /// </summary>
        /// <param name="nameOfTheMaze"></param>
        /// <returns></returns>
        public object getMaze(string nameOfTheMaze)
        {
            return m_mazes[nameOfTheMaze];
        }

        /// <summary>
        /// this function activate the run functionthat run the solve in a new thread
        /// </summary>
        /// <param name="mazeName"></param>
        /// <param name="algoName"></param>
        public void solveMaze(string mazeName, string algoName)
        {
            Run("solveMaze", mazeName, algoName);
        }

        /// <summary>
        /// this function return specific solution by maze name
        /// </summary>
        /// <param name="mazeName">maze name</param>
        /// <returns>solution</returns>
        public Solution getSolution(string mazeName)
        {
            return m_mazesSolution[m_mazes[mazeName] as Maze3d];
        }

        /// <summary>
        /// this function return the solve time of specific maze
        /// </summary>
        /// <param name="mazeName">mazename</param>
        /// <returns>string of solve time</returns>
        internal string getSolvedTime(string mazeName)
        {
            return m_mazesSolveTime[mazeName];
        }

        /// <summary>
        /// this function help us to save a maze
        /// we use the compress(write) that place in the mazeLib
        /// </summary>
        /// <param name="mazeName">maze name</param>
        /// <param name="mazePath">maze path</param>
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

        /// <summary>
        /// this function return the number of states that the algorithm developed
        /// </summary>
        /// <param name="mazeName">maze name</param>
        /// <returns>number ofstates</returns>
        public int getStatesDeveloped(string mazeName)
        {
            return m_mazesStatesDeveloped[mazeName];
        }

        /// <summary>
        /// this function help us to load a maze
        /// </summary>
        /// <param name="mazePath">maze path</param>
        /// <param name="mazeNameToSave">name to save</param>
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


        /// <summary>
        /// this function which operation to do and activate the function that run this computing in thread pool 
        /// </summary>
        /// <param name="commandName">command name</param>
        /// <param name="mazeName">maze name</param>
        /// <param name="parameters">parameters</param>
        private void Run(string commandName, string mazeName, string parameters)
        {
            switch (commandName)
            {
                case "generate3dMaze":
                    //this maze exist
                    if (m_mazes.ContainsKey(mazeName))
                    {
                        ModelChanged("mazeExist", "The name " + mazeName + " already exist");
                    }
                    else//do not exist
                    {
                        RunInThreadPool(commandName, mazeName, parameters);
                    }
                    break;
                case "solveMaze":
                    //the solution exist
                    if (m_mazesSolution.ContainsKey(m_mazes[mazeName] as Maze3d))
                    {
                        ModelChanged("isExist", mazeName + ",false");
                    }
                    //run in thread pool
                    else
                    {
                        RunInThreadPool(commandName, mazeName, parameters);
                    }
                    break;
            }
        }

        /// <summary>
        /// this function check which operation to doand run it in the thread pool
        /// </summary>
        /// <param name="CommandName">command name</param>
        /// <param name="mazeName">maze name</param>
        /// <param name="parameters">parameters</param>
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

        /// <summary>
        /// this function prepared the info of the solution ofspecific maze name
        ///and call to the event that alarm to the presenter that somthing change
        /// </summary>
        /// <param name="mazeName">maze name</param>
        /// <param name="timeToSolve">time to solve</param>
        /// <param name="algo">algorithm name</param>
        private void getSolutionInfo(string mazeName, string timeToSolve, ISearchingAlgorithm algo)
        {
            m_mazesSolveTime[mazeName] = timeToSolve;
            m_mazesStatesDeveloped[mazeName] = algo.statesDeveloped();
            ModelChanged("solveMaze", mazeName + ",true");
        }

        /// <summary>
        /// this function help us to save maze that have a solution to zip 
        /// </summary>
        public void saveMazesToZip()
        {
            removeZipFile();
            foreach (Maze3d maze in m_mazesSolution.Keys)
            {
                ICompressor compressor = new MyMaze3Dcompressor();
                SearchableMaze3d myMaze = new SearchableMaze3d(maze);
                myMaze.initializeGrid();
                byte[] mazeToSave = compressor.compress(maze.toByteArray());
                //now we want to write the compressed maze to a zip file
                string solutionToSave = m_mazesSolution[maze].ToString();
                string mazeName = m_mazes.FirstOrDefault(x => x.Value == maze).Key;
                m_folderNames.Add(mazeName);
                setFolderAndSaveMaze(mazeToSave, solutionToSave, mazeName);
            }
            createZipFile();
            removeFolders();
        }

        /// <summary>
        /// this function remove the zip file
        /// </summary>
        private void removeZipFile()
        {
            if (File.Exists("Mazes.zip"))
            {
                File.Delete("Mazes.zip");
            }
        }

        /// <summary>
        /// this function remove folder
        /// </summary>
        private void removeFolders()
        {
            foreach (string folder in m_folderNames)
            {
                foreach (string file in Directory.GetFiles(folder))
                {
                    string filePath = folder + @"\" + file;
                    File.Delete(file);
                }
                Directory.Delete(folder);
            }
        }

        /// <summary>
        /// this function create a zip file
        /// </summary>
        private void createZipFile()
        {
            using (ZipFile zip = new ZipFile())
            {
                foreach (string folder in m_folderNames)
                {
                    zip.AddDirectory(folder);
                    zip.Save("Mazes.zip");
                }
            }
        }

        /// <summary>
        /// set folder and save maze
        /// </summary>
        /// <param name="mazeToSave">maze to save in bytes</param>
        /// <param name="solutionToSave">solution to save</param>
        /// <param name="mazeName">maze name</param>
        private void setFolderAndSaveMaze(byte[] mazeToSave, string solutionToSave, string mazeName)
        {
            createFolder(mazeName);
            writeToFolder(mazeToSave, mazeName + @"\" + mazeName + ".maze");
            writeToFolder(solutionToSave, mazeName + @"\" + mazeName + ".sol");
        }

        /// <summary>
        /// this function create a folder
        /// </summary>
        /// <param name="mazeName">maze name</param>
        private void createFolder(string mazeName)
        {
            Directory.CreateDirectory(mazeName);
        }

        /// <summary>
        /// this function write to folder
        /// </summary>
        /// <param name="objectToSave">object to save</param>
        /// <param name="nameToSave">name to save</param>
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

        /// <summary>
        /// this function delete specific maze 
        /// </summary>
        /// <param name="mazeName">maze name</param>
        public void removeMaze(string mazeName)
        {
            if (m_mazesSolution.ContainsKey(m_mazes[mazeName]))
            {
                m_mazesSolution.Remove(m_mazes[mazeName]);
            }
            m_mazes.Remove(mazeName);
            m_mazesNames.Remove(mazeName);
        }

        /// <summary>
        /// this function help us to clear the maze from the memory
        /// </summary>
        public void removeAllMazes()
        {
            m_mazesNames.Clear();
            m_mazesSolution.Clear();
            m_mazes.Clear();
        }
    }
}
