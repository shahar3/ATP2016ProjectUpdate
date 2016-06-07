using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MazeRunner2016
{
    /// <summary>
    /// this command is responsible to get the dimensions and the maze name as
    /// parameters and call the generate function in the model layer 
    /// </summary>
    public class CommandGenerate3dMaze : ACommand
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
                //convert the strings parameters to a int
                int x, y, z;
                convertParmToInt(parameters, out x, out y, out z);
                //create a new thread that run the generate separately
                Thread t = new Thread(() =>
                {
                    m_model.generateMaze(x, y, z, parameters[0]);
                    Thread.Sleep(40);
                    //m_view.Output("Maze " + parameters[0] + " is ready");
                });
                t.Name = "GenerateThread";
                t.Start();
                m_model.activateEvent("generate3dMaze", parameters[0]);

                //add the thread to list of threads for kill them in the end of the program
                m_threads.Add(t);
            }
            catch (Exception e)
            {
                //m_view.Output(e.Message);
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
            string nameOfTheMaze = parameters[0];
            m_view.displayMaze(m_model.getMaze(nameOfTheMaze));
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
            //activate the function in model that save the maze
            string mazeName = parameters[0];
            string mazePath = parameters[1];
            m_model.saveMaze(mazeName, mazePath);
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
            m_model.loadMaze(parameters[0], parameters[1]);
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
            //                string mazeName = parameters[0].ToLower();
            //                if (m_model.getMaze(mazeName) == null)
            //                {
            //                    m_view.Output("Maze " + mazeName + " doesn't exist");
            //                    return;
            //                }
            //                m_view.Output("The size of the maze is " + m_model.getMazeSize(m_model.getMaze(mazeName)).ToString() + " Bytes");
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

            //                string filePath = string.Empty;
            //                for (int i = 0; i < parameters.Length; i++)
            //                {
            //                    filePath += " " + parameters[i];
            //                }
            //                if (!File.Exists(filePath))
            //                {
            //                    m_view.Output("File " + filePath + " doesn't exist");
            //                    return;
            //                }
            //                m_view.Output("The size of " + filePath + " is " + m_model.getFileSize(filePath) + " bytes");
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
            m_model.solveMaze(parameters[0], parameters[1]);
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
            //                if (parameters.Length < 1) //need 1 parameter
            //                {
            //                    m_view.Output("Expected to get a parameter");
            //                    return;
            //                }
            //                string mazeName = parameters[0].ToLower();
            //                if (!m_model.solutionExist(mazeName))
            //                {
            //                    m_view.Output("Solution for the maze " + mazeName + " doesn't exist");
            //                    return;
            //                }
            //                m_view.displaySolution(m_model.getSolution(mazeName));
            //                m_view.Output("Do you want to show a visual solution? (y\\n)");
            //                string ans = m_view.input();
            //                if (ans.ToLower() == "y")
            //                {
            //                    m_model.markSolution(mazeName);
            //                    m_view.displayMaze(m_model.getMaze(mazeName));
            //                    m_model.clearSolution(mazeName);
            //                }
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
            //                m_view.Output("Exiting the program...\nThank you for using MazeRunner v1.0");
            //                closeThreads(); //close all the threads
            //                Thread.Sleep(3000); //wait
            //                Environment.Exit(0); //exit the program
        }

        //            /// <summary>
        //            /// an helping method, iterates on all the threads and waiting them to finish 
        //            /// before exiting the program
        //            /// </summary>
        //            private static void closeThreads()
        //            {
        //                foreach (Thread t in m_threads)
        //                {
        //                    t.Join();
        //                }
        //            }
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

    #region helping commands
    public class CommandGetMazesNames : ACommand
    {
        public CommandGetMazesNames(IModel model, IView view) : base(model, view)
        {
        }

        public override void DoCommand(params string[] parameters)
        {
            m_model.prepareMazesNames();
        }

        public override string GetDescription()
        {
            return "prepare all the mazes names exist in our system";
        }

        public override string GetName()
        {
            return "prepareMazesNames";
        }
    }
    #endregion

}
