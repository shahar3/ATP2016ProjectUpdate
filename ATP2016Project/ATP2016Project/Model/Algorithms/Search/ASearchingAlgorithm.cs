using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATP2016Project.Model.Algorithms.Search
{


    abstract class ASearchingAlgorithm : ISearchingAlgorithm
    {
        //our class members
        private Queue<AState> m_openList;
        private Queue<AState> m_closeList;
        private ISearchable m_searchable;
        private List<AState> m_successors;
        private Solution m_solution;
        protected AState currentState;

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
            //
            this.CloseList = new Queue<AState>();
            this.Successors = new List<AState>();
            this.Solution = new Solution();
        }

        public Solution Solution
        {
            get { return m_solution; }
            set { m_solution = value; }
        }



        public List<AState> Successors
        {
            get { return m_successors; }
            set { m_successors = value; }
        }


        public ISearchable Searchable
        {
            get { return m_searchable; }
            set { m_searchable = value; }
        }



        public Queue<AState> CloseList
        {
            get { return m_closeList; }
            set { m_closeList = value; }
        }


        public Queue<AState> OpenList
        {
            get { return m_openList; }
            set { m_openList = value; }
        }

        public abstract Solution search(ISearchable searchable);

        public int statesDeveloped()
        {
            return CloseList.Count;
        }

        public string timeToSolve(ISearchable searchable)
        {

            DateTime startingTime = DateTime.Now;
            search(searchable);
            DateTime endTime = DateTime.Now;
            TimeSpan difference = endTime - startingTime;
            string result = difference.TotalSeconds.ToString();
            return "It took " + result + "seconds to solve";
        }

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
