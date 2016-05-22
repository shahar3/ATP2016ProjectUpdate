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
    class CommandDir : ACommand
    {
        public CommandDir(IModel model, IView view) : base(model, view)
        {
        }

        public override void DoCommand(params string[] parameters)
        {
            string fullPath = string.Empty;
            foreach (string sPath in parameters)
            {
                fullPath += " " + sPath;
            }
            try
            {
                string dir = m_model.getDir(fullPath);
                m_view.Output(dir);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public override string GetDescription()
        {
            return "Dir <path> - show all the file and folders inside the path \nWrite in the format \"C:\\directory \"";
        }

        public override string GetName()
        {
            return "dir";
        }
    }

    class CommandGenerate3dMaze : ACommand
    {
        public CommandGenerate3dMaze(IModel model, IView view) : base(model, view)
        {
        }

        public override void DoCommand(params string[] parameters)
        {
            try
            {
                if (checkIfDimensionsAreValid(parameters))
                {
                    int x = Int32.Parse(parameters[1]);
                    int y = Int32.Parse(parameters[2]);
                    int z = Int32.Parse(parameters[3]);
                    Thread t = new Thread(() =>
                    {
                        m_model.generateMaze(x, y, z, parameters[0]);
                        Thread.Sleep(40);
                        m_view.Output("Maze " + parameters[0] + " is ready");
                    });
                    t.Name = "GenerateThread";
                    t.Start();
                    m_threads.Add(t);
                }
            }
            catch (Exception e)
            {
                m_view.Output(e.Message);
            }
        }

        private bool checkIfDimensionsAreValid(string[] parameters)
        {
            //length
            if (parameters.Length != 4)
            {
                m_view.Output("Expected 4 parameters");
                return false;
            }
            //there is numbers?
            //positive numbers
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
                if (res > 40)
                {
                    m_view.Output("The numbers must be lower than 40");
                    return false;
                }
            }
            return true;
        }
        public override string GetDescription()
        {
            return "generate 3d maze <maze name> <dimensions(x y z)> \n   generate new maze with the name and the dimensions that you enter. \n   if exist a maze with a same name the new maze will override it\n   Maximum size is 40. Name the maze without spaces";
        }

        public override string GetName()
        {
            return "generate3dMaze";
        }
    }

    class CommandDisplay : ACommand
    {
        public CommandDisplay(IModel model, IView view) : base(model, view)
        {
        }

        public override void DoCommand(params string[] parameters)
        {
            string mazeName = parameters[0].ToLower();
            if (m_model.getMaze(mazeName) == null)
            {
                m_view.Output("Maze " + mazeName + " doesn't exist");
                return;
            }
            m_view.displayMaze(m_model.getMaze(mazeName));
        }

        public override string GetDescription()
        {
            return "display <maze name> - will print the maze";
        }

        public override string GetName()
        {
            return "display";
        }
    }

    class CommandSaveMaze : ACommand
    {
        public CommandSaveMaze(IModel model, IView view) : base(model, view)
        {
        }

        public override void DoCommand(params string[] parameters)
        {
            if (parameters.Length < 2)
            {
                m_view.Output("Must have 2 parameters");
                return;
            }
            string mazeName = parameters[0];
            string path = string.Empty;
            for (int i = 1; i < parameters.Length; i++)
            {
                path += " " + parameters[i];
            }
            if (!Directory.Exists(path))
            {
                m_view.Output("File path " + path + " doesn't exist!");
                return;
            }
            if (m_model.getMaze(mazeName) != null)
            {
                m_view.Output(m_model.saveMaze(mazeName, path));
            }
            else
            {
                m_view.Output("Maze " + mazeName + " doesn't exist!");
            }
        }

        public override string GetDescription()
        {
            return "save maze <maze name> <file path> - save the maze with a given name to a given file path(full path)";
        }

        public override string GetName()
        {
            return "saveMaze";
        }
    }

    class CommandLoadMaze : ACommand
    {
        public CommandLoadMaze(IModel model, IView view) : base(model, view)
        {
        }

        public override void DoCommand(params string[] parameters)
        {
            string path = string.Empty;
            for (int i = 0; i < parameters.Length - 1; i++)
            {
                path += " " + parameters[i];
            }
            string name = parameters[parameters.Length - 1];
            if (!File.Exists(path))
            {
                m_view.Output("File path " + path + " doesn't exist!");
                return;
            }
            m_view.Output(m_model.loadMaze(path, name));

        }

        public override string GetDescription()
        {
            return "load maze <file path> <maze name> - load the maze from the whole path and save it in the name you choose";
        }

        public override string GetName()
        {
            return "loadMaze";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    class CommandMazeSize : ACommand
    {
        public CommandMazeSize(IModel model, IView view) : base(model, view)
        {
        }

        public override void DoCommand(params string[] parameters)
        {
            string mazeName = parameters[0];
            if (m_model.getMaze(mazeName) == null)
            {
                m_view.Output("Maze " + mazeName + " doesn't exist");
                return;
            }
            m_view.Output("The size of the maze is " + m_model.getMazeSize(m_model.getMaze(mazeName)).ToString() + " Bytes");
        }

        public override string GetDescription()
        {
            return "maze size <maze name> - display the size of the maze in bytes";
        }

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

        public override string GetDescription()
        {
            return "file size <file path> - display the size of the file in bytes";
        }

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

        public override void DoCommand(params string[] parameters)
        {
            if (parameters.Length < 2)
            {
                m_view.Output("There is not enough arguments. Expected mazeName and algorithm");
                return;
            }
            string mazeName = parameters[0];
            string algorithm = parameters[1];
            if (m_model.getMaze(mazeName) == null)
            {
                m_view.Output("There is no maze with the name " + mazeName);
                return;
            }
            if (!m_model.algorithmExist(algorithm))
            {
                m_view.Output("The algorithm " + algorithm + " doesn't exist");
            }
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

        public override string GetDescription()
        {
            return "solve maze <maze name> <algorithm(BFS or DFS)> - solve the maze with specific algorithm";
        }

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

        public override void DoCommand(params string[] parameters)
        {
            if (parameters.Length < 2)
            {
                m_view.Output("You need 2 parameters");
                return;
            }
            string mazeName = parameters[0];
            if (!m_model.solutionExist(mazeName))
            {
                m_view.Output("Solution for the maze " + mazeName + " doesn't exist");
                return;
            }
            m_view.displaySolution(m_model.getSolution(mazeName));
        }

        public override string GetDescription()
        {
            return "display solution <maze name> -display the steps of the solution in format (x,y,z)";

        }

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

        public override string GetDescription()
        {
            return "exit - exit the program";
        }

        public override string GetName()
        {
            return "exit";
        }
    }
}
