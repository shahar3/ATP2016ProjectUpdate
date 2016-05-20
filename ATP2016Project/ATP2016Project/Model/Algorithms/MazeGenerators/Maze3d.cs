using System;
using System.Collections.Generic;

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

        public Maze3d(byte[] unCompressMaze) : this(unCompressMaze[0], unCompressMaze[1], unCompressMaze[2])
        {
            Position startPoint = new Position(unCompressMaze[3], unCompressMaze[4], unCompressMaze[5]);
            Position goalPoint = new Position(unCompressMaze[6], unCompressMaze[7], unCompressMaze[8]);
            this.StartPoint = startPoint;
            this.GoalPoint = goalPoint;
            //
            int lastPosition = 9;
            for (int level = 0; level < ZLength; level++)
            {
                Maze2d maze = Maze2DLayers[level] as Maze2d;
                for (int i = 0; i < maze.Grid.GetLength(0); i++)
                {
                    for (int j = 0; j < maze.Grid.GetLength(1); j++)
                    {
                        maze.Grid[i, j] = unCompressMaze[lastPosition++];
                    }
                }
            }
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
                Console.WriteLine("****LEVEL {0}****", level + 1);
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
                        else if (maze.Grid[i, j] == 0) //there is a space
                        {
                            Console.Write(space);
                        }
                        else //solution path
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.Write(space);
                            Console.ResetColor();
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        public byte[] toByteArray()
        {
            List<byte> byteArray = new List<byte>();
            //add the dimensions,start point and goal point
            //add the dimensions
            byteArray.Add((byte)(this.XLength));
            byteArray.Add((byte)(this.YLength));
            byteArray.Add((byte)(this.ZLength));
            //add the start point
            byteArray.Add((byte)this.StartPoint.X);
            byteArray.Add((byte)this.StartPoint.Y);
            byteArray.Add((byte)this.StartPoint.Z);
            //add the goal point
            byteArray.Add((byte)this.GoalPoint.X);
            byteArray.Add((byte)this.GoalPoint.Y);
            byteArray.Add((byte)this.GoalPoint.Z);
            //add the maze
            for (int i = 0; i < ZLength; i++)
            {
                Maze2d maze = this.Maze2DLayers[i] as Maze2d;
                for (int j = 0; j < maze.Grid.GetLength(0); j++)
                {
                    for (int k = 0; k < maze.Grid.GetLength(1); k++)
                    {
                        byteArray.Add((byte)maze.Grid[j, k]);
                    }
                }
            }
            return byteArray.ToArray();
        }
    }
}
