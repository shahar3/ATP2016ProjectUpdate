using System.Collections.Generic;

namespace ATP2016Project.Model.Algorithms.Search
{
    interface ISearchable
    {
        AState getInitialState();
        AState getGoalState();
        List<AState> getAllPossibleStates(AState state);
    }
}
