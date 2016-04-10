using System;

namespace ATP2016Project.Model.Algorithms.MazeGenerators
{
    class Maze2d : Maze
    {
        /// <summary>
        /// Use the base constructor from the maze class
        /// </summary>
        /// <param name="xLength"></param>
        /// <param name="yLength"></param>
        public Maze2d(int xLength, int yLength) : base(xLength, yLength)
        {
        }

        /// <summary>
        /// the default constructor
        /// </summary>
        public Maze2d() : base()
        {

        }

        /// <summary>
        /// the naive algorithm printing method
        /// </summary>
        public override void print()
        {
            {
                string empty = " ";
                string wall = "█";
                for (int j = 0; j < YLength; j++)
                {
                    for (int k = 0; k < XLength; k++)
                    {
                        if (new Position(k, j, 0).Equals(this.StartPoint))
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write("S");
                            Console.ResetColor();
                        }
                        else if (new Position(k, j, 0).Equals(this.GoalPoint))
                        {
                            Console.Write("E");
                        }
                        else if (this.MazeArray[k, j] == 0)
                        {
                            Console.Write(empty);
                        }
                        else
                        {
                            Console.Write(wall);
                        }
                    }
                    Console.WriteLine("");
                }
                Console.WriteLine("");
                Console.WriteLine("");
            }
        }
    }
}
