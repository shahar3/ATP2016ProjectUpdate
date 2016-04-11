using ATP2016Project.Model.Algorithms.MazeGenerators;
using ATP2016Project.Model.Algorithms.Search;
using System;

namespace ATP2016Project
{
    class Program
    {
        static void Main(string[] args)
        {
            //testMaze2dGenerator(new SimpleMaze2dGenerator(new pr(new maze()));
            //IMazeGenerator mg = new SimpleMaze2dGenerator();
            //mg.generate(new Maze2d(30, 30), null);
            IMazeGenerator mg = new MyMaze3dGenerator();
            //maze = mg.generate(maze, new PrimAlgorithm(maze));
            //Console.WriteLine(mg.measureAlgorithmTime(20, 20, 4));
            Maze maze = mg.generate(15, 15, 4);
            maze.print();
            IMazeGenerator mg2 = new SimpleMaze2dGenerator();
            //Console.WriteLine(mg2.measureAlgorithmTime(15, 15, 0));
            ISearchable searchable = new SearchableMaze3d(maze);
            ISearchingAlgorithm bfs = new BreadthFirstSearch();
            bfs.search(searchable);
            Console.ReadKey();
        }

        //private static void testMaze2dGenerator(IMazeGenerator mg)
        //{
        //    Console.WriteLine(mg.measureAlgeorithemTime());
        //    Maze maze = mg.generate();
        //    Position start = maze.getStartPosition();
        //    start.print();
        //    maze.getGoalPosition().print();
        //    maze.print();
        //    throw new NotImplementedException();
        //}
    }
}