using System;
using ATP2016Project.Model.Algorithms.MazeGenerators;

namespace ATP2016Project
{
    class Program
    {
        static void Main(string[] args)
        {
            testMaze2dGenerator(new SimpleMaze2dGenerator());
        }

        private static void testMaze2dGenerator(IMazeGenerator mg)
        {
            Console.WriteLine(mg.measureAlgeorithemTime());
            Maze maze = mg.generate();
            Position start = maze.getStartPosition();
            start.print();
            maze.getGoalPosition().print();
            maze.print();
            throw new NotImplementedException();
        }
    }
}