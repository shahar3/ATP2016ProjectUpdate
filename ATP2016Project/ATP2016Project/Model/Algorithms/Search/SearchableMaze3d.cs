using ATP2016Project.Model.Algorithms.MazeGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATP2016Project.Model.Algorithms.Search
{
    class SearchableMaze3d : ISearchable
    {
        private Maze m_maze;
        private List<AState> m_successors;

        public SearchableMaze3d(IMaze maze)
        {
            m_maze = maze as Maze;
        }

        public Maze MyMaze
        {
            get { return m_maze; }
            set { m_maze = value; }
        }

        public List<AState> getAllPossibleStates(AState state)
        {
            Position currentPosition = (state as MazeState).Position;
            m_successors = new List<AState>();
            //up position
            Position upPosition = new Position(currentPosition.X - 1, currentPosition.Y, currentPosition.Z);
            if (checkIfValid(upPosition) && !checkIfThereIsWall(upPosition))
            {
                AState stateToAdd = new MazeState(upPosition, state);
                m_successors.Add(stateToAdd);
            }
            //down position
            Position downPosition = new Position(currentPosition.X + 1, currentPosition.Y, currentPosition.Z);
            if (checkIfValid(downPosition) && !checkIfThereIsWall(downPosition))
            {
                AState stateToAdd = new MazeState(downPosition, state);
                m_successors.Add(stateToAdd);
            }
            //left position
            Position leftPosition = new Position(currentPosition.X, currentPosition.Y - 1, currentPosition.Z);
            if (checkIfValid(leftPosition) && !checkIfThereIsWall(leftPosition))
            {
                AState stateToAdd = new MazeState(leftPosition, state);
                m_successors.Add(stateToAdd);
            }
            //right position
            Position rightPosition = new Position(currentPosition.X, currentPosition.Y + 1, currentPosition.Z);
            if (checkIfValid(rightPosition) && !checkIfThereIsWall(rightPosition))
            {
                AState stateToAdd = new MazeState(rightPosition, state);
                m_successors.Add(stateToAdd);
            }
            return m_successors;

        }

        private bool checkIfThereIsWall(Position newPosition)
        {
            int level = newPosition.Z;
            return (m_maze.Maze2DLayers[level] as Maze).Grid[newPosition.X, newPosition.Y] == 1;
        }

        private bool checkIfValid(Position posToCheck)
        {
            return posToCheck.X > 0 && posToCheck.X < m_maze.XLength * 2 + 1 && posToCheck.Y > 0 && posToCheck.Y < m_maze.YLength * 2 + 1;
        }

        public AState getGoalState()
        {
            AState goalState = new MazeState(new Position(m_maze.GoalPoint.X * 2 + 1, m_maze.GoalPoint.Y * 2 + 1), null);
            return goalState;
        }

        public AState getInitialState()
        {
            AState initialState = new MazeState(new Position(m_maze.StartPoint.X * 2 + 1, m_maze.StartPoint.Y * 2 + 1), null);
            return initialState;
        }

        public void markInGrid(MazeState currentState)
        {
            Position position = (currentState as MazeState).Position;
            (this.MyMaze.Maze2DLayers[position.Z] as Maze).Grid[position.X, position.Y] = 2;
        }

    }
}
