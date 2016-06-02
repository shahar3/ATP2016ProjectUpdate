using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib
{
    /// <summary>
    /// the basic interface for any searching algorithm
    /// includes the main function search that solve the searching problem
    /// and 2 helping methods to determine the efficiency level of each
    /// algorithm
    public interface ISearchingAlgorithm
    {
        /// <summary>
        /// in this function we implement the search algorithm on a general searching problem
        /// </summary>
        /// <param name="searchable">can vary through many different searching problem</param>
        /// <returns>the solution for the problem</returns>
        Solution search(ISearchable searchable);
        /// <summary>
        /// help us to determine if the algorithm is effective compared to other algorithms
        /// </summary>
        /// <returns>how many states was developed during the search process</returns>
        int statesDeveloped();
        /// <summary>
        /// help us to determine how good is the algorithm time-wise.
        /// </summary>
        /// <param name="searchable"></param>
        /// <returns>the time elapsed since we start to solve the problem untill it solved</returns>
        String timeToSolve(ISearchable searchable);
    }
}