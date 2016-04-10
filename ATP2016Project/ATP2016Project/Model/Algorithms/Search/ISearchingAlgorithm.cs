using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATP2016Project.Model.Algorithms.Search
{
    interface ISearchingAlgorithm
    {
        Solution search(ISearchable searchable);
    }
}