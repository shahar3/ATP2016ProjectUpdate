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
            ASearchingAlgorithm dfs = new DepthFirstSearch();
            AMazeGenerator mg = new MyMaze3dGenerator();
            Maze maze = mg.generate(5, 5, 4);
            ISearchable searchable = new SearchableMaze3d(maze);
            maze.print();
            Solution solution = dfs.search(searchable);
            solution.printSolution();
            (searchable as SearchableMaze3d).markSolutionInGrid(solution);
            maze.print();
            Console.WriteLine(dfs.timeToSolve(searchable));
            Console.WriteLine(dfs.statesDeveloped());
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