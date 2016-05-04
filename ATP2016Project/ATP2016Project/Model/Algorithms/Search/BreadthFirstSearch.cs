using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATP2016Project.Model.Algorithms.MazeGenerators;

namespace ATP2016Project.Model.Algorithms.Search
{
    class BreadthFirstSearch : ASearchingAlgorithm
    {
        /// <summary>
        /// this is our implementation for generic BFS
        /// we follow the psuedo algorithm:
        /// 1 - add the initial state to the open list
        /// 2 - while open list isn't empty do
        /// 2.1 - get the next state from the open list
        /// 2.2 - if the state is equal to the goal state we backtrace the solution and finish the algorithm
        /// 2.3 - else - find all of his neighbours and add the relevants to the open list
        /// 2.4 - add the current state to the close list
        /// 3 - return false if the program reach this point
        /// </summary>
        public BreadthFirstSearch() : base()
        {
        }

        /// <summary>
        /// the main function
        /// from here we implement our algorithm. get as parameter an object
        /// that inherit from the interface ISearchable
        /// </summary>
        /// <param name="searchable"></param>
        /// <returns>the solution to the searchable problem</returns>
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
                //if it is, we finish the algorithm otherwise we continue
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
                        //successor.Previous = currentState;
                        OpenList.Enqueue(successor);
                    }
                }
            }
            //if there is no solution
            return null;
        }

        /// <summary>
        /// backtracing the solution
        /// starting from the last state (the goal state) and iterating backwards
        /// </summary>
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

        /// <summary>
        /// Add the initial state to be the first in the open list
        /// </summary>
        private void addInitialState()
        {
            this.OpenList.Enqueue(this.Searchable.getInitialState());
        }
    }
}
