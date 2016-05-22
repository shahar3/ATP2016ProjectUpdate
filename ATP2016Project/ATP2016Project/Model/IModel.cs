using ATP2016Project.Model.Algorithms.MazeGenerators;
using ATP2016Project.Model.Algorithms.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATP2016Project.Model
{
    /// <summary>
    /// the facade of the model layer
    /// the controller interacts with the model through this functions
    /// </summary>
    interface IModel
    {
        /// <summary>
        /// generates a 3d maze given the dimensions
        /// and saves the maze with a given name in the mazes dictionary
        /// </summary>
        /// <param name="x">number of rows</param>
        /// <param name="y">number of columns</param>
        /// <param name="z">number of labels</param>
        /// <param name="name">the name of the maze</param>
        void generateMaze(int x, int y, int z, string name);
        /// <summary>
        /// returns the maze with a given name from the dictionary
        /// </summary>
        /// <param name="name">the name of the maze</param>
        /// <returns>maze from the dictionary</returns>
        IMaze getMaze(string name);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mazeName"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
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
