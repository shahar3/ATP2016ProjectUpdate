using System;

namespace ATP2016Project.Model.Algorithms.MazeGenerators
{
    class SimpleMaze2dGenerator : AMazeGenerator
    {
        public override Maze generate(IMaze maze)
        {
            //cast the maze to maze2d
            Maze2d maze2d = maze as Maze2d;
            if (maze2d == null)
            {
                Console.WriteLine("the maze is not a 2d maze");
                return null;
            }
            //init the maze with walls (1)
            initMazeToBeFullWithWalls(maze2d);
            //set the starting point and goal point to be free (0)
            setStartPointAndGoalPoint(maze2d);
            //build the goal path
            buildGoalPath(maze2d);
            //surround with walls randomly
            return null;
        }

        private void buildGoalPath(Maze2d maze)
        {
            buildPathRec(maze, maze.StartPoint);
        }

        private void buildPathRec(Maze2d maze, Position curPoint)
        {
            if (curPoint.Equals(maze.GoalPoint))
            {
                return;
            }
            else
            {
                Random rand = new Random();
                int direction = rand.Next(0, 3);
                switch (direction)
                {
                    //up
                    case 0:
                        curPoint.Y -= 1;
                        if (checkIfPossible(maze, curPoint))
                        {
                            maze.MazeArray[curPoint.X, curPoint.Y, curPoint.Z] = 0;
                        }
                        break;
                    //right
                    case 1:
                        curPoint.X += 1;
                        if (checkIfPossible(maze, curPoint))
                        {
                            maze.MazeArray[curPoint.X, curPoint.Y, curPoint.Z] = 0;
                        }
                        break;
                    //down
                    case 2:
                        curPoint.Y += 1;
                        if (checkIfPossible(maze, curPoint))
                        {
                            maze.MazeArray[curPoint.X, curPoint.Y, curPoint.Z] = 0;
                        }
                        break;
                        //left
                        curPoint.X -= 1;
                    case 3:
                        if (checkIfPossible(maze, curPoint))
                        {
                            maze.MazeArray[curPoint.X, curPoint.Y, curPoint.Z] = 0;
                        }
                        break;
                }
                buildPathRec(maze, curPoint);
            }
        }

        private bool checkIfPossible(Maze2d maze, Position curPoint)
        {
            int maxX = maze.XLength;
            int maxY = maze.YLength;
            if (curPoint.X >= 0 && curPoint.X < maxX && curPoint.Y >= 0 && curPoint.Y < maxY)
            {
                return true;
            }
            return false;
        }

        private void setStartPointAndGoalPoint(Maze2d maze)
        {
            try
            {
                chooseStartAndGoalPoints(maze);
                int xStart = maze.StartPoint.X;
                int yStart = maze.StartPoint.Y;
                int xGoal = maze.GoalPoint.X;
                int yGoal = maze.GoalPoint.Y;
                maze.MazeArray[xStart, yStart, 0] = 0;
                maze.MazeArray[xGoal, yGoal, 0] = 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void chooseStartAndGoalPoints(Maze2d maze)
        {
            Random rnd = new Random();
            //set start point
            maze.StartPoint.X = rnd.Next(0, maze.XLength - 1);
            maze.StartPoint.Y = rnd.Next(0, maze.YLength - 1);
            //set goal point
            maze.GoalPoint.X = rnd.Next(0, maze.XLength - 1);
            maze.GoalPoint.Y = rnd.Next(0, maze.YLength - 1);
        }

        private void initMazeToBeFullWithWalls(Maze2d maze)
        {
            try
            {
                for (int i = 0; i < maze.XLength; i++)
                {
                    for (int j = 0; j < maze.YLength; j++)
                    {
                        maze.MazeArray[i, j, 0] = 1;
                    }
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }
}
