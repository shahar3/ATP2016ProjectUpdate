using System;
using System.Collections;
using System.Threading;

namespace ATP2016Project.Model.Algorithms.MazeGenerators
{
    /// <summary>
    /// Implementation of prim algorithm (modified version)
    /// we followed the steps:
    /// 1-choose a random point in the maze
    /// 2-add the point to close list
    /// 3-while the close list doesn't have all the points of the maze do:
    /// 3.1-find all the neighbors of the points in the close list
    /// 3.2-choose one randomly and "break the wall" between him and the most adjacent point in the close list
    /// 3.3-in the case there are more than one adjacent point, choose one randomly
    /// 3.4-add the neighbour you chose to the close list
    /// 4-return the grid as the generated maze
    /// </summary>
    class PrimAlgorithm
    {
        private Maze myMaze;
        private ArrayList m_closePositions, m_neighbours;
        private int[,] grid;

        public PrimAlgorithm(IMaze maze)
        {
            myMaze = maze as Maze;
            grid = new int[myMaze.XLength * 2 - 1, myMaze.YLength * 2 - 1];
            m_closePositions = new ArrayList();
            m_neighbours = new ArrayList();
            initTheMazeGridToBeEmpty(); //mark the cells with 1's
        }

        private void initTheMazeGridToBeEmpty()
        {
            int rows = myMaze.MazeArray.GetLength(0);
            int cols = myMaze.MazeArray.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    grid[i * 2, j * 2] = 1;
                }
            }
        }

        public void startGenerating()
        {
            int numberOfPositions = myMaze.XLength * myMaze.YLength;
            chooseRandomStartPosition();
            addPointToTheMaze(myMaze.StartPoint);
            Position nextPos = new Position();
            while (m_closePositions.Count < numberOfPositions)
            {
                findNeighbours();
                nextPos = chooseOneNeighbourRandomly();
                addPointToTheMaze(nextPos);
                breakWallBetweenCells(nextPos);
            }
        }


        private void addPointToTheMaze(Position posToAdd)
        {
            //grid[posToAdd.X, posToAdd.Y] = 1;
            m_closePositions.Add(posToAdd);
        }

        private void breakWallBetweenCells(Position curPos)
        {
            Position adjacent = findAdjacentCell(curPos);
            //we mark the cell with 1 to indicate a broken wall
            calculatePositionOfTheWall(curPos, adjacent);
        }

        private void calculatePositionOfTheWall(Position curPos, Position adjacent)
        {
            //they are in the same row
            if (curPos.X == adjacent.X)
            {
                //we use the formula minY*2+1 = the place to break
                int placeToBreak = Math.Min(curPos.Y, adjacent.Y) * 2 + 1;
                //break
                grid[curPos.X * 2, placeToBreak] = 1;
            }
            //they are in the same column
            else
            {
                int placeToBreak = Math.Min(curPos.X, adjacent.X) * 2 + 1;
                //break
                grid[placeToBreak, curPos.Y * 2] = 1;
            }
        }

        private Position findAdjacentCell(Position curPos)
        {
            Position positionReturn = new Position();
            ArrayList neighboursInCloseList = new ArrayList();
            foreach (Position pos in m_closePositions)
            {
                if (curPos.isNeighbour(pos))
                {
                    neighboursInCloseList.Add(pos);
                }
            }
            return chooseNeighbourToBreakWallWithRandomly(neighboursInCloseList);
        }

        private Position chooseNeighbourToBreakWallWithRandomly(ArrayList neighboursInCloseList)
        {
            int numOfNeighboursToChooseFrom = neighboursInCloseList.Count;
            Random r = new Random();
            int randomNumber = r.Next(numOfNeighboursToChooseFrom);
            return neighboursInCloseList[randomNumber] as Position;
        }

        private void findNeighbours()
        {
            int closePositionsSize = m_closePositions.Count - 1;
            Position currentPositin = m_closePositions[closePositionsSize] as Position;
            //find right neighbour and add to arrayList
            Position rightPosition = new Position(currentPositin.X, currentPositin.Y + 1, currentPositin.Z);
            //check if this point eligible
            isEligible(rightPosition);
            //find left neighbour and add to arrayList
            Position leftPosition = new Position(currentPositin.X, currentPositin.Y - 1, currentPositin.Z);
            isEligible(leftPosition);
            //find above neighbour and add to araayList
            Position abovePosition = new Position(currentPositin.X - 1, currentPositin.Y, currentPositin.Z);
            isEligible(abovePosition);
            //find below neighbour and add to arrayList
            Position belowPositin = new Position(currentPositin.X + 1, currentPositin.Y, currentPositin.Z);
            isEligible(belowPositin);
        }

        private void isEligible(Position posToCheck)
        {
            if ((!ClosePositions.Contains(posToCheck)) && (!m_neighbours.Contains(posToCheck)) && isInsideTheMaze(posToCheck))
            {
                m_neighbours.Add(posToCheck);
            }
        }



        private bool isInsideTheMaze(Position posToCheck)
        {
            if (posToCheck.X < 0 || posToCheck.X >= myMaze.XLength || posToCheck.Y < 0 || posToCheck.Y >= myMaze.YLength)
            {
                return false;
            }
            return true;
        }

        private Position chooseOneNeighbourRandomly()
        {
            int numOfNeighbours = m_neighbours.Count;
            Random r = new Random();
            Thread.Sleep(1);
            int randomNeigbourNum = r.Next(numOfNeighbours);
            Position neighbour = m_neighbours[randomNeigbourNum] as Position;
            m_neighbours.Remove(neighbour);
            return neighbour;
        }

        private void chooseRandomStartPosition()
        {
            myMaze.StartPoint = new Position();
            Random r = new Random();
            int row = 0;
            int col = r.Next(myMaze.YLength);
            myMaze.StartPoint.X = row;
            myMaze.StartPoint.Y = col;
        }

        public ArrayList ClosePositions
        {
            get { return m_closePositions; }
            set { m_closePositions = value; }
        }

        public int[,] Grid
        {
            get { return grid; }
            set { grid = value; }
        }
    }
}
