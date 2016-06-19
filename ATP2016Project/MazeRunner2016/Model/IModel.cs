using MazeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner2016
{
    //this the delegate that define in the event model change will see later
    public delegate void finishedComputing(string name, string otherInformation);

    /// <summary>
    /// this the interfacr of the model
    /// here we declare about all th functions that apper in the model
    /// </summary>
    public interface IModel
    {

        event finishedComputing ModelChanged;

        void generateMaze(int x, int y, int z, string mazeName);
        void activateEvent(string commandName, string otherInformation);
        void prepareMazesNames();
        string[] getMazesNames();
        object getMaze(string nameOfTheMaze);
        void solveMaze(string mazeName, string algoName);
        Solution getSolution(string mazeName);
        void saveMaze(string mazeName, string mazePath);
        int getStatesDeveloped(string mazeName);
        void loadMaze(string mazePath, string mazeNameToSave);
        void saveMazesToZip();
        void removeMaze(string mazeName);
        void removeAllMazes();
    }
}
