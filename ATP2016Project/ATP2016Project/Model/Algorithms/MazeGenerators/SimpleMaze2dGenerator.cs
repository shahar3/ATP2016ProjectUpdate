using System;
using System.Threading;

namespace ATP2016Project.Model.Algorithms.MazeGenerators
{
    class SimpleMaze2dGenerator : AMazeGenerator
    {
        private Maze2d myMaze;
        private const int PercentOfWalls = 25; //set the chance to break a wall

        /// <summary>
        /// This is the main method in this class
        /// from this method we call all the other helping methods to implement our algorithm
        /// </summary>
        /// <param name="maze"></param>
        /// <param name="algo"></param>
        /// <returns></returns>
        public override Maze generate(IMaze maze, PrimAlgorithm algo)
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
            //surround the maze with walls randomly
            breakWallsRandomlyMaze(PercentOfWalls);
            return myMaze;
        }

        /// <summary>
        /// We iterate through all the walls and break randomly with a given chance rate the walls
        /// </summary>
        /// <param name="percent"></param>
        private void breakWallsRandomlyMaze(int percent)
        {
            for (int i = 0; i < myMaze.XLength; i++)
            {
                for (int j = 0; j < myMaze.YLength; j++)
                {
                    if (myMaze.MazeArray[i, j] == 1)
                    {
                        Thread.Sleep(5);
                        Random rand = new Random();
                        int randomNumber = rand.Next(100);
                        if (randomNumber < percent) //25% to break a wall
                        {
                            myMaze.MazeArray[i, j] = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The wrapper method that calls the recursive method to build a path
        /// </summary>
        private void buildGoalPath()
        {
            buildPathRec(myMaze.StartPoint);
        }

        /// <summary>
        /// given a starting point, this function builds a simple path to
        /// the goal point of the maze
        /// </summary>
        /// <param name="curPoint"></param>
        private void buildPathRec(Position curPoint)
        {
            //the break statement. if the current position is a neighbour of the goal position we finish to build the path
            if (isNeighbour(curPoint))
            {
                return;
            }
            else
            {
                Random rand = new Random();
                Thread.Sleep(6); //to make it completely random
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
                        breakWall(curPoint.X, curPoint.Y - 1, out curPoint);
                        break;
                    //right
                    case 1:
                        breakWall(curPoint.X + 1, curPoint.Y, out curPoint);
                        break;
                    //down
                    case 2:
                        if (curPoint.Y > goalY)
                        {
                            break;
                        }
                        breakWall(curPoint.X, curPoint.Y + 1, out curPoint);
                        break;
                }
                buildPathRec(curPoint);
            }
        }

        /// <summary>
        /// checks a certain position, and if it possible we break the wall (putting 0)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="mazePoint"></param>
        private void breakWall(int x, int y, out Position mazePoint)
        {
            mazePoint = new Position(x, y);
            if (checkIfPossible(x, y))
            {
                myMaze.MazeArray[mazePoint.X, mazePoint.Y] = 0;
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
                myMaze.MazeArray[xStart, yStart] = 2;
                myMaze.MazeArray[xGoal, yGoal] = 3;
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
                        myMaze.MazeArray[i, j] = 1;
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
