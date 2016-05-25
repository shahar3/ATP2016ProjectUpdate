using ATP2016Project.Controller;
using ATP2016Project.Model;
using ATP2016Project.Model.Algorithms.Compression;
using ATP2016Project.Model.Algorithms.MazeGenerators;
using ATP2016Project.Model.Algorithms.Search;
using ATP2016Project.View;
using System;
using System.IO;

namespace ATP2016Project
{
    class Program
    {
        /// <summary>
        /// our main method
        /// from here we calling our tester functions
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //testMaze2dGenerator(new SimpleMaze2dGenerator());
            //testMaze3dGenerator(new MyMaze3dGenerator());
            //testSearchAlgorithms();
            //testCompressor();
            //testCompressorStream();
            RunCLI(); //our user command line interface
            Console.ReadKey();
        }

        #region CLI

        private static void RunCLI()
        {
            IController controller = new MyController();
            IModel model = new MyModel(controller);
            controller.SetModel(model);
            IView view = new CLI(controller, controller.GetCommands());
            controller.SetView(view);
            view.Start();
        }

        #endregion

        #region compressor testing

        private static void testCompressorStream()
        {
            //write compressed maze
            IMazeGenerator mg = new MyMaze3dGenerator();
            IMaze maze = mg.generate(18, 18, 2);
            byte[] original = (maze as Maze3d).toByteArray();
            using (FileStream fileOutStream = new FileStream("1.Maze", FileMode.Create))
            {
                using (Stream inputStream = new MemoryStream(original))
                {
                    using (Stream outStream = new MyCompressorStream(fileOutStream, MyCompressorStream.compressionMode.compress))
                    {
                        byte[] byteArray = new byte[100];
                        int r = 0;
                        while ((r = inputStream.Read(byteArray, 0, byteArray.Length)) != 0)
                        {
                            outStream.Write(byteArray, 0, 100);
                            outStream.Flush();
                            byteArray = new byte[100];
                        }
                    }
                }
            }
            //check if the compressed file is ok
            ICompressor c = new MyMaze3Dcompressor();
            byte[] compressedOriginal = c.compress(original);
            int additional = (original.Length / 100) * 2;
            byte[] compressedFile;
            using (FileStream fileInStream = new FileStream("1.maze", FileMode.Open))
            {
                compressedFile = new byte[compressedOriginal.Length + additional];
                fileInStream.Read(compressedFile, 0, compressedFile.Length);
            }
            //write decompressed maze
            using (FileStream fileOutStream = new FileStream("1Dec.Maze", FileMode.Create))
            {
                using (Stream inputStream = new FileStream("1.Maze", FileMode.Open))
                {
                    using (Stream outStream = new MyCompressorStream(fileOutStream, MyCompressorStream.compressionMode.decompress))
                    {
                        byte[] byteArray = new byte[100];
                        int r = 0;
                        while ((r = inputStream.Read(byteArray, 0, byteArray.Length)) != 0)
                        {
                            outStream.Write(byteArray, 0, 100);
                            outStream.Flush();
                            byteArray = new byte[100];
                        }
                    }
                }
            }
            //read decompressed maze
            byte[] mazeBytes;
            byte[] buffer;
            using (FileStream fileInStream = new FileStream("1.maze", FileMode.Open))
            {
                using (Stream inStream = new MyCompressorStream(fileInStream, MyCompressorStream.compressionMode.decompress))
                {
                    mazeBytes = new byte[(maze as Maze3d).toByteArray().Length];
                    buffer = new byte[100];
                    int mult = 0;
                    int r = 0;
                    while ((r = inStream.Read(buffer, 0, 100)) != 0)
                    {
                        for (int i = 0; (i + 100 * mult) < mazeBytes.Length && i < buffer.Length; i++)
                        {
                            mazeBytes[i + 100 * mult] = buffer[i];
                        }
                        buffer = new byte[100];
                        mult++;
                    }
                }
            }

            string print = original.Length == mazeBytes.Length ?
                "The Original maze and the decompressed maze bytes array are in the same size" : "The Original maze and the decompressed maze bytes array are not in the same size";
            Console.WriteLine(print);
            bool ok = true;
            for (int i = 0; original.Length == mazeBytes.Length && i < mazeBytes.Length; i++)
            {
                if (mazeBytes[i] != original[i])
                {
                    ok = false;
                    break;
                }
            }
            Console.WriteLine(ok);
            Maze3d loadedMaze = new Maze3d(mazeBytes);
            maze.print();
            Console.WriteLine();
            loadedMaze.print();
            compressingEfficiency(original, compressedFile);
        }

        private static void compressingEfficiency(byte[] original, byte[] compressedFile)
        {
            Console.WriteLine("Statistics:");
            Console.WriteLine("Original maze bytes length: {0}, compressed maze in file length: {1}", original.Length, compressedFile.Length);
            Console.WriteLine("The compressed file saved total of {0} bytes and reduced {1}% of the size of the file", original.Length - compressedFile.Length, (int)(((double)original.Length / (double)compressedFile.Length) * 100));
        }

