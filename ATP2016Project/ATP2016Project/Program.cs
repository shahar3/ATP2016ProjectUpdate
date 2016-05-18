using ATP2016Project.Model.Algorithms.Compression;
using ATP2016Project.Model.Algorithms.MazeGenerators;
using ATP2016Project.Model.Algorithms.Search;
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
            testCompressorStream();
            Console.ReadKey();
        }

        private static void testCompressorStream()
        {
            IMazeGenerator mg = new MyMaze3dGenerator();
            IMaze maze = mg.generate(8, 8, 2);
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
            ICompressor c = new MyMaze3Dcompressor();
            byte[] compressedOriginal = c.compress(original);
            int additional = (original.Length / 100) * 2;
            byte[] compressedFile;
            using (FileStream fileInStream = new FileStream("1.maze", FileMode.Open))
            {
                compressedFile = new byte[compressedOriginal.Length + additional];
                fileInStream.Read(compressedFile, 0, compressedFile.Length);
            }

            byte[] decompressedFile = c.decompress(compressedFile);
            int min = Math.Min(original.Length, decompressedFile.Length);
            for (int i = 0; i < min; i++)
            {
                if (original[i] != decompressedFile[i])
                {
                    Console.WriteLine("Not good");
                    break;
                }
            }

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

            Maze3d loadedMaze = new Maze3d(mazeBytes);
            maze.print();
            Console.WriteLine();
            loadedMaze.print();
        }

        private static void testCompressor()
        {
            ICompressor c = new MyMaze3Dcompressor();
            //byte[] byteArray = { 1, 1, 1, 0, 0, 0, 1, 0, 0, 0 };
            //byte[] sol = c.compress(byteArray);
            //foreach (byte b in sol)
            //{
            //    Console.Write(b);
            //}
            //Console.WriteLine();

            //Console.WriteLine(byte.MaxValue);

            //byte[] d = c.decompress(sol);
            //foreach (byte b in d)
            //{
            //    Console.Write(b);
            //}
            //test compress maze
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