using MazeLib;
using MazeRunner2016.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MazeRunner2016
{
    public class View : IView
    {
        //settings fields
        private int m_numberOfThreads;
        private string[] m_createMazeAlgo;
        private string[] m_solveMazeAlgo;

        private string[] mazesNames;
        private string m_saveMessage, timeToSolve, statesDeveloped, m_loadMessage;
        private Maze3d myMaze;
        private Solution mazeSolution;
        private List<string> m_functions;
        private bool isMainThread;

        public View()
        {
        }

        public event somethingHappened ViewChanged;
        public event solutionDelegate SolutionRetrieved;

        public void activateEvent(object sender, EventArgs e)
        {
            ViewChanged(sender, e);
        }

        public void displayMaze(object maze)
        {
            myMaze = maze as Maze3d;
        }

        public void injectionSettingsView(int numberOfThreads, string[] createMazeAlgo, string[] solveMazeAlgo)
        {
            m_numberOfThreads = numberOfThreads;
            m_createMazeAlgo = createMazeAlgo;
            m_solveMazeAlgo = solveMazeAlgo;
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

        public void saveFunctions(List<string> functions)
        {
            m_functions = functions;
        }

        public List<string> getFunctions()
        {
            return m_functions;
        }

        public void showMessage(string otherInfromation)
        {
            MessageBox.Show(otherInfromation);
        }

        public void activateEventSolution()
        {
            SolutionRetrieved();
        }



        internal void isAnotherThread(string otherInfromation)
        {
            isMainThread = false;
        }

        public bool anotherThread()
        {
            return isMainThread;
        }
    }
}
