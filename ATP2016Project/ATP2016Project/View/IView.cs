using ATP2016Project.Controller;
using ATP2016Project.Model.Algorithms.MazeGenerators;
using ATP2016Project.Model.Algorithms.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATP2016Project.View
{
    /// <summary>
    /// this is the facade of our view layer
    /// the controller interact with the view layer only through this interface
    /// </summary>
    interface IView
    {
        /// <summary>
        /// The main method in our CLI. 
        /// responsible to control the output and input of our program
        /// </summary>
        void Start();
        /// <summary>
        /// With this function we write to the stream (can be file, console or other streams)
        /// </summary>
        /// <param name="output">the string we want to write to the stream</param>
        void Output(string output);
        /// <summary>
        /// get a maze and prints it visualy to the stream
        /// </summary>
        /// <param name="maze">the maze we want to print</param>
        void displayMaze(IMaze maze);
        /// <summary>
        /// gets an input from the user and returns it as a string
        /// </summary>
        /// <returns>input from the user</returns>
        string input();
        /// <summary>
        /// our CLI must know the supported commands. this function initialize our commands dictionary
        /// </summary>
        /// <param name="commands">the commands dictionary</param>
        void SetCommands(Dictionary<string, ICommand> commands);
        /// <summary>
        /// writes the solution that we get as parameter to the output stream
        /// </summary>
        /// <param name="sol">the solution of the maze</param>
        void displaySolution(Solution sol);
    }
}
