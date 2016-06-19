using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MazeRunner2016
{
    /// <summary>
    /// this class has the layer that connected with view and model 
    /// it activate function in model and view 
    /// </summary>
    public class Presenter
    {
        private View m_ui;
        private Model m_model;
        private Dictionary<string, ICommand> m_commands;
        private List<string> m_functions = new List<string>();
        //settings fields
        private int numberOfThreads;
        private string[] createMazeAlgo;
        private string[] solveMazeAlgo;

        public Presenter(IView ui, IModel model)
        {
            m_ui = ui as View;
            m_model = model as Model;
            //settings
            initSettings();
            //injection settins to the model and view
            injectionSettins();
            //initialize the events for the view and model layer
            initEvents();
            setCommands();
            initFunctions();
        }
        /// <summary>
        /// injection settings to the view and the model
        /// </summary>
        private void injectionSettins()
        {
            m_ui.injectionSettingsView(numberOfThreads, createMazeAlgo, solveMazeAlgo);
            m_model.injectionSettingsModel(numberOfThreads, createMazeAlgo, solveMazeAlgo);
        }
        /// <summary>
        /// this function init the settings from the settings file
        /// </summary>
        private void initSettings()
        {
            numberOfThreads = Properties.Settings.Default.numberOfThreads;
            createMazeAlgo = new string[Properties.Settings.Default.numberOfAlgoToCreate];
            createMazeAlgo = Properties.Settings.Default.createMazeAlgo.Split(',');
            solveMazeAlgo = new string[Properties.Settings.Default.numberOfAlgoToSolve];
            solveMazeAlgo = Properties.Settings.Default.solveMazeAlgo.Split(',');
        }
        /// <summary>
        /// set the commands we are have
        /// </summary>
        private void setCommands()
        {
            m_commands = new Dictionary<string, ICommand>();
            m_commands["generate3dMaze"] = new CommandGenerate3dMaze(m_model, m_ui);
            m_commands["displayMaze"] = new CommandDisplay(m_model, m_ui);
            m_commands["saveMaze"] = new CommandSaveMaze(m_model, m_ui);
            m_commands["Load maze"] = new CommandLoadMaze(m_model, m_ui);
            m_commands["mazeSize"] = new CommandMazeSize(m_model, m_ui);
            m_commands["fileSize"] = new CommandFileSize(m_model, m_ui);
            m_commands["solveMaze"] = new CommandSolveMaze(m_model, m_ui);
            m_commands["displaySolution"] = new CommandDisplaySolution(m_model, m_ui);
            m_commands["Exit"] = new CommandExit(m_model, m_ui);
            m_commands["getMazesNames"] = new CommandGetMazesNames(m_model, m_ui);
            m_commands["removeAll"] = new CommandRemoveAll(m_model, m_ui);
            m_commands["remove"] = new CommandRemove(m_model, m_ui);
        }
        /// <summary>
        /// add the name of thefunction to the dictionary
        /// </summary>
        private void initFunctions()
        {
            m_functions.Add("Home");
            m_functions.Add("Generate maze");
            m_functions.Add("Save maze");
            m_functions.Add("Load maze");
            m_functions.Add("Remove maze");
            m_functions.Add("Display maze");
            m_functions.Add("Play");
            m_functions.Add("Exit");
        }
        /// <summary>
        /// init the events that call from the model and view 
        /// </summary>
        private void initEvents()
        {
            m_ui.ViewChanged += delegate (Object sender, EventArgs e)
            {
                string command = string.Empty;
                string[] args;
                //check if its a button
                if (sender is ListBox)
                {
                    ListBox listMenu = sender as ListBox;
                    args = (e as MazeEventArgs).Params;
                    command = listMenu.SelectedItem.ToString();
                }
                else if (sender is Button)
                {
                    Button button = sender as Button;
                    args = (e as MazeEventArgs).Params;
                    command = button.Name.Substring(0, button.Name.Length - 3);
                }
                else
                {
                    command = (e as MazeEventArgs).Params[0];
                    args = (e as MazeEventArgs).Params;
                }
                m_commands[command].DoCommand(args);
            };

            m_model.ModelChanged += delegate (string notification, string otherInfromation)
            {
                string[] information;
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
                        information = otherInfromation.Split(',');
                        m_ui.saveSolution(m_model.getSolution(information[0]));
                        m_ui.saveTimeToSolve(m_model.getSolvedTime(information[0]));
                        m_ui.saveStatesDeveloped(m_model.getStatesDeveloped(information[0]));
                        m_ui.activateEventSolution();
                        break;
                    case "saveMaze":
                        m_ui.saveMessage(otherInfromation);
                        break;
                    case "loadMaze":
                        m_ui.loadMessage(otherInfromation);
                        break;
                    case "exit":
                        break;
                    case "getFunctions":
                        m_ui.saveFunctions(m_functions);
                        break;
                    case "isExist":
                        information = otherInfromation.Split(',');
                        m_ui.saveSolution(m_model.getSolution(information[0]));
                        m_ui.isAnotherThread(otherInfromation);
                        m_ui.isMainThread = true;
                        break;
                    case "mazeExist":
                        m_ui.showMessage("There is a maze with this name already");
                        break;
                    default:
                        break;
                }
            };
        }
    }
}
