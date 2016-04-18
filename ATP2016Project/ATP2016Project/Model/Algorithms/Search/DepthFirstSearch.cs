using ATP2016Project.Model.Algorithms.MazeGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATP2016Project.Model.Algorithms.Search
{
    class DepthFirstSearch : ASearchingAlgorithm
    {
        private Dictionary<AState, List<AState>> statesByPosition;

        public DepthFirstSearch() : base()
        {
            statesByPosition = new Dictionary<AState, List<AState>>();
        }

        public override Solution search(ISearchable searchable)
        {
            Console.WriteLine("start from {0} and need to get to {1}", searchable.getInitialState().State, searchable.getGoalState().State);
            this.Searchable = searchable;
            //add the first state to the open list
            addInitialState();
            //run the algorithm until we dont have anything in the open list
            while (this.OpenList.Count != 0)
            {
                //get the next state from the open list
                currentState = this.OpenList.Dequeue();
                Console.WriteLine(currentState.State);
                //check if the current state is the goal state
                //if it is, we finish the algorithm otherwise we continue
                if (currentState.Equals(this.Searchable.getGoalState()))
                {
                    //we finished and now we backtrace the solution
                    backtraceSolution();
                    this.Solution.ReverseSolution();
                    return this.Solution;
                }
                //check if we visited this state before
                if (!statesByPosition.ContainsKey(currentState))
                {
                    //find all the successors states
                    statesByPosition.Add(currentState, this.Searchable.getAllPossibleStates(currentState));
                }
                //filter the close list states
                filterCloseListSuccessors();
                //check if there are neighbours exists
                while (statesByPosition[currentState].Count == 0)
                {
                    CloseList.Enqueue(currentState);
                    currentState = currentState.Previous;
                }
                //add the current state to the close list
                //CloseList.Enqueue(currentState);
                //choose random successor
                currentState = randomSuccsessor();
                //add the random state to the open list
                OpenList.Enqueue(currentState);
            }
            return null;
        }

        /// <summary>
        /// check from all the successors which one is already in the close list
        /// </summary>
        private void filterCloseListSuccessors()
        {
            List<AState> statesToRemove = new List<AState>();
            foreach (AState state in statesByPosition[currentState])
            {
                if (CloseList.Contains(state) || currentState.Previous != null && state.State == currentState.Previous.State)
                {
                    statesToRemove.Add(state);
                }
            }
            foreach (AState state in statesToRemove)
            {
                statesByPosition[currentState].Remove(state);
            }
        }

        private AState randomSuccsessor()
        {
            Random rnd = new Random();
            int length = statesByPosition[currentState].Count;
            int randomStateNum = rnd.Next(length);
            AState randomState = statesByPosition[currentState][randomStateNum];
            statesByPosition[currentState].Remove(randomState);
            return randomState;
        }

        private void backtraceSolution()
        {
            //iterate untill we get to the start point
            while (currentState.Previous != null)
            {
                markInGrid();
                //add the state to the solution
                this.Solution.addState(currentState);
                currentState = currentState.Previous;
            }
        }

        private void markInGrid()
        {
            Position position = (currentState as MazeState).Position;
            ((this.Searchable as SearchableMaze3d).MyMaze.Maze2DLayers[position.Z] as Maze).Grid[position.X, position.Y] = 2;
        }

        /// <summary>
        /// Add the initial state to be the first in the open list
        /// </summary>
        private void addInitialState()
        {
            this.OpenList.Enqueue(this.Searchable.getInitialState());
        }
    }
}
