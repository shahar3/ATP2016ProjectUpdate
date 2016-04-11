using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATP2016Project.Model.Algorithms.Search
{
    class BreadthFirstSearch : ASearcingAlgorithm
    {
        private AState currentState;

        public BreadthFirstSearch() : base()
        {
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
                //check if the current state is the goal state
                //if it is we finish the algorithm otherwise we continue
                if (currentState.Equals(this.Searchable.getGoalState()))
                {
                    //we finished and now we backtrace the solution
                    backtraceSolution();
                    this.Solution.ReverseSolution();
                    return this.Solution;
                }
                //find all the successors states
                this.Successors = this.Searchable.getAllPossibleStates(currentState);
                //add the current state to the close list
                CloseList.Enqueue(currentState);
                foreach (AState successor in this.Successors)
                {
                    if (!this.OpenList.Contains(successor) && !this.CloseList.Contains(successor))
                    {
                        successor.Previous = currentState;
                        OpenList.Enqueue(successor);
                    }
                }

            }
            //if there is no solution
            return null;
        }

        private void backtraceSolution()
        {
            //iterate untill we get to the start point
            while (currentState.Previous != null)
            {
                //add the state to the solution
                this.Solution.addState(currentState);
                currentState = currentState.Previous;
            }
        }

        private void addInitialState()
        {
            this.OpenList.Enqueue(this.Searchable.getInitialState());
        }
    }
}
