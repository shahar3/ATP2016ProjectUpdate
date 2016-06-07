using MazeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner2016
{
    public delegate void finishedComputing(string name, string otherInformation);

    public interface IModel
    {
        event finishedComputing ModelChanged;

        void generateMaze(int x, int y, int z, string v);
        void activateEvent(string commandName, string otherInformation);
        void prepareMazesNames();
        string[] getMazesNames();
        object getMaze(string nameOfTheMaze);
        void solveMaze(string mazeName, string algoName);
        Solution getSolution(string mazeName);
        void saveMaze(string mazeName, string mazePath);
        int getStatesDeveloped(string mazeName);
    }
}
