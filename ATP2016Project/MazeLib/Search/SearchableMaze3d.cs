using ATP2016Project.Model.Algorithms.MazeGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib
{
    /// <summary>
    /// this class is the object adapter for our maze problem
    /// the class inherit the ISearchable interface and implement the
    /// functions from there. that way we can use our searching algorithms 
    /// to solve our specific maze problem
    /// </summary>
    public class SearchableMaze3d : ISearchable
    {
        private Maze m_maze; //we keep our maze in this variable
        private List<AState> m_successors; //list of all the neighbours of the state

        /// <summary>
        /// the default constructor
        /// cast the maze to the abstract class Maze and store it in our member variable
        /// </summary>
        /// <param name="maze"></param>
        public SearchableMaze3d(IMaze maze)
        {
            m_maze = maze as Maze;
        }

        /// <summary>
        /// the maze property
        /// this way we can access our maze from different classes
        /// </summary>
        public Maze MyMaze
        {
            get { return m_maze; }
            set { m_maze = value; }
        }

        /// <summary>
        /// this function finds all the neighbours of a given state
        /// and store the valid ones in a list
        /// </summary>
        /// <param name="state"></param>
        /// <returns>list of valid states</returns>
        public List<AState> getAllPossibleStates(AState state)
        {
            //get the current position from the MazeState
            Position currentPosition = (state as MazeState).Position;
            m_successors = new List<AState>(); //init the successors list 
            //up position
            Position upPosition = new Position(currentPosition.X - 2, currentPosition.Y, currentPosition.Z);
            if (checkIfValid(upPosition) && !checkIfThereIsWall(upPosition, currentPosition))
            {
                AState stateToAdd = new MazeState(upPosition, state);
                m_successors.Add(stateToAdd);
            }
            //down position
            Position downPosition = new Position(currentPosition.X + 2, currentPosition.Y, currentPosition.Z);
            if (checkIfValid(downPosition) && !checkIfThereIsWall(downPosition, currentPosition))
            {
                AState stateToAdd = new MazeState(downPosition, state);
                m_successors.Add(stateToAdd);
            }
            //left position
            Position leftPosition = new Position(currentPosition.X, currentPosition.Y - 2, currentPosition.Z);
            if (checkIfValid(leftPosition) && !checkIfThereIsWall(leftPosition, currentPosition))
            {
                AState stateToAdd = new MazeState(leftPosition, state);
                m_successors.Add(stateToAdd);
            }
            //right position
            Position rightPosition = new Position(currentPosition.X, currentPosition.Y + 2, currentPosition.Z);
            if (checkIfValid(rightPosition) && !checkIfThereIsWall(rightPosition, currentPosition))
            {
                AState stateToAdd = new MazeState(rightPosition, state);
                m_successors.Add(stateToAdd);
            }
            //above position (z+1)
            Position abovePosition = new Position(currentPosition.X, currentPosition.Y, currentPosition.Z + 1);
            if (checkIfValid(abovePosition) && !checkIfThereIsWall(abovePosition))
            {
                AState stateToAdd = new MazeState(abovePosition, state);
                m_successors.Add(stateToAdd);
            }
            return m_successors;

        }

        /// <summary>
        /// check if the wall between 2 adjacent cells is broken
        /// </summary>
        /// <param name="newPos"></param>
        /// <param name="curPos"></param>
        /// <returns></returns>
        private bool checkIfThereIsWall(Position newPos, Position curPos)
        {
            if (newPos.X == curPos.X) //they are horizional
            {
                if (newPos.Y < curPos.Y)
                {
                    Position wallPosToCheck = new Position(newPos.X, newPos.Y + 1, newPos.Z);
                    return checkIfThereIsWall(wallPosToCheck);
                }
                else
                {
                    Position wallPosToCheck = new Position(curPos.X, curPos.Y + 1, newPos.Z);
                    return checkIfThereIsWall(wallPosToCheck);
                }
            }
            else //they are vertical
            {
                if (newPos.X < curPos.X)
                {
                    Position wallPosToCheck = new Position(newPos.X + 1, newPos.Y, newPos.Z);
                    return checkIfThereIsWall(wallPosToCheck);
                }
                else
                {
                    Position wallPosToCheck = new Position(curPos.X + 1, curPos.Y, newPos.Z);
                    return checkIfThereIsWall(wallPosToCheck);
                }
            }
        }

        /// <summary>
        /// this function check it the given position is a wall or not.
        /// that way we can know if the position is valid or not
        /// </summary>
        /// <param name="newPosition"></param>
        /// <returns></returns>
        private bool checkIfThereIsWall(Position newPosition)
        {
            int level = newPosition.Z;
            return (m_maze.Maze2DLayers[level] as Maze).Grid[newPosition.X, newPosition.Y] == 1;
        }

        /// <summary>
        /// this function determine if a given position is inside the maze boundaries
        /// </summary>
        /// <param name="posToCheck"></param>
        /// <returns>if the position is inside or not</returns>
        private bool checkIfValid(Position posToCheck)
        {
            bool inside = posToCheck.Z < m_maze.ZLength;
            return posToCheck.X > 0 && posToCheck.X < m_maze.XLength * 2 + 1 && posToCheck.Y > 0 && posToCheck.Y < m_maze.YLength * 2 + 1 && inside;
        }

        /// <summary>
        /// this function returns the goal state of the maze. that
        /// way we can check when we reached our goal point
        /// </summary>
        /// <returns></returns>
        public AState getGoalState()
        {
            AState goalState = new MazeState(new Position(m_maze.GoalPoint.X * 2 + 1, m_maze.GoalPoint.Y * 2 + 1, m_maze.GoalPoint.Z), null);
            return goalState;
        }

        /// <summary>
        /// this function return the starting point of our maze
        /// </summary>
        /// <returns>the starting point of the maze</returns>
        public AState getInitialState()
        {
            AState initialState = new MazeState(new Position(m_maze.StartPoint.X * 2 + 1, m_maze.StartPoint.Y * 2 + 1), null);
            return initialState;
        }

        /// <summary>
        /// this function help the user to see the path of the solution in a more visual way
        /// </summary>
        /// <param name="currentState"></param>
        private void markInGrid(MazeState currentState)
        {
            Position position = (currentState as MazeState).Position;
            (this.MyMaze.Maze2DLayers[position.Z] as Maze).Grid[position.X, position.Y] = 2;
        }

        /// <summary>
        /// this is the function that color the trace of the solution path
        /// help the user to see the solution more clearly
        /// </summary>
        /// <param name="solution"></param>
        public void markSolutionInGrid(Solution solution)
        {
            foreach (MazeState state in solution.getSolutionPath())
            {
                if (state.Previous != null)
                {
                    bool upOneLevel = !((state.Previous as MazeState).Position.Z == state.Position.Z);//check if we went to the upper level
                    if (!upOneLevel)
                        markInGrid(state, state.Previous);
                }
                markInGrid(state);
            }
        }

        /// <summary>
        /// paint also the wall between 2 cells
        /// </summary>
        /// <param name="state"></param>
        /// <param name="previous"></param>
        private void markInGrid(MazeState curState, AState prevState)
        {
            if (curState.Position.X == (prevState as MazeState).Position.X) //horizional wall
            {
                if (curState.Position.Y < (prevState as MazeState).Position.Y)
                {
                    Position posToPaint = new Position(curState.Position.X, curState.Position.Y + 1, curState.Position.Z);
                    (this.MyMaze.Maze2DLayers[posToPaint.Z] as Maze).Grid[posToPaint.X, posToPaint.Y] = 2;
                }
                else
                {
                    Position posToPaint = new Position(curState.Position.X, curState.Position.Y - 1, curState.Position.Z);
                    (this.MyMaze.Maze2DLayers[posToPaint.Z] as Maze).Grid[posToPaint.X, posToPaint.Y] = 2;
                }
            }
            else //vertical wall
            {
                if (curState.Position.X < (prevState as MazeState).Position.X)
                {
                    Position posToPaint = new Position(curState.Position.X + 1, curState.Position.Y, curState.Position.Z);
                    (this.MyMaze.Maze2DLayers[posToPaint.Z] as Maze).Grid[posToPaint.X, posToPaint.Y] = 2;
                }
                else
                {
                    Position posToPaint = new Position(curState.Position.X - 1, curState.Position.Y, curState.Position.Z);
                    (this.MyMaze.Maze2DLayers[posToPaint.Z] as Maze).Grid[posToPaint.X, posToPaint.Y] = 2;
                }
            }
        }

        /// <summary>
        /// remove all the 2's - the solution path
        /// and restoring the default maze
        /// </summary>
        public void initializeGrid()
        {
            foreach (Maze mazeLayer in MyMaze.Maze2DLayers)
            {
                for (int i = 0; i < mazeLayer.Grid.GetLength(0); i++)
                {
                    for (int j = 0; j < mazeLayer.Grid.GetLength(1); j++)
                    {
                        if (mazeLayer.Grid[i, j] == 2)
                        {
                            mazeLayer.Grid[i, j] = 0;
                        }
                    }
                }
            }
        }
    }
}
