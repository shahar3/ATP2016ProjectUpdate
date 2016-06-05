using MazeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MazeRunner2016
{
    public class View : IView
    {
        private string[] mazesNames;
        private Maze3d myMaze;

        public View()
        {
        }

        public event somethingHappened ViewChanged;

        public void activateEvent(object sender, EventArgs e)
        {
            ViewChanged(sender, e);
        }

        public void displayMaze(object maze)
        {
            myMaze = maze as Maze3d;
        }

        public void enterMazesNames(string[] names)
        {
            mazesNames = names;
        }

        public byte[] getMazeBytes()
        {
            return myMaze.toByteArray();
        }

        public string[] getMazesNames()
        {
            return mazesNames;
        }
    }
}
