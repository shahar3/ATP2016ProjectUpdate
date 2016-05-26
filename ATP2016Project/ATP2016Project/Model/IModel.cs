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
        /// save the maze with a given name in a path that we choose
        /// </summary>
        /// <param name="mazeName"></param>
        /// <param name="filePath"></param>
        /// <returns>a string to print to the stream</returns>
        string saveMaze(string mazeName, string filePath);
        /// <summary>
        /// load the maze from a given path and save it in the dictionary 
        /// with a given name
        /// </summary>
        /// <param name="path">the full path of the maze</param>
        /// <param name="name">the name of the maze</param>
        /// <returns>a string to print to the stream</returns>
        string loadMaze(string path, string name);
        /// <summary>
        /// the size of the maze in the memory in bytes
        /// </summary>
        /// <param name="maze">the name of the maze</param>
        /// <returns>the size of the maze in bytes</returns>
        long getMazeSize(IMaze maze);
        /// <summary>
        /// returns the size in bytes of the maze saved in a file
        /// given as a parameter
        /// </summary>
        /// <param name="filePath">the full path of the maze</param>
        /// <returns></returns>
        long getFileSize(string filePath);
        /// <summary>
        /// check if the given algorithm exist in our system
        /// </summary>
        /// <param name="algoName">the name of the algorithm</param>
        /// <returns>if the algorithm exist</returns>
        bool algorithmExist(string algoName);
        /// <summary>
        /// Solve the maze with a given algorithm and store the solution
        /// in the dictionary
        /// </summary>
        /// <param name="mazeName">the name of the maze</param>
        /// <param name="algoName">the name of the algorithm</param>
        void solveMaze(string mazeName, string algoName);
        /// <summary>
        /// check if there is a solution to the maze we get as a parameter
        /// </summary>
        /// <param name="mazeName">the maze name</param>
        /// <returns>if there is a solution for the maze in the dictionary return true, else false</returns>
        bool solutionExist(string mazeName);
        /// <summary>
        /// get the solution of the maze we get as a parameter
        /// </summary>
        /// <param name="mazeName">the name of the maze</param>
        /// <returns>the solution of the maze</returns>
        Solution getSolution(string mazeName);
        /// <summary>
        /// get a dir path and returns all the dirs and files inside
        /// </summary>
        /// <param name="path">the path of the dir</param>
        /// <returns>all the files and dirs in the path</returns>
        string getDir(string path);
        /// <summary>
        /// mark the solution for the maze in the grid
        /// </summary>
        /// <param name="mazeName">the grid</param>
        void markSolution(string mazeName);
        /// <summary>
        /// clear the path of the solution
        /// </summary>
        /// <param name="mazeName">the maze with the solution path</param>
        void clearSolution(string mazeName);

    }
}
