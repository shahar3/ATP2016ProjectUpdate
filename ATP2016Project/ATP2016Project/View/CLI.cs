using ATP2016Project.Controller;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATP2016Project.Model.Algorithms.MazeGenerators;
using ATP2016Project.Model.Algorithms.Search;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ATP2016Project.View
{
    class CLI : IView
    {
        private Stream m_input;
        private Stream m_output;
        private Dictionary<string, ICommand> m_commands;
        private const string cursor = ">>";
        private IController m_controller;
        private Dictionary<string, ICommand> dictionary;

        public CLI(IController controller)
        {
            m_controller = controller;
            m_input = Console.OpenStandardInput();
            m_output = Console.OpenStandardOutput();
        }

        public CLI(IController controller, Dictionary<string, ICommand> commands) : this(controller)
        {
            this.m_commands = commands;
        }

        /// <summary>
        /// The main method in our CLI. 
        /// responsible to control the output and input of our program
        /// </summary>
        public void Start()
        {
            printInstructions(); //print the instructions
            string userCommand;
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine();
                printCommands(); //print all the commands and the descriptions
                Output(""); //print the cursor
                string[] commandAndParams = input().Trim().Split(' ');
                userCommand = commandAndParams[0].ToLower();
                string[] onlyParams = getParams(commandAndParams);
                if (commandExist(userCommand))
                {
                    m_commands[userCommand].DoCommand(onlyParams);
                }
                else
                {
                    Output("unrecognize command!");
                }
                if (userCommand == "exit")
                {
                    exit = true;
                }
                //Console.ReadKey();
            }
        }

        /// <summary>
        /// get the parameters of the command
        /// </summary>
        /// <param name="commandAndParams">the array of the full command</param>
        /// <returns>string array of the parameters</returns>
        private string[] getParams(string[] commandAndParams)
        {
            string[] onlyParams = new string[commandAndParams.Length - 1];
            for (int i = 1; i < commandAndParams.Length; i++)
            {
                onlyParams[i - 1] = commandAndParams[i];
            }
            return onlyParams;
        }

        /// <summary>
        /// check if there is a command like the parameter we get in the commands dictionary
        /// </summary>
        /// <param name="command">the command</param>
        /// <returns>if the command exist in the dictionary</returns>
        private bool commandExist(string command)
        {
            return m_commands.ContainsKey(command);
        }

        /// <summary>
        /// print all the commands and their descriptions
        /// </summary>
        private void printCommands()
        {
            int i = 1;
            foreach (string command in m_commands.Keys)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("({0})", i);
                Console.ResetColor();
                Console.Write(" {1} - ", i, command);
                MarkParameters(command); //print the description and mark the parameters in red
                Console.WriteLine();
                i++;
            }
        }

        /// <summary>
        /// mark the parameters in the description in red
        /// </summary>
        /// <param name="command">the description</param>
        private void MarkParameters(string command)
        {
            foreach (char commandChar in m_commands[command].GetDescription())
            {
                if (commandChar == '<') //parameter starts
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                Console.Write(commandChar);
                if (commandChar == '>') //parameter ends
                {
                    Console.ResetColor();
                }
            }
        }

        /// <summary>
        /// print the instructions to help the user
        /// </summary>
        private void printInstructions()
        {
            Console.WriteLine("Maze Command Line Interface (CLI) Started");
            Console.WriteLine("----------------------------------------------");
        }

        /// <summary>
        /// With this function we write to the stream (can be file, console or other streams)
        /// </summary>
        /// <param name="output">the string we want to write to the stream</param>
        public void Output(string output)
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            StreamWriter sw = new StreamWriter(m_output);
            sw.AutoFlush = true;
            Console.SetCursorPosition(0, Console.CursorTop);
            sw.WriteLine("");
            sw.WriteLine(output);
            sw.Write(cursor);
            Console.ResetColor();
        }

        /// <summary>
        /// an helping method to write in color to the screen. help us to display the maze
        /// </summary>
        /// <param name="output">the string we want to print</param>
        /// <param name="color">the desired color</param>
        private void Output(string output, ConsoleColor color)
        {
            if (color == ConsoleColor.Red)
            {
                Console.BackgroundColor = color;
            }
            else
            {
                Console.ForegroundColor = color;
            }
            Console.Write(output);
            Console.ResetColor();
        }

        /// <summary>
        /// gets the input from the user reading from the input stream
        /// </summary>
        /// <returns>input from the user</returns>
        public string input()
        {
            string input = "";
            StreamReader sr = new StreamReader(m_input);
            input = sr.ReadLine();
            return input;
        }
        /// <summary>
        /// our CLI must know the supported commands. this function initialize our commands dictionary
        /// </summary>
        /// <param name="commands">the commands dictionary</param>
        public void SetCommands(Dictionary<string, ICommand> commands)
        {
            m_commands = commands;
        }

        /// <summary>
        /// get a maze and prints it visualy to the stream
        /// </summary>
        /// <param name="maze">the maze we want to print</param>
        public void displayMaze(IMaze myMaze)
        {
            Maze3d maze3d = myMaze as Maze3d;
            string wall = "██";
            string space = "  ";
            for (int level = 0; level < maze3d.ZLength; level++)
            {
                Maze2d maze = maze3d.Maze2DLayers[level] as Maze2d;
                int rowLength = maze.Grid.GetLength(0);
                int colLength = maze.Grid.GetLength(1);
                Console.WriteLine("****LEVEL {0}****", level + 1);
                for (int i = 0; i < rowLength; i++)
                {
                    for (int j = 0; j < colLength; j++)
                    {
                        outputMaze(maze3d, wall, space, level, maze, i, j); //determine if we need to put wall or space
                    }
                    Output("\n", ConsoleColor.White);
                }
                Output("\n", ConsoleColor.White);
                Output("\n", ConsoleColor.White);
            }
        }

        /// <summary>
        /// determine if we need to put wall or space
        /// </summary>
        /// <param name="maze3d">the maze</param>
        /// <param name="wall">wall sign</param>
        /// <param name="space">space sign</param>
        /// <param name="level">maze level</param>
        /// <param name="maze">2d maze</param>
        /// <param name="i">row</param>
        /// <param name="j">column</param>
        private void outputMaze(Maze3d maze3d, string wall, string space, int level, Maze2d maze, int i, int j)
        {
            if (level == 0 && i == maze3d.StartPoint.X * 2 + 1 && j == maze3d.StartPoint.Y * 2 + 1)
            {
                Output("S ", ConsoleColor.Green);
            }
            else if (level == maze3d.ZLength - 1 && i == maze3d.GoalPoint.X * 2 + 1 && j == maze3d.GoalPoint.Y * 2 + 1)
            {
                Output("E ", ConsoleColor.DarkRed);
            }
            else if (maze.Grid[i, j] == 1) //put wall
            {
                Output(wall, ConsoleColor.White);
            }
            else if (maze.Grid[i, j] == 0) //there is a space
            {
                Output(space, ConsoleColor.White);
            }
            else //2 put solution block
            {
                Output(space, ConsoleColor.Red);
            }
        }

        /// <summary>
        /// writes the solution that we get as parameter to the output stream
        /// </summary>
        /// <param name="sol">the solution of the maze</param>
        public void displaySolution(Solution sol)
        {
            string displaySolution = string.Empty;
            foreach (AState state in sol.getSolutionPath())
            {
                displaySolution += (state as MazeState).Position.ToString() + "\n";
            }
            Output(displaySolution);
        }
    }
}
