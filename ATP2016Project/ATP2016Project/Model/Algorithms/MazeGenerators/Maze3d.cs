﻿using System;

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
                //the upper side of the frame
                for (int i = 0; i < colLength + 2; i++)
                {
                    Console.Write(wall);
                }
                Console.WriteLine();
                for (int i = 0; i < rowLength; i++)
                {
                    for (int j = 0; j < colLength; j++)
                    {

                        if (j == 0) //part of the frame
                        {
                            Console.Write(wall);
                        }
                        //if (maze.GoalPoint!=null&i == GoalPoint.X * 2 && j == GoalPoint.Y * 2)
                        //{
                        //    Console.Write("E "); //goal point
                        //}
                        //else if (maze.StartPoint!=null&i == StartPoint.X * 2 && j == StartPoint.Y * 2)
                        //{
                        //    Console.Write("S "); //start point
                        //}
                        if (maze.Grid[i, j] == 1) //put space
                        {
                            Console.Write(space);
                        }
                        else //there is a wall
                        {
                            Console.Write(wall);
                        }
                        if (j == colLength - 1) //part of the frame
                        {
                            Console.Write(wall);
                        }
                    }
                    Console.WriteLine();
                }
                //the lower side of the frame
                for (int i = 0; i < colLength + 2; i++)
                {
                    Console.Write(wall);
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}
