using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner2016
{
    class View : IView
    {
        public event somethingHappened ViewChanged;

        public void activateEvent(object sender, EventArgs e)
        {
            ViewChanged(sender, e);
        }
    }
}
