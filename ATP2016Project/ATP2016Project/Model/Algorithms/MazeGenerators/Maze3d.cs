using System;

namespace ATP2016Project.Model.Algorithms.MazeGenerators
{
    class Maze3d : Maze
    {

        /// <summary>
        /// Uses the base constructor for 3d maze in the maze class
        /// </summary>
        /// <param name="xLength"></param>
        /// <param name="yLength"></param>
        /// <param name="zLength"></param>
        public Maze3d(int xLength, int yLength, int zLength) : base(xLength, yLength, zLength)
        {

        }

        /// <summary>
        /// the "smart" algorithm printing method
        /// printing s at start point and e at goal point
        /// wrapping the maze with a frame
        /// </summary>
        public override void print()
        {
            string wall = "██";
            string space = "  ";
            for (int level = 0; level < ZLength; level++)
            {
                Maze2d maze = Maze2DLayers[level] as Maze2d;
                int rowLength = maze.Grid.GetLength(0);
                int colLength = maze.Grid.GetLength(1);
                for (int i = 0; i < rowLength; i++)
                {
                    for (int j = 0; j < colLength; j++)
                    {
                        if (level == 0 && i == StartPoint.X * 2 + 1 && j == StartPoint.Y * 2 + 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("S ");
                            Console.ResetColor();
                        }
                        else if (level == ZLength - 1 && i == GoalPoint.X * 2 + 1 && j == GoalPoint.Y * 2 + 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("E ");
                            Console.ResetColor();
                        }
                        else if (maze.Grid[i, j] == 1) //put wall
                        {
                            Console.Write(wall);
                        }
                        else //there is a space
                        {
                            Console.Write(space);
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}
