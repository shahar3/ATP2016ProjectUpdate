using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib
{

    /// <summary>
    /// this is our abstract class for searching algorithms
    /// implemets the searching algorithms interface
    /// support our main function (search) alongside functions for the
    /// statistics(states developed)
    /// </summary>
    public abstract class ASearchingAlgorithm : ISearchingAlgorithm
    {
        //our class members
        protected static int statesCounter = 0;
        private Queue<AState> m_openList;
        private Queue<AState> m_closeList;
        private ISearchable m_searchable;
        private List<AState> m_successors;
        private Solution m_solution;
        protected AState currentState;

        /// <summary>
        /// the defualt constructor
        /// calls the initialize function
        /// </summary>
        public ASearchingAlgorithm()
        {
            //first we initialize the lists and the solution
            initLists();
        }

        /// <summary>
        /// init our lists that relevant to our algorithms
        /// </summary>
        private void initLists()
        {
            this.OpenList = new Queue<AState>();
            this.CloseList = new Queue<AState>();
            this.Successors = new List<AState>();
            this.Solution = new Solution();
        }

        /// <summary>
        /// our property to solution
        /// </summary>
        public Solution Solution
        {
            get { return m_solution; }
            set { m_solution = value; }
        }


        /// <summary>
        /// the successors property
        /// we chose to keep them in a list
        /// </summary>
        public List<AState> Successors
        {
            get { return m_successors; }
            set { m_successors = value; }
        }

        /// <summary>
        /// the searchable property
        /// we get an object that inherit from the interface ISearchable
        /// </summary>
        public ISearchable Searchable
        {
            get { return m_searchable; }
            set { m_searchable = value; }
        }


        /// <summary>
        /// the closeList property
        /// we keep all the states that are done in this queue
        /// </summary>
        public Queue<AState> CloseList
        {
            get { return m_closeList; }
            set { m_closeList = value; }
        }

        /// <summary>
        /// the openList property
        /// we keep all the states that we have to use in this queue
        /// </summary>
        public Queue<AState> OpenList
        {
            get { return m_openList; }
            set { m_openList = value; }
        }

        /// <summary>
        /// the main method for any searching algorithm
        /// each algorithm implement it different and hence we keep it abstract
        /// </summary>
        /// <param name="searchable"></param>
        /// <returns>the solution to the maze</returns>
        public abstract Solution search(ISearchable searchable);

        /// <summary>
        /// usefull for the statistics, 
        /// in that way we can determine which algorithm is more efficient
        /// </summary>
        /// <returns>number of states developed</returns>
        public int statesDeveloped()
        {
            return statesCounter;
        }

        /// <summary>
        /// also a good function to determine which algorithm is better
        /// </summary>
        /// <param name="searchable"></param>
        /// <returns>time elapsed since the algorithm start to solve the problem till the end</returns>
        public string timeToSolve(ISearchable searchable)
        {
            searchable.initializeGrid();
            DateTime startingTime = DateTime.Now;
            search(searchable);
            DateTime endTime = DateTime.Now;
            TimeSpan difference = endTime - startingTime;
            string result = difference.TotalSeconds.ToString();
            return "It took " + result + " seconds to solve";
        }

        /// <summary>
        /// this function is essential for giving the user the correct solution
        /// </summary>
        private void backtraceSolution()
        {
            //iterate untill we get to the start point
            while (currentState.Previous != null)
            {
                //markInGrid();
                //add the state to the solution
                this.Solution.addState(currentState);
                currentState = currentState.Previous;
            }
        }
    }
}
