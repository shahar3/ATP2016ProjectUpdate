using ATP2016Project.Model;
using ATP2016Project.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// In this class we have create the commands dictionary and insert to the
/// dictionary all the commands
/// </summary>
namespace ATP2016Project.Controller
{
    class MyController : IController
    {
        private IView m_view;
        private IModel m_model;

        public MyController()
        {
        }
        /// <summary>
        /// init the field m_odel with the model that we get
        /// </summary>
        /// <param name="model">model</param>
        public void SetModel(IModel model)
        {
            m_model = model;
        }
        /// <summary>
        /// init the field m_view with the view that we get
        /// </summary>
        /// <param name="view"></param>
        public void SetView(IView view)
        {
            m_view = view;
            m_view.SetCommands(GetCommands());
        }

        public void Output(string output)
        {
        }
        /// <summary>
        /// create the new commmands and insert them to the commands dictionary 
        /// </summary>
        /// <returns>commands dictionary</returns>
        public Dictionary<string, ICommand> GetCommands()
        {
            Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>();
            ICommand dir = new CommandDir(m_model, m_view);
            ICommand generate3dMaze = new CommandGenerate3dMaze(m_model, m_view);
            ICommand display = new CommandDisplay(m_model, m_view);
            ICommand saveMaze = new CommandSaveMaze(m_model, m_view);
            ICommand loadMaze = new CommandLoadMaze(m_model, m_view);
            ICommand mazeSize = new CommandMazeSize(m_model, m_view);
            ICommand fileSize = new CommandFileSize(m_model, m_view);
            ICommand solveMaze = new CommandSolveMaze(m_model, m_view);
            ICommand displaySolution = new CommandDisplaySolution(m_model, m_view);
            ICommand exit = new CommandExit(m_model, m_view);
            commands.Add(dir.GetName().ToLower(), dir);
            commands.Add(generate3dMaze.GetName().ToLower(), generate3dMaze);
            commands.Add(display.GetName().ToLower(), display);
            commands.Add(saveMaze.GetName().ToLower(), saveMaze);
            commands.Add(loadMaze.GetName().ToLower(), loadMaze);
            commands.Add(mazeSize.GetName().ToLower(), mazeSize);
            commands.Add(fileSize.GetName().ToLower(), fileSize);
            commands.Add(solveMaze.GetName().ToLower(), solveMaze);
            commands.Add(displaySolution.GetName().ToLower(), displaySolution);
            commands.Add(exit.GetName().ToLower(), exit);
            return commands;
        }
    }
}
