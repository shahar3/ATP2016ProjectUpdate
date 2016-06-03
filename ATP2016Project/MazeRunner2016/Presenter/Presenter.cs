using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MazeRunner2016
{
    public class Presenter
    {
        private View m_ui;
        private Model m_model;
        private Dictionary<string, ICommand> m_commands;

        public Presenter(IView ui, IModel model)
        {
            m_ui = ui as View;
            m_model = model as Model;
            //initialize the events for the view and model layer
            initEvents();
            setCommands();
        }

        private void setCommands()
        {
            m_commands = new Dictionary<string, ICommand>();
            m_commands["generate3dMaze"] = new CommandGenerate3dMaze(m_model, m_ui);
            m_commands["displayMaze"] = new CommandDisplay(m_model, m_ui);
            m_commands["saveMaze"] = new CommandSaveMaze(m_model, m_ui);
            m_commands["loadMaze"] = new CommandLoadMaze(m_model, m_ui);
            m_commands["mazeSize"] = new CommandMazeSize(m_model, m_ui);
            m_commands["fileSize"] = new CommandFileSize(m_model, m_ui);
            m_commands["solveMaze"] = new CommandSolveMaze(m_model, m_ui);
            m_commands["displaySolution"] = new CommandDisplaySolution(m_model, m_ui);
            m_commands["Exit"] = new CommandExit(m_model, m_ui);
        }

        private void initEvents()
        {
            m_ui.ViewChanged += delegate (Object sender, EventArgs e)
            {
                Button b = sender as Button;
                string[] args = (e as MazeEventArgs).Params;
                string command = b.Name.Substring(0, b.Name.Length - 3);
                //((e as MazeEventArgs).TextBox as TextBox).Text = command;
                m_commands[command].DoCommand(args);
            };

            m_model.ModelChanged += delegate (string notification, string otherInfromation)
            {
                switch (notification)
                {
                    case "generate3dMaze":
                        m_ui.displayInTextBox("Maze: " + otherInfromation + ", is generated");
                        break;
                    case "display":
                        //code
                        break;
                    case "exit":
                        break;
                    default:
                        break;
                }
            };
        }
    }
}
