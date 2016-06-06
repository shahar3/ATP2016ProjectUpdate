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
        private Solution mazeSolution;

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

        public Maze3d getMaze()
        {
            return myMaze;
        }

        public string[] getMazesNames()
        {
            return mazesNames;
        }

        public void saveSolution(Solution sol)
        {
            mazeSolution = sol;
        }

        public Solution getSolution()
        {
            return mazeSolution;
        }
    }
}
