using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLibrary
{
    /// <summary>
    /// this class represent our solution to the searching problems
    /// the solution is build from a sequence of states that together compose the
    /// required solution. this solution is not the only solution. there can be
    /// many different and correct solutions
    /// we support the basic functions of add state to the solution, print the solution
    /// and check if a solution to the problem exists.
    /// </summary>
    class Solution
    {
        List<AState> m_pathOfSolution; //keep the states of the solution

        /// <summary>
        /// the default constructor 
        /// init the list of the path of the solution
        /// </summary>
        public Solution()
        {
            m_pathOfSolution = new List<AState>();
        }

        /// <summary>
        /// add the state to the solution's path
        /// </summary>
        /// <param name="state">the state that we want to add to the solution</param>
        public void addState(AState state)
        {
            m_pathOfSolution.Add(state);
        }

        /// <summary>
        /// check if there is solution
        /// if there are no states in the solution's path list, there
        /// is no valid solution to our problem.
        /// </summary>
        /// <returns></returns>
        public bool isSolutionExist()
        {
            return m_pathOfSolution.Count > 0;
        }

        /// <summary>
        /// get the list of state of our solution
        /// </summary>
        /// <returns></returns>
        public List<AState> getSolutionPath()
        {
            return m_pathOfSolution;
        }

        /// <summary>
        /// get the number of steps of the solution
        /// help us to see if the algorithm if efficient
        /// </summary>
        /// <returns></returns>
        public int getSolutionNumberOfSteps()
        {
            return m_pathOfSolution.Count;
        }

        /// <summary>
        /// this function is crucial to our users of the program
        /// because the list store the states from the last one to the first state
        /// we need to reverse it.
        /// </summary>
        public void ReverseSolution()
        {
            m_pathOfSolution.Reverse();
        }

        /// <summary>
        /// show the solution state by state
        /// </summary>
        public void printSolution()
        {
            foreach (AState state in m_pathOfSolution)
            {
                Console.WriteLine(state.State);
            }
        }
    }
}
