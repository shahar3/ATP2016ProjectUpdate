using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATP2016Project.Model;
using ATP2016Project.View;
using System.IO;
using System.Threading;
using ATP2016Project.Model.Algorithms.MazeGenerators;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.CompilerServices;

namespace ATP2016Project.Controller
{
    /// <summary>
    /// The dir command gets a path as a parameter and prints to the stream
    /// all the directories and files inside the path
    /// </summary>
    class CommandDir : ACommand
    {
        public CommandDir(IModel model, IView view) : base(model, view)
        {
        }
        /// <summary>
        /// this function gets a path as a parameter and activate function in model
        /// that find all the directory and files and return that as a string
        /// </summary>
        /// <param name="parameters">path</param>
        public override void DoCommand(params string[] parameters)
        {
            string fullPath = string.Empty;
            //chain the full path as a string
            foreach (string sPath in parameters)
            {
                fullPath += " " + sPath;
            }
            try
            {
                //activate the function in model and get all the folder and files as a string
                string dir = m_model.getDir(fullPath);
                //activate the function in the view that output the result
                m_view.Output(dir);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        /// <summary>
        /// this function return the description of this command
        /// </summary>
        /// <returns>description of the command</returns>
        public override string GetDescription()
        {
            return "Dir <path> - show all the file and folders inside the path \nWrite in the format \"C:\\directory \"";
        }
        /// <summary>
        /// this function return the name of this command.
        /// </summary>
        /// <returns>the name of this comand</returns>
        public override string GetName()
        {
            return "dir";
        }
    }

    /// <summary>
    /// this command is responsible to get the dimensions and the maze name as
    /// parameters and call the generate function in the model layer 
    /// </summary>
    class CommandGenerate3dMaze : ACommand
    {
        public CommandGenerate3dMaze(IModel model, IView view) : base(model, view)
        {
        }
        /// <summary>
        /// this command is responsible to get the dimensions and the maze name as
        /// parameters and call the generate function in the model layer 
        /// </summary>
        /// <param name="parameters"></param>
        public override void DoCommand(params string[] parameters)
        {
            try
            {
                if (checkIfDimentionsAreValid(parameters))
                {
                    //convert the strings parameters to a int
                    int x, y, z;
                    convertParmToInt(parameters, out x, out y, out z);
                    //create a new thread that run the generate separately
                    Thread t = new Thread(() =>
                    {
                        m_model.generateMaze(x, y, z, parameters[0]);
                        Thread.Sleep(40);
                        m_view.Output("Maze " + parameters[0] + " is ready");
                    });
                    t.Name = "GenerateThread";
                    t.Start();
                    //add the thread to list of threads for kill them in the end of the program
                    m_threads.Add(t);
                }
            }
            catch (Exception e)
            {
                m_view.Output(e.Message);
            }
        }
        /// <summary>
        /// convert the parameters to Int
        /// </summary>
        /// <param name="parameters">parameters</param>
        /// <param name="x">int x</param>
        /// <param name="y">int y</param>
        /// <param name="z">int z</param>
        private static void convertParmToInt(string[] parameters, out int x, out int y, out int z)
        {
            x = Int32.Parse(parameters[1]);
            y = Int32.Parse(parameters[2]);
            z = Int32.Parse(parameters[3]);
        }

        /// <summary>
        /// an helping method to check if the dimensions of the maze are valid
        /// </summary>
        /// <param name="parameters">the dimensions</param>
        /// <returns>if the dimensions are valid or not</returns>
        private bool checkIfDimentionsAreValid(string[] parameters)
        {
            //length
            if (parameters.Length != 4)
            {
                m_view.Output("Expected 4 parameters");
                return false;
            }
            //there is numbers?
            //positive numbers?
            for (int i = 1; i < parameters.Length; i++)
            {
                string param = parameters[i];
                int res = 0;
                if (!Int32.TryParse(param, out res))
                {
                    m_view.Output("please insert only numbers");
                    return false;
                }
                if (res < 0)
                {
                    m_view.Output("You need to insert only positive numbers");
                    return false;
                }
                if (res > 40) //our maze limit
                {
                    m_view.Output("The numbers must be lower than 40");
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// this function return the description of this command
        /// </summary>
        /// <returns>description of the command</returns>
        public override string GetDescription()
        {
            return "generate 3d maze <maze name> <dimensions(x y z)> \n   generate new maze with the name and the dimensions that you enter. \n   if exist a maze with a same name the new maze will override it\n   Maximum size is 40. Name the maze without spaces";
        }
        /// <summary>
        /// this function return the name of this command.
        /// </summary>
        /// <returns>the name of this comand</returns>
        public override string GetName()
        {
            return "generate3dMaze";
        }
    }

    /// <summary>
    /// This command is responsible to call the model to recieve the maze
    /// and than call the view layer to display the maze
    /// </summary>
    class CommandDisplay : ACommand
    {
        public CommandDisplay(IModel model, IView view) : base(model, view)
        {
        }
        /// <summary>
        /// this function gets name of maze as a parameter and activate function in model layer
        /// that get this specific maze and activate function in view layer that display the maze
        /// </summary>
        /// <param name="parameters">name of a maze</param>
        public override void DoCommand(params string[] parameters)
        {
            if (parameters.Length < 1)
            {
                m_view.Output("Expected to get a parameter");
                return;
            }
            //get the maze name
            string mazeName = parameters[0].ToLower();
            //check if this maze exit and if not print to the sream message to the user
            if (m_model.getMaze(mazeName) == null)

            {
                m_view.Output("Maze " + mazeName + " doesn't exist");
                return;
            }
            //activate function the display the maze in the view layer
            //after we activate function in model layer the return the specific maze
            m_view.displayMaze(m_model.getMaze(mazeName));
        }
        /// <summary>
        /// this function return the description of this command
        /// </summary>
        /// <returns>description of the command</returns>
        public override string GetDescription()
        {
            return "display <maze name> - will print the maze";
        }
        /// <summary>
        /// this function return the name of this command.
        /// </summary>
        /// <returns>the name of this comand</returns>
        public override string GetName()
        {
            return "display";
        }
    }

    /// <summary>
    /// save the maze in the dictionary with a name we get from the user
    /// </summary>
    class CommandSaveMaze : ACommand
    {
        public CommandSaveMaze(IModel model, IView view) : base(model, view)
        {
        }
        /// <summary>
        /// this function activate function in model layer that save the maze 
        /// and print to the stream in the view layer that the maze save
        /// </summary>
        /// <param name="parameters">path</param>
        public override void DoCommand(params string[] parameters)
        {
            string mazeName, path = string.Empty;
            //check the parameters
            if (!checkParamToSave(parameters, out mazeName, out path))
            {
                return;
            }
            //check if this maze name that ew get from the user exist
            //and activate the function in model later that save the maze
            //after this we print to the stream in view layer that its save
            if (m_model.getMaze(mazeName) != null)
            {
                m_view.Output(m_model.saveMaze(mazeName, path));
            }
            else
            {
                m_view.Output("Maze " + mazeName + " doesn't exist!");
            }
        }
        /// <summary>
        /// check if the param that ew get from the user is valid
        /// </summary>
        /// <param name="parameters">parmateres</param>
        /// <param name="mazeName">maze name-empty</param>
        /// <param name="path">path-empty</param>
        private bool checkParamToSave(string[] parameters, out string mazeName, out string path)
        {
            path = string.Empty;
            mazeName = string.Empty;
            //length
            if (parameters.Length < 2)
            {
                m_view.Output("Must have 2 parameters");
                return false;
            }
            mazeName = parameters[0];
            for (int i = 1; i < parameters.Length; i++)
            {
                path += " " + parameters[i];
            }
            //check if this path is exit
            if (!Directory.Exists(path))
            {
                m_view.Output("File path " + path + " doesn't exist!");
                return false;
            }
            return true;
        }
        /// <summary>
        /// this function return the description of this command
        /// </summary>
        /// <returns>description of the command</returns>
        public override string GetDescription()
        {
            return "save maze <maze name> <file path> - save the maze with a given name to a given file path(full path)";
        }
        /// <summary>
        /// this function return the name of this command.
        /// </summary>
        /// <returns>the name of this comand</returns>
        public override string GetName()
        {
            return "saveMaze";
        }
    }

    /// <summary>
    /// load the maze with the name we get as a parameter from a path
    /// we get from the user
    /// </summary>
    class CommandLoadMaze : ACommand
    {
        public CommandLoadMaze(IModel model, IView view) : base(model, view)
        {
        }
        /// <summary>
        /// this function load maze from specific path
        /// use function in model layer that load the maze and inser it to the dictinery
        /// </summary>
        /// <param name="parameters">path and name</param>
        public override void DoCommand(params string[] parameters)
        {
            if (parameters.Length < 2)
            {
                m_view.Output("Must have 2 parameters");
                return;
            }
            string path = string.Empty;
            for (int i = 0; i < parameters.Length - 1; i++)
            {
                path += " " + parameters[i];
            }
            string name = parameters[parameters.Length - 1];
            //check if this file exist
            if (!File.Exists(path))
            {
                m_view.Output("File path " + path + " doesn't exist!");
                return;
            }
            m_view.Output(m_model.loadMaze(path, name));

        }
        /// <summary>
        /// this function return the description of this command
        /// </summary>
        /// <returns>description of the command</returns>
        public override string GetDescription()
        {
            return "load maze <file path> <maze name> - load the maze from the whole path and save it in the name you choose";
        }
        /// <summary>
        /// this function return the name of this command.
        /// </summary>
        /// <returns>the name of this comand</returns>
        public override string GetName()
        {
            return "loadMaze";
        }
    }

    /// <summary>
    /// retuns the size of the maze in our memory in bytes
    /// </summary>
    class CommandMazeSize : ACommand
    {
        public CommandMazeSize(IModel model, IView view) : base(model, view)
        {
        }
        /// <summary>
        /// this function activate function in model layer that return the maze size in memory
        /// and after this print to the stream in view layer the result
        /// </summary>
        /// <param name="parameters">name of maze</param>
        public override void DoCommand(params string[] parameters)
        {
            string mazeName = parameters[0].ToLower();
            if (m_model.getMaze(mazeName) == null)
            {
                m_view.Output("Maze " + mazeName + " doesn't exist");
                return;
            }
            m_view.Output("The size of the maze is " + m_model.getMazeSize(m_model.getMaze(mazeName)).ToString() + " Bytes");
        }
        /// <summary>
        /// this function return the description of this command
        /// </summary>
        /// <returns>description of the command</returns>
        public override string GetDescription()
        {
            return "maze size <maze name> - display the size of the maze in bytes";
        }
        /// <summary>
        /// this function return the name of this command.
        /// </summary>
        /// <returns>the name of this comand</returns>
        public override string GetName()
        {
            return "mazeSize";
        }
    }

    /// <summary>
    /// return the size in bytes of a maze we saved in a file
    /// </summary>
    class CommandFileSize : ACommand
    {
        public CommandFileSize(IModel model, IView view) : base(model, view)
        {
        }
        /// <summary>
        /// this function activate function in model layer that return the maze size from specific path
        /// and after this print to the stream in view layer the result
        /// </summary>
        /// <param name="parameters">path</param>
        public override void DoCommand(params string[] parameters)
        {

            string filePath = string.Empty;
            for (int i = 0; i < parameters.Length; i++)
            {
                filePath += " " + parameters[i];
            }
            if (!File.Exists(filePath))
            {
                m_view.Output("File " + filePath + " doesn't exist");
                return;
            }
            m_view.Output("The size of " + filePath + " is " + m_model.getFileSize(filePath) + " bytes");
        }
        /// <summary>
        /// this function return the description of this command
        /// </summary>
        /// <returns>description of the command</returns>
        public override string GetDescription()
        {
            return "file size <file path> - display the size of the file in bytes";
        }
        /// <summary>
        /// this function return the name of this command.
        /// </summary>
        /// <returns>the name of this comand</returns>
        public override string GetName()
        {
            return "fileSize";
        }
    }

    /// <summary>
    /// solve the specific maze we chose and store it in the dictionary
    /// </summary>
    class CommandSolveMaze : ACommand
    {
        public CommandSolveMaze(IModel model, IView view) : base(model, view)
        {
        }
        /// <summary>
        /// this function activate function in model layer that solve the maze with specific name and algorithm
        /// and after this print to the stream in view layer that the sollution is ready
        /// </summary>
        /// <param name="parameters">maze name,algoritm</param>
        public override void DoCommand(params string[] parameters)
        {
            string mazeName, algorithm;
            //check the parmeters
            if (!checkToParamSolve(parameters, out mazeName, out algorithm))
            {
                return;
            }
            //run the computations in a new thread
            Thread t = new Thread(() =>
            {
                m_model.solveMaze(mazeName, algorithm);
                Thread.Sleep(60);
                m_view.Output("Solution for " + mazeName + " is ready");
            });
            t.Name = "SolveThread";
            t.Start();
            m_threads.Add(t);
        }
        /// <summary>
        /// this function check the parameters that we get from the user
        /// </summary>
        /// <param name="parameters">parmeters from user</param>
        /// <param name="mazeName">maze name</param>
        /// <param name="algorithm">algo</param>
        /// <returns>valid or not</returns>
        private bool checkToParamSolve(string[] parameters, out string mazeName, out string algorithm)
        {
            mazeName = string.Empty;
            algorithm = string.Empty;
            //length
            if (parameters.Length < 2)
            {
                m_view.Output("There is not enough arguments. Expected mazeName and algorithm");
                return false;
            }
            mazeName = parameters[0].ToLower();
            algorithm = parameters[1];
            //exist
            if (m_model.getMaze(mazeName) == null)
            {
                m_view.Output("There is no maze with the name " + mazeName);
                return false;
            }
            if (!m_model.algorithmExist(algorithm))
            {
                m_view.Output("The algorithm " + algorithm + " doesn't exist");
            }
            return true;
        }
        /// <summary>
        /// this function return the description of this command
        /// </summary>
        /// <returns>description of the command</returns>
        public override string GetDescription()
        {
            return "solve maze <maze name> <algorithm(BFS or DFS)> - solve the maze with specific algorithm";
        }
        /// <summary>
        /// this function return the name of this command.
        /// </summary>
        /// <returns>the name of this comand</returns>
        public override string GetName()
        {
            return "solveMaze";
        }
    }

    /// <summary>
    /// Display the solution of the specific maze we chose. 
    /// </summary>
    class CommandDisplaySolution : ACommand
    {
        public CommandDisplaySolution(IModel model, IView view) : base(model, view)
        {
        }
        /// <summary>
        /// this function activate function in model layer that return the solution of specific maze name
        /// and after this print the string(solution) to the sream in view layer
        /// </summary>
        /// <param name="parameters">maze name</param>
        public override void DoCommand(params string[] parameters)
        {
            if (parameters.Length < 1) //need 1 parameter
            {
                m_view.Output("Expected to get a parameter");
                return;
            }
            string mazeName = parameters[0].ToLower();
            if (!m_model.solutionExist(mazeName))
            {
                m_view.Output("Solution for the maze " + mazeName + " doesn't exist");
                return;
            }
            m_view.displaySolution(m_model.getSolution(mazeName));
            m_view.Output("Do you want to show a visual solution? (y\\n)");
            string ans = m_view.input();
            if (ans.ToLower() == "y")
            {
                m_model.markSolution(mazeName);
                m_view.displayMaze(m_model.getMaze(mazeName));
                m_model.clearSolution(mazeName);
            }
        }
        /// <summary>
        /// this function return the description of this command
        /// </summary>
        /// <returns>description of the command</returns>
        public override string GetDescription()
        {
            return "display solution <maze name> -display the steps of the solution in format (x,y,z)";

        }
        /// <summary>
        /// this function return the name of this command.
        /// </summary>
        /// <returns>the name of this comand</returns>
        public override string GetName()
        {
            return "displaySolution";
        }
    }

    /// <summary>
    /// Exit the program, after closing all the active threads
    /// </summary>
    class CommandExit : ACommand
    {
        public CommandExit(IModel model, IView view) : base(model, view)
        {
        }

        public override void DoCommand(params string[] parameters)
        {
            m_view.Output("Exiting the program...\nThank you for using MazeRunner v1.0");
            closeThreads(); //close all the threads
            Thread.Sleep(3000); //wait
            Environment.Exit(0); //exit the program
        }

        /// <summary>
        /// an helping method, iterates on all the threads and waiting them to finish 
        /// before exiting the program
        /// </summary>
        private static void closeThreads()
        {
            foreach (Thread t in m_threads)
            {
                t.Join();
            }
        }
        /// <summary>
        /// this function return the description of this command
        /// </summary>
        /// <returns>description of the command</returns>
        public override string GetDescription()
        {
            return "exit - exit the program";
        }
        /// <summary>
        /// this function return the name of this command.
        /// </summary>
        /// <returns>the name of this comand</returns>
        public override string GetName()
        {
            return "exit";
        }
    }
}
