using ATP2016Project.Model;
using ATP2016Project.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATP2016Project.Controller
{
    class MyController : IController
    {
        private IView m_view;
        private IModel m_model;

        public MyController()
        {
        }

        public void SetModel(IModel model)
        {
            m_model = model;
        }

        public void SetView(IView view)
        {
            m_view = view;
            m_view.SetCommands(GetCommands());
        }

        public void Output(string output)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, ICommand> GetCommands()
        {
            Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>();
            ICommand dir = new CommandDir(m_model, m_view);
            ICommand generate3dMaze = new CommandGenerate3dMaze(m_model, m_view);
            ICommand display = new CommandDisplay(m_model, m_view);
            ICommand displayCrossSectionBy = new CommandDisplayCrossSectionBy(m_model, m_view);
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
            commands.Add(displayCrossSectionBy.GetName().ToLower(), displayCrossSectionBy);
            commands.Add(saveMaze.GetName().ToLower(), saveMaze);
            commands.Add(loadMaze.GetName().ToLower(), loadMaze);
            commands.Add(mazeSize.GetName().ToLower(), mazeSize);
            commands.Add(fileSize.GetName().ToLower(), fileSize);
            commands.Add(solveMaze.GetName().ToLower(), solveMaze);
            commands.Add(displaySolution.GetName().ToLower(), solveMaze);
            commands.Add(exit.GetName().ToLower(), exit);
            return commands;
        }
    }
}
