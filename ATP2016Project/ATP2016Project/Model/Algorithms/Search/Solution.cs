using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATP2016Project.Model.Algorithms.Search
{
    class Solution
    {
        List<AState> m_pathOfSolution;

        public Solution()
        {
            m_pathOfSolution = new List<AState>();
        }

        public void addState(AState state)
        {
            m_pathOfSolution.Add(state);
        }

        public bool isSolutionExist()
        {
            return m_pathOfSolution.Count > 0;
        }

        public List<AState> getSolutionPath()
        {
            return m_pathOfSolution;
        }

        public int getSolutionNumberOfSteps()
        {
            return m_pathOfSolution.Count;
        }

        public void ReverseSolution()
        {
            m_pathOfSolution.Reverse();
        }

        public void printSolution()
        {
            foreach (AState state in m_pathOfSolution)
            {
                Console.WriteLine(state.State);
            }
        }
    }
}
