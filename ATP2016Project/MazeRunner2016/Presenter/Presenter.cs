using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            m_commands["getMazesNames"] = new CommandGetMazesNames(m_model, m_ui);
        }

        private void initEvents()
        {
            m_ui.ViewChanged += delegate (Object sender, EventArgs e)
            {
                string command = string.Empty;
                string[] args;
                //check if its a button
                if (sender is Button)
                {
                    Button b = sender as Button;
                    args = (e as MazeEventArgs).Params;
                    command = b.Name.Substring(0, b.Name.Length - 3);
                }
                else
                {
                    command = (e as MazeEventArgs).Params[0];
                    args = (e as MazeEventArgs).Params;
                }
                m_commands[command].DoCommand(args);
                //((e as MazeEventArgs).TextBox as TextBox).Text = command;
            };

            m_model.ModelChanged += delegate (string notification, string otherInfromation)
            {
                switch (notification)
                {
                    case "getMazesNames":
                        string[] names = m_model.getMazesNames();
                        m_ui.enterMazesNames(names);
                        break;
                    case "generate3dMaze":
                        MessageBox.Show("Maze: " + otherInfromation + " is generated");
                        break;
                    case "display":
                        //code
                        break;
                    case "solveMaze":
                        m_ui.saveSolution(m_model.getSolution(otherInfromation));
                        m_ui.saveTimeToSolve(m_model.getSolvedTime(otherInfromation));
                        break;
                    case "saveMaze":
                        m_ui.saveMessage(otherInfromation);
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
