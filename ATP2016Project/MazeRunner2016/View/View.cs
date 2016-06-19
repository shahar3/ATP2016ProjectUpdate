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
    /// <summary>
    /// this class contain functions that activate from the presenter layer
    /// </summary>
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
        public bool isMainThread;

        public View()
        {
        }
        //events
        public event somethingHappened ViewChanged;
        public event solutionDelegate SolutionRetrieved;
        /// <summary>
        /// this function activate event that say to the presenter that 
        /// the operation is done
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event args</param>
        public void activateEvent(object sender, EventArgs e)
        {
            ViewChanged(sender, e);
        }
        /// <summary>
        /// this function save the maze that we getin parameters
        /// </summary>
        /// <param name="maze">maze</param>
        public void displayMaze(object maze)
        {
            myMaze = maze as Maze3d;
        }
        /// <summary>
        /// init the settings fieldsfrom paramters that we get from the presenter
        /// </summary>
        /// <param name="numberOfThreads">num of thread</param>
        /// <param name="createMazeAlgo">string create</param>
        /// <param name="solveMazeAlgo">string solve</param>
        public void injectionSettingsView(int numberOfThreads, string[] createMazeAlgo, string[] solveMazeAlgo)
        {
            m_numberOfThreads = numberOfThreads;
            m_createMazeAlgo = createMazeAlgo;
            m_solveMazeAlgo = solveMazeAlgo;
        }
        /// <summary>
        /// save the name of mazes that we get in field
        /// </summary>
        /// <param name="names">mazes names</param>
        public void enterMazesNames(string[] names)
        {
            mazesNames = names;
        }
        /// <summary>
        /// return the maze the save in the field
        /// </summary>
        /// <returns>maze</returns>
        public Maze3d getMaze()
        {
            return myMaze;
        }
        /// <summary>
        /// return the mazes names
        /// </summary>
        /// <returns>mazes name</returns>
        public string[] getMazesNames()
        {
            return mazesNames;
        }
        /// <summary>
        /// save the solution in field
        /// </summary>
        /// <param name="sol">solution</param>
        public void saveSolution(Solution sol)
        {
            mazeSolution = sol;

        }
        /// <summary>
        /// return the solution
        /// </summary>
        /// <returns>solution</returns>
        public Solution getSolution()
        {
            return mazeSolution;
        }
        /// <summary>
        /// return the time to solve
        /// </summary>
        /// <returns>time to solve</returns>
        public string getTimeToSolve()
        {
            return timeToSolve;
        }
        /// <summary>
        /// save the time in field
        /// </summary>
        /// <param name="time">string time</param>
        public void saveTimeToSolve(string time)
        {
            timeToSolve = time;
        }
        /// <summary>
        /// return state develope
        /// </summary>
        /// <returns>states</returns>
        public string getStatesDeveloped()
        {
            return statesDeveloped;
        }
        /// <summary>
        /// save statedevelope
        /// </summary>
        /// <param name="states">state</param>
        public void saveStatesDeveloped(int states)
        {
            statesDeveloped = states.ToString();

        }
        /// <summary>
        /// save message in field
        /// </summary>
        /// <param name="otherInfromation">message</param>
        public void saveMessage(string otherInfromation)
        {
            m_saveMessage = otherInfromation;
        }
        /// <summary>
        /// return the save message
        /// </summary>
        /// <returns>save message</returns>
        public string getSaveMessage()
        {
            return m_saveMessage;
        }
        /// <summary>
        /// load message
        /// </summary>
        /// <param name="msg">message</param>
        public void loadMessage(string msg)
        {
            m_loadMessage = "Maze " + msg + " was saved successfully";
        }
        /// <summary>
        /// return loadmessage
        /// </summary>
        /// <returns>string message</returns>
        public string getLoadMessage()
        {
            return m_loadMessage;
        }
        /// <summary>
        /// save function
        /// </summary>
        /// <param name="functions">list</param>
        public void saveFunctions(List<string> functions)
        {
            m_functions = functions;
        }
        /// <summary>
        /// return m_function
        /// </summary>
        /// <returns>function</returns>
        public List<string> getFunctions()
        {
            return m_functions;
        }
        /// <summary>
        /// shoe the message that we get has parameters
        /// </summary>
        /// <param name="otherInfromation">message</param>
        public void showMessage(string otherInfromation)
        {
            MessageBox.Show(otherInfromation);
        }
        /// <summary>
        /// active events
        /// </summary>
        public void activateEventSolution()
        {
            SolutionRetrieved();
        }
        /// <summary>
        /// save if is another thread
        /// </summary>
        /// <param name="otherInfromation">trueor false</param>
        internal void isAnotherThread(string otherInfromation)
        {
            isMainThread = true;
        }
        /// <summary>
        /// return if is another thread
        /// </summary>
        /// <returns>true or false</returns>
        public bool anotherThread()
        {
            return isMainThread;
        }
    }
}
