using ATP2016Project.Model.Algorithms.MazeGenerators;
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
        void saveMaze(string mazeName, string filePath);
    }
}
