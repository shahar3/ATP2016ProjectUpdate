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

        public List<AState> getAllPossibleStates(AState state)
        {
            Position currentPosition = (state as MazeState).Position;
            m_successors = new List<AState>();
            Position newPosition = new Position(currentPosition.X - 1, currentPosition.Y, currentPosition.Z);
            if (checkIfValid(newPosition) && !checkIfThereIsWall(newPosition))
            {
                AState stateToAdd = new MazeState(newPosition, state);
                m_successors.Add(stateToAdd);
            }
            newPosition.X += 2;
            if (checkIfValid(newPosition) && !checkIfThereIsWall(newPosition))
            {
                AState stateToAdd = new MazeState(newPosition, state);
                m_successors.Add(stateToAdd);
            }
            newPosition.X -= 1;
            newPosition.Y -= 1;
            if (checkIfValid(newPosition) && !checkIfThereIsWall(newPosition))
            {
                AState stateToAdd = new MazeState(newPosition, state);
                m_successors.Add(stateToAdd);
            }
            newPosition.Y += 2;
            if (checkIfValid(newPosition) && !checkIfThereIsWall(newPosition))
            {
                AState stateToAdd = new MazeState(newPosition, state);
                m_successors.Add(stateToAdd);
            }
            return m_successors;

        }

        private bool checkIfThereIsWall(Position newPosition)
        {
            int level = newPosition.Z;
            Console.WriteLine("Check if position {0} has wall... the grid[{1},{2}]={3}", newPosition, newPosition.X, newPosition.Y, (m_maze.Maze2DLayers[level] as Maze).Grid[newPosition.X, newPosition.Y]);
            return (m_maze.Maze2DLayers[level] as Maze).Grid[newPosition.X, newPosition.Y] == 1;
        }

        private bool checkIfValid(Position posToCheck)
        {
            return posToCheck.X > 0 && posToCheck.X < m_maze.XLength && posToCheck.Y > 0 && posToCheck.Y < m_maze.YLength;
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

    }
}
