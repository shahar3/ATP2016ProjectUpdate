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

        public List<AState> getAllPossibleStates(AState state)
        {
            Position currentPosition = (state as MazeState).Position;
            m_successors = new List<AState>();
            Position newPosition = new Position(currentPosition.X - 1, currentPosition.Y, currentPosition.Z);
            if (checkIfValid(newPosition))
            {
                AState stateToAdd = new MazeState(newPosition, state);
                m_successors.Add(stateToAdd);
            }
            newPosition.X += 2;
            if (checkIfValid(newPosition))
            {
                AState stateToAdd = new MazeState(newPosition, state);
                m_successors.Add(stateToAdd);
            }
            newPosition.X -= 1;
            newPosition.Y -= 1;
            if (checkIfValid(newPosition))
            {
                AState stateToAdd = new MazeState(newPosition, state);
                m_successors.Add(stateToAdd);
            }
            newPosition.Y += 2;
            if (checkIfValid(newPosition))
            {
                AState stateToAdd = new MazeState(newPosition, state);
                m_successors.Add(stateToAdd);
            }
            return m_successors;

        }

        private bool checkIfValid(Position posToCheck)
        {
            return posToCheck.X > 0 && posToCheck.X < m_maze.XLength && posToCheck.Y > 0 && posToCheck.Y < m_maze.YLength;
        }

        public AState getGoalState()
        {
            AState goalState = new MazeState(m_maze.GoalPoint, null);
            return goalState;
        }

        public AState getInitialState()
        {
            AState initialState = new MazeState(m_maze.StartPoint, null);
            return initialState;
        }

        public SearchableMaze3d(IMaze maze)
        {
            m_maze = maze as Maze;
        }
    }
}
