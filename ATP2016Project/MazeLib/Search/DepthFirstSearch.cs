using ATP2016Project.Model.Algorithms.MazeGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib
{
    public class DepthFirstSearch : ASearchingAlgorithm
    {
        /// <summary>
        /// this is our implemntion for DFS algorithm
        /// we follow the psuedo algorithm:
        /// 1-add the initial state to the open list
        /// 2-while the open list is no empty do:
        /// 2.1-if the current state is the goal state we finish
        /// 2.2- get all the sucsessors have on current state
        /// 3.2- choose random sucsessor and insert to open list 
        /// </summary>
        private Random rnd = new Random();
        private Dictionary<AState, List<AState>> statesByPosition;

        public DepthFirstSearch() : base()
        {
            statesByPosition = new Dictionary<AState, List<AState>>();
        }

        /// <summary>
        ///  the main function
        /// from here we implement our algorithm. get as parameter an object
        /// that inherit from the interface ISearchable
        /// </summary>
        /// <param name="searchable"></param>
        /// <returns></returns>
        public override Solution search(ISearchable searchable, out string timeToSolve)
        {
            statesCounter = 0;
            OpenList.Clear();
            CloseList.Clear();
            this.OpenList = new Queue<AState>();
            this.CloseList = new Queue<AState>();
            timeToSolve = "";
            DateTime startingTime = DateTime.Now;
            searchable.initializeGrid(); //init the grid
            Console.WriteLine("start from {0} and need to get to {1}", searchable.getInitialState().State, searchable.getGoalState().State);
            this.Searchable = searchable;
            //add the first state to the open list
            addInitialState();
            //run the algorithm until we dont have anything in the open list
            while (this.OpenList.Count != 0)
            {
                statesCounter++;
                //get the next state from the open list
                currentState = this.OpenList.Dequeue();
                //check if the current state is the goal state
                //if it is, we finish the algorithm otherwise we continue
                if (currentState.Equals(this.Searchable.getGoalState()))
                {
                    //we finished and now we backtrace the solution
                    backtraceSolution();
                    this.Solution.ReverseSolution();
                    DateTime endTime = DateTime.Now;
                    TimeSpan difference = endTime - startingTime;
                    string result = difference.TotalSeconds.ToString();
                    timeToSolve = result;
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
        /// <summary>
        /// this is function that find random sucsessor
        /// </summary>
        /// <returns></returns>
        private AState randomSuccsessor()
        {
            int length = statesByPosition[currentState].Count;
            int randomStateNum = rnd.Next(length);
            AState randomState = statesByPosition[currentState][randomStateNum];
            statesByPosition[currentState].Remove(randomState);
            return randomState;
        }
        /// <summary>
        /// in this function we build the solution 
        /// we start from the current state(here this the goal state) and add
        /// the state to the solution and go to the previous state until we arrive to start state
        /// after this function we reverse this solution and get the real solution
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
