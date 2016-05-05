using System.Collections.Generic;

namespace ATP2016Project.Model.Algorithms.Search
{
    /// <summary>
    /// the general interface to help us to perform object adaption
    /// that way we can translate our specific problem to a general form of problem
    /// </summary>
    interface ISearchable
    {
        /// <summary>
        /// this function help us to get the initial state of the problem.
        /// the starting point of our game
        /// </summary>
        /// <returns>the initial state</returns>
        AState getInitialState();
        /// <summary>
        /// this function help us to get the goal state of the problem
        /// that way we can now when we reached our goal position to stop
        /// the searching algorithm
        /// </summary>
        /// <returns>the goal state</returns>
        AState getGoalState();
        /// <summary>
        /// this function find all the "neighbours" - the states that can be reached
        /// within one move from the current state
        /// </summary>
        /// <param name="state"></param>
        /// <returns>all the possible solutions we can reach from the current state</returns>
        List<AState> getAllPossibleStates(AState state);
        void initializeGrid();
    }
}
