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
        private string m_saveMessage, timeToSolve, statesDeveloped, m_loadMessage;
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

        public string getTimeToSolve()
        {
            return timeToSolve;
        }

        public void saveTimeToSolve(string time)
        {
            timeToSolve = time;
        }

        public string getStatesDeveloped()
        {
            return statesDeveloped;
        }

        public void saveStatesDeveloped(int states)
        {
            statesDeveloped = states.ToString();
        }

        public void saveMessage(string otherInfromation)
        {
            m_saveMessage = otherInfromation;
        }

        public string getSaveMessage()
        {
            return m_saveMessage;
        }

        public void loadMessage(string msg)
        {
            m_loadMessage = "Maze " + msg + " was saved successfully";
        }

        public string getLoadMessage()
        {
            return m_loadMessage;
        }
    }
}
