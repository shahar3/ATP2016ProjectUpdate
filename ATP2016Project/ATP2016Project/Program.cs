using ATP2016Project.Model.Algorithms.MazeGenerators;
using ATP2016Project.Model.Algorithms.Search;
using System;

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
            testSearchAlgorithms();
            Console.ReadKey();
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
            Maze maze = mg.generate(5, 5, 4);
            //object adapter
            ISearchable searchable = new SearchableMaze3d(maze);
            Console.WriteLine("The original maze");
            maze.print();
            efficiencyTest(searchable, dfs, "DFS", maze);
            Console.WriteLine("Enter any key to continue");
            Console.Clear();
            //clear the maze
            searchable.initializeGrid();
            efficiencyTest(searchable, bfs, "BFS", maze);
        }

        /// <summary>
        /// help us to check the algorithm by his running time, states developed and 
        /// different aspects
        /// </summary>
        /// <param name="searchable"></param>
        /// <param name="dfs"></param>
        /// <param name="algoName"></param>
        private static void efficiencyTest(ISearchable searchable, ASearchingAlgorithm algo, string algoName, Maze maze)
        {
            Console.WriteLine("Testing {0}:", algoName);
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
            Console.WriteLine("The {0} developed {1} states during his running time", algoName, algo.statesDeveloped());
            Console.WriteLine("Checking the time to solve...");
            Console.WriteLine(algo.timeToSolve(searchable));
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