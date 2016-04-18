using ATP2016Project.Model.Algorithms.MazeGenerators;
using ATP2016Project.Model.Algorithms.Search;
using System;

namespace ATP2016Project
{
    class Program
    {
        static void Main(string[] args)
        {
            //(new SimpleMaze2dGenerator());
            //testMaze3dGenerator(new MyMaze3dGenerator());
            testSearchAlgorithms();
            Console.ReadKey();
        }

        private static void testSearchAlgorithms()
        {
            ASearchingAlgorithm dfs = new DepthFirstSearch();
            AMazeGenerator mg = new MyMaze3dGenerator();
            Maze maze = mg.generate(19, 19, 2);
            ISearchable searchable = new SearchableMaze3d(maze);
            maze.print();
            dfs.search(searchable);
            maze.print();
        }

        private static void testMaze3dGenerator(MyMaze3dGenerator mg)
        {
            Console.WriteLine(mg.measureAlgorithmTime(19, 19, 2));
            Maze maze = mg.generate(19, 19, 2);
            maze.getStartPosition().print();
            maze.getGoalPosition().print();
            maze.print();

        }

        private static void testMaze2dGenerator(IMazeGenerator mg)
        {
            Console.WriteLine(mg.measureAlgorithmTime(19, 19, 0));
            Maze maze = mg.generate(19, 19, 0);
            Position start = maze.getStartPosition();
            start.print();
            maze.getGoalPosition().print();
            maze.print();
        }
    }
}