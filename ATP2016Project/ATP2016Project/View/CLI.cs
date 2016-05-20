using ATP2016Project.Controller;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    Console.WriteLine("unrecognize command!");
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
            string[] onlyParams = new string[commandAndParams.Length];
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

                Console.WriteLine("{0}- {1} - {2} ", i, command, m_commands[command].GetDescription());
                i++;
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
            sw.WriteLine(output);
            sw.Write(cursor);
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
    }
}
