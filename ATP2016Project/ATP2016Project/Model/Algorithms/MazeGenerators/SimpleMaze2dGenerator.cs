using System;
using System.Threading;

namespace ATP2016Project.Model.Algorithms.MazeGenerators
{
    class SimpleMaze2dGenerator : AMazeGenerator
    {
        private Maze2d myMaze;
        public override Maze generate(IMaze maze)
        {
            //cast the maze to maze2d
            myMaze = maze as Maze2d;
            if (myMaze == null)
            {
                Console.WriteLine("the maze is not a 2d maze");
                return null;
            }
            //init the maze with walls (1)
            initMazeToBeFullWithWalls();
            //set the starting point and goal point to be free (0)
            setStartPointAndGoalPoint();
            //build the goal path
            buildGoalPath();
            //surround with walls randomly
            surroundMaze(20);
            myMaze.print();
            return myMaze;
        }

        private void surroundMaze(int percent)
        {
            for (int i = 0; i < myMaze.XLength; i++)
            {
                for (int j = 0; j < myMaze.YLength; j++)
                {
                    if (myMaze.MazeArray[i, j, 0] == 1)
                    {
                        Thread.Sleep(5);
                        Random rand = new Random();
                        int randomNumber = rand.Next(100);
                        if (randomNumber < percent)
                        {
                            myMaze.MazeArray[i, j, 0] = 0;
                        }
                    }
                }
            }
        }

        private void buildGoalPath()
        {
            buildPathRec(myMaze.StartPoint);
        }

        private void buildPathRec(Position curPoint)
        {
            if (isNeighbour(curPoint))
            {
                return;
            }
            else
            {
                Random rand = new Random();
                Thread.Sleep(6);
                int direction = rand.Next(3);
                int goalX = myMaze.GoalPoint.X;
                int goalY = myMaze.GoalPoint.Y;
                switch (direction)
                {
                    //up
                    case 0:
                        if (curPoint.Y < goalY)
                        {
                            break;
                        }
                        if (checkIfPossible(curPoint.X, curPoint.Y - 1))
                        {
                            curPoint.Y -= 1;
                            myMaze.MazeArray[curPoint.X, curPoint.Y, curPoint.Z] = 0;
                        }
                        break;
                    //right
                    case 1:
                        if (checkIfPossible(curPoint.X + 1, curPoint.Y))
                        {
                            curPoint.X += 1;
                            myMaze.MazeArray[curPoint.X, curPoint.Y, curPoint.Z] = 0;
                        }
                        break;
                    //down
                    case 2:
                        if (curPoint.Y > goalY)
                        {
                            break;
                        }
                        if (checkIfPossible(curPoint.X, curPoint.Y + 1))
                        {
                            curPoint.Y += 1;
                            myMaze.MazeArray[curPoint.X, curPoint.Y, curPoint.Z] = 0;
                        }
                        break;
                }
                buildPathRec(curPoint);
            }
        }

        private bool isNeighbour(Position curPoint)
        {
            int x = curPoint.X, y = curPoint.Y;
            int goalX = myMaze.GoalPoint.X, goalY = myMaze.GoalPoint.Y;
            if (x + 1 == goalX && y == goalY || x - 1 == goalX && y == goalY || y - 1 == goalY && x == goalX || y + 1 == goalY && x == goalX)
            {
                return true;
            }
            return false;
        }

        private bool checkIfPossible(int x, int y)
        {
            int maxX = myMaze.XLength;
            int maxY = myMaze.YLength;
            if (x >= 0 && x < maxX && y >= 0 && y < maxY)
            {
                return true;
            }
            return false;
        }

        private void setStartPointAndGoalPoint()
        {
            try
            {
                chooseStartAndGoalPoints();
                int xStart = myMaze.StartPoint.X;
                int yStart = myMaze.StartPoint.Y;
                int xGoal = myMaze.GoalPoint.X;
                int yGoal = myMaze.GoalPoint.Y;
                myMaze.MazeArray[xStart, yStart, 0] = 0;
                myMaze.MazeArray[xGoal, yGoal, 0] = 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void chooseStartAndGoalPoints()
        {
            Random rnd = new Random();
            //set start point
            myMaze.StartPoint = new Position();
            myMaze.StartPoint.X = 0;
            myMaze.StartPoint.Y = rnd.Next(0, myMaze.YLength);
            //set goal point
            myMaze.GoalPoint = new Position();
            myMaze.GoalPoint.X = myMaze.XLength - 1;
            myMaze.GoalPoint.Y = rnd.Next(0, myMaze.YLength);
        }

        private void initMazeToBeFullWithWalls()
        {
            try
            {
                for (int i = 0; i < myMaze.XLength; i++)
                {
                    for (int j = 0; j < myMaze.YLength; j++)
                    {
                        myMaze.MazeArray[i, j, 0] = 1;
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
