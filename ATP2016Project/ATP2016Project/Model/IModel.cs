using ATP2016Project.Model.Algorithms.MazeGenerators;
using ATP2016Project.Model.Algorithms.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATP2016Project.Model
{
    interface IModel
    {
        void generateMaze(int x, int y, int z, string name);
        IMaze getMaze(string name);
        string saveMaze(string mazeName, string filePath);
        string loadMaze(string path, string name);
        long getMazeSize(IMaze maze);
        long getFileSize(string filePath);
        bool algorithmExist(string algoName);
        void solveMaze(string mazeName, string algoName);
        bool solutionExist(string mazeName);
        Solution getSolution(string mazeName);
        string getDir(string path);
    }
}
