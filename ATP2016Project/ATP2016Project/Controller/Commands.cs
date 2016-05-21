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

namespace ATP2016Project.Controller
{
    class CommandDir : ACommand
    {
        public CommandDir(IModel model, IView view) : base(model, view)
        {

        }

        public override void DoCommand(params string[] parameters)
        {
            Console.WriteLine("Showing the dir {0}", parameters[0]);
            string path = parameters[0];
            try
            {
                if (Directory.Exists(path))
                {
                    foreach (string dir in Directory.GetDirectories(path))
                    {
                        m_view.Output(dir);
                    }
                    foreach (string file in Directory.GetFiles(path))
                    {
                        m_view.Output(file);
                    }
                }
                else
                {
                    m_view.Output("Directory " + path + " doesn't exist!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public override string GetDescription()
        {
            return "Dir <path> - show all the file and folders inside the path";
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
                    Thread t = new Thread(() =>
                    {
                        m_model.generateMaze(Int32.Parse(parameters[1]), Int32.Parse(parameters[2]), Int32.Parse(parameters[3]), parameters[0]);
                        m_view.Output("Maze " + parameters[0] + " is ready");
                    });
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
            }
            return true;
        }
        public override string GetDescription()
        {
            return "generate 3d maze <maze name> <dimensions(x y z)> \n generate new maze with the name and the dimensions that you enter. \n if exist a maze with a same name the new maze will override it";
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
            string mazeName = parameters[0];
            if (m_model.getMaze(mazeName) != null)
            {
                m_view.displayMaze(m_model.getMaze(mazeName));
            }
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
            string mazeName = parameters[0];
            string path = parameters[1];
            if (!Directory.Exists(path))
            {
                m_view.Output("File path " + path + " doesn't exist!");
                return;
            }
            if (m_model.getMaze(mazeName) != null)
            {
                m_model.saveMaze(mazeName, path);
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
            string path = parameters[0];
            Console.WriteLine(path);
            string name = parameters[1];
            if (!File.Exists(path))
            {
                m_view.Output("File path " + path + " doesn't exist!");
                return;
            }
            Console.WriteLine("ok");
            m_model.loadMaze(path, name);

        }

        public override string GetDescription()
        {
            return "load maze <file path> <maze name> - load the maze from the path";
        }

        public override string GetName()
        {
            return "loadMaze";
        }
    }

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

    class CommandFileSize : ACommand
    {
        public CommandFileSize(IModel model, IView view) : base(model, view)
        {
        }

        public override void DoCommand(params string[] parameters)
        {
            string filePath = parameters[0];
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
                m_view.Output("Solution for " + mazeName + " is ready");
            });
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

    class CommandDisplaySolution : ACommand
    {
        public CommandDisplaySolution(IModel model, IView view) : base(model, view)
        {
        }

        public override void DoCommand(params string[] parameters)
        {
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

    class CommandExit : ACommand
    {
        public CommandExit(IModel model, IView view) : base(model, view)
        {
        }

        public override void DoCommand(params string[] parameters)
        {
            m_view.Output("Exiting the program...\nThank you for using MazeRunner v1.0");
            foreach (Thread t in m_threads)
            {
                t.Abort();
            }
            Thread.Sleep(3000);
            Environment.Exit(0);
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
