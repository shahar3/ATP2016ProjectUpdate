using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner2016
{
    delegate void somethingHappened(Object sender, EventArgs e);
    interface IView
    {
        event somethingHappened ViewChanged;
        void activateEvent(Object sender, EventArgs e);
    }
}
