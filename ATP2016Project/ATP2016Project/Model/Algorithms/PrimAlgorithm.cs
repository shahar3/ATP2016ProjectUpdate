using System;
using System.Collections;

namespace ATP2016Project.Model.Algorithms.MazeGenerators
{
    class PrimAlgorithm
    {
        private ArrayList m_moves;
        private Maze myMaze;
        private ArrayList m_closePositions, m_neighbours;
        private int[,] grid;

        public PrimAlgorithm(Maze maze)
        {
            myMaze = maze;
            grid = new int[maze.XLength * 2 - 1, maze.YLength * 2 - 1];
            startGenerating();
        }

        private void startGenerating()
        {
            int numberOfPositions = myMaze.XLength * myMaze.YLength;
            chooseRandomStartPosition();
            m_closePositions.Add(myMaze.StartPoint);
            Position curPos = myMaze.StartPoint;
            Position nextPos = new Position();
            while (m_closePositions.Count < numberOfPositions)
            {
                m_neighbours = findNeighbours();
                nextPos = chooseOneNeighbourRandomly();
                breakWallBetweenCells(curPos, nextPos);
            }
        }

        private void breakWallBetweenCells(Position curPos, Position nextPos)
        {
            throw new NotImplementedException();
        }

        private void breakWallBetweenCells(Position nextPos)
        {
            throw new NotImplementedException();
        }

        private ArrayList findNeighbours()
        {
            throw new NotImplementedException();
        }

        private Position chooseOneNeighbourRandomly()
        {
            throw new NotImplementedException();
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


        /// <summary>
        /// getters and setters that contain the moves that the algorithm choose in the same order
        /// </summary>
        public ArrayList Moves
        {
            get { return m_moves; }
            set { m_moves = value; }
        }


    }
}
