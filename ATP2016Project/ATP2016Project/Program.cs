using ATP2016Project.Model.Algorithms.MazeGenerators;
using System;

namespace ATP2016Project
{
    class Program
    {
        static void Main(string[] args)
        {
            //testMaze2dGenerator(new SimpleMaze2dGenerator());
            IMazeGenerator mg = new SimpleMaze2dGenerator();
            mg.generate(new Maze2d(12, 12));
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