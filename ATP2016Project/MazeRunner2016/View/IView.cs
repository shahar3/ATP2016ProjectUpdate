using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeLib;

namespace MazeRunner2016
{
    public delegate void somethingHappened(Object sender, EventArgs e);
    public delegate void solutionDelegate();
    public interface IView
    {
        event somethingHappened ViewChanged;
        event solutionDelegate SolutionRetrieved;
        void activateEvent(Object sender, EventArgs e);
        string[] getMazesNames();
        void enterMazesNames(string[] names);
        void displayMaze(Object maze);
        Maze3d getMaze();
        void saveSolution(Solution sol);
        void saveStatesDeveloped(int states);
        Solution getSolution();
        string getTimeToSolve();
        void saveTimeToSolve(string time);
        string getStatesDeveloped();
        string getSaveMessage();
        void loadMessage(string msg);
        string getLoadMessage();
        void saveFunctions(List<string> functions);
        List<string> getFunctions();
        void showMessage(string otherInfromation);
        bool anotherThread();
    }
}
