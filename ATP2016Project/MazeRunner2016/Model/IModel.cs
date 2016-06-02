using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner2016
{
    delegate void finishedComputing(string name);

    interface IModel
    {
        event finishedComputing ModelChanged;
    }
}
