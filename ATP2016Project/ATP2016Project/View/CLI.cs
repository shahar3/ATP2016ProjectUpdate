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

        public void Start()
        {
            printInstructions();
            string userCommand;
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine();
                printCommands();
                Output("");
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

        private string[] getParams(string[] commandAndParams)
        {
            string[] onlyParams = new string[commandAndParams.Length - 1];
            for (int i = 1; i < commandAndParams.Length; i++)
            {
                onlyParams[i - 1] = commandAndParams[i];
            }
            return onlyParams;
        }

        private bool commandExist(string command)
        {
            return m_commands.ContainsKey(command);
        }

        private void printCommands()
        {
            int i = 1;
            foreach (string command in m_commands.Keys)
            {
                Console.Write("{0}- {1} - ", i, command);
                MarkParameters(command);
                Console.WriteLine();
                i++;
            }
        }

        private void MarkParameters(string command)
        {
            foreach (char commandChar in m_commands[command].GetDescription())
            {
                if (commandChar == '<')
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                Console.Write(commandChar);
                if (commandChar == '>')
                {
                    Console.ResetColor();
                }
            }
        }

        private void printInstructions()
        {
            Console.WriteLine("Maze Command Line Interface (CLI) Started");
            Console.WriteLine("----------------------------------------------");
        }

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

        private void Output(string output, ConsoleColor color)
        {
            Console.ForegroundColor = color;
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

        public void SetCommands(Dictionary<string, ICommand> commands)
        {
            m_commands = commands;
        }

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
                        if (level == 0 && i == maze3d.StartPoint.X * 2 + 1 && j == maze3d.StartPoint.Y * 2 + 1)
                        {
                            Output("S ", ConsoleColor.Green);
                        }
                        else if (level == maze3d.ZLength - 1 && i == maze3d.GoalPoint.X * 2 + 1 && j == maze3d.GoalPoint.Y * 2 + 1)
                        {
                            Output("E ", ConsoleColor.Red);
                        }
                        else if (maze.Grid[i, j] == 1) //put wall
                        {
                            Output(wall, ConsoleColor.White);
                        }
                        else if (maze.Grid[i, j] == 0) //there is a space
                        {
                            Output(space, ConsoleColor.White);
                        }
                        else
                        {
                            Output(space, ConsoleColor.Red);
                        }
                    }
                    Output("\n", ConsoleColor.White);
                }
                Output("\n", ConsoleColor.White);
                Output("\n", ConsoleColor.White);
            }
        }

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
