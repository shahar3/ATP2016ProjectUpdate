using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner2016
{
    public delegate void somethingHappened(Object sender, EventArgs e);
    public interface IView
    {
        event somethingHappened ViewChanged;
        void activateEvent(Object sender, EventArgs e);
        string[] getMazesNames();
        void enterMazesNames(string[] names);
        void displayMaze(Object maze);
        byte[] getMazeBytes();
    }
}