        private static void testCompressor()
        {
            ICompressor c = new MyMaze3Dcompressor();
            IMazeGenerator mg = new MyMaze3dGenerator();
            IMaze maze = mg.generate(5, 5, 2);
            byte[] originalArray = (maze as Maze3d).toByteArray();
            byte[] compressArray = c.compress(originalArray);
            byte[] byArray = c.decompress(compressArray);
            bool sameSize = byArray.Length == originalArray.Length;
            if (sameSize)
            {
                bool ok = true;
                for (int i = 0; i < originalArray.Length; i++)
                {
                    if (originalArray[i] != byArray[i])
                    {
                        ok = false;
                        break;
                    }
                }
                Console.WriteLine("OK = {0}", ok);
            }
            maze.print();
            Console.WriteLine();
            IMaze mazeFromBytes = new Maze3d(originalArray);
            mazeFromBytes.print();
        }

        #endregion

        #region maze searching tester
        /// <summary>
        /// build to test the searching algorithm. allow us to compare between algorithms
        /// according to their perfomances
        /// </summary>
        private static void testSearchAlgorithms()
        {
            //create the searching algorithms that we want to test
            ASearchingAlgorithm dfs = new DepthFirstSearch();
            ASearchingAlgorithm bfs = new BreadthFirstSearch();
            //create the 3d maze that we perform our testings on
            AMazeGenerator mg = new MyMaze3dGenerator();
            Maze maze = mg.generate(15, 15, 4);
            //object adapter
            ISearchable searchable = new SearchableMaze3d(maze);
            Console.WriteLine("The original maze");
            maze.print();
            //help us to compare between algorithms
            int numOfStatesDFS = 0;
            int numOfStatesBFS = 0;
            string timeDFS = string.Empty;
            string timeBFS = string.Empty;
            efficiencyTest(searchable, dfs, "DFS", maze, out numOfStatesDFS, out timeDFS);
            Console.WriteLine("Enter any key to continue");
            Console.Clear();
            //clear the maze
            searchable.initializeGrid();
            efficiencyTest(searchable, bfs, "BFS", maze, out numOfStatesBFS, out timeBFS);
            //compare the algorithms and summerize
            Console.Clear();
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("--------------------SUMMERIZE----------------------");
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine();
            if (numOfStatesDFS < numOfStatesBFS)
            {
                Console.WriteLine("DFS developed {0} less states than BFS", numOfStatesBFS - numOfStatesDFS);
            }
            else
            {
                Console.WriteLine("BFS developed {0} less states than BFS", numOfStatesDFS - numOfStatesBFS);
            }
            if (Double.Parse(timeDFS) < Double.Parse(timeBFS))
            {
                Console.WriteLine("DFS took {0} seconds less to run than BFS", Double.Parse(timeBFS) - Double.Parse(timeDFS));
            }
            else
            {
                Console.WriteLine("BFS took {0} seconds less to run than DFS", Double.Parse(timeDFS) - Double.Parse(timeBFS));
            }
        }

        /// <summary>
        /// help us to check the algorithm by his running time, states developed and 
        /// different aspects
        /// </summary>
        /// <param name="searchable"></param>
        /// <param name="dfs"></param>
        /// <param name="algoName"></param>
        private static void efficiencyTest(ISearchable searchable, ASearchingAlgorithm algo, string algoName, Maze maze, out int statesDeveloped, out string timeElapsed)
        {
            Console.WriteLine("Testing {0}:", algoName);
            Console.WriteLine("Enter any key to continue");
            Console.ReadKey(true);
            Solution solution = algo.search(searchable);
            Console.WriteLine("The solution path (step by step)");
            solution.printSolution();
            Console.WriteLine("Do you want to see a visual represantion of the solution?(y/n)");
            char ans = Console.ReadKey(true).KeyChar;
            if (ans == 'y' || ans == 'Y')
            {
                (searchable as SearchableMaze3d).markSolutionInGrid(solution);
                Console.WriteLine("Printing solution path");
                maze.print();
            }
            Console.WriteLine("Enter any key to continue");
            Console.ReadKey(true);
            Console.Write("The {0} developed ", algoName);
            statesDeveloped = algo.statesDeveloped();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(algo.statesDeveloped());
            Console.ResetColor();
            Console.WriteLine(" states during his running time");
            Console.WriteLine("Checking the time to solve...");
            string timeToSolve = algo.timeToSolve(searchable);
            Console.WriteLine(timeToSolve);
            int lastIndex = timeToSolve.IndexOf(" seconds");
            timeElapsed = timeToSolve.Substring(8, lastIndex - 8);
            Console.WriteLine("Enter any key to continue");
            Console.ReadKey(true);
        }

        #endregion

        #region maze creation testers
        private static void testMaze3dGenerator(MyMaze3dGenerator mg)
        {
            Console.WriteLine(mg.measureAlgorithmTime(19, 19, 2));
            Maze maze = mg.generate(19, 19, 2);
            Console.Write("The start position is: ");
            maze.getStartPosition().print();
            Console.Write("The goal position is: ");
            maze.getGoalPosition().print();
            maze.print();
        }

        private static void testMaze2dGenerator(IMazeGenerator mg)
        {
            Console.WriteLine(mg.measureAlgorithmTime(19, 19, 0));
            Maze maze = mg.generate(19, 19, 0);
            Position start = maze.getStartPosition();
            Console.Write("The start position is: ");
            start.print();
            Console.Write("The goal position is: ");
            maze.getGoalPosition().print();
            maze.print();
        }
        #endregion
    }
}