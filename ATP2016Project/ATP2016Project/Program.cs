using ATP2016Project.Model.Algorithms.MazeGenerators;
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
            IMaze maze = new Maze3d(25, 25, 2);
            //maze = mg.generate(maze, new PrimAlgorithm(maze));
            //maze.print();
            Console.WriteLine(mg.measureAlgorithmTime(maze, new PrimAlgorithm(maze)));
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