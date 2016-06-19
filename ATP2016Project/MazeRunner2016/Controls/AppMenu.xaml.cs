using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MazeRunner2016.Controls
{
    /// <summary>
    /// Interaction logic for AppMenu.xaml
    /// </summary>
    public partial class AppMenu : UserControl
    {
        private IView m_view;
        private Panel panel; //the main panel in our app
        /// <summary>
        /// the default constructor
        /// </summary>
        public AppMenu()
        {
            //init
            InitializeComponent();
        }

        /// <summary>
        /// set the view. that way we can pass it as parameter to the other controls
        /// </summary>
        /// <param name="view"></param>
        public void setView(IView view)
        {
            m_view = view;
        }

        /// <summary>
        /// Set the panel to be the main panel in our app
        /// </summary>
        /// <param name="actionPanel">the main panel</param>
        public void setPanel(Panel actionPanel)
        {
            panel = actionPanel;
        }

        /// <summary>
        /// Call the load dialog and performs the load command
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">the args</param>
        private void clickLoad(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog(); //our load dialog
            openFileDialog.Filter = "Maze files (*.maze)|*.maze"; //show only the mazes
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); //load from my documents
            if (openFileDialog.ShowDialog() == true)
            {
                string[] args = new string[2];
                string path = openFileDialog.FileName;
                args[0] = path; //maze path
                CustomDialog loadDialog = new CustomDialog("Please enter a name for the maze:", "MazeName");
                if (loadDialog.ShowDialog() == true) //if we pressed ok
                {
                    args[1] = loadDialog.Answer; //maze name
                    m_view.activateEvent(sender, new MazeEventArgs(args));
                    string msgToShow = m_view.getLoadMessage();
                    MessageBox.Show(msgToShow);
                }
            }
        }

        /// <summary>
        /// performs the save task and command
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">the args</param>
        private void clickSave(object sender, RoutedEventArgs e)
        {
            SaveControl saveControl = new SaveControl(m_view);
            panel.Children.Clear();
            panel.Children.Add(saveControl);
        }

        /// <summary>
        /// show on screen the generate maze control
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">the args</param>
        private void clickNew(object sender, RoutedEventArgs e)
        {
            GenerateControl generateControl = new GenerateControl(m_view);
            panel.Children.Clear();
            panel.Children.Add(generateControl);
        }

        /// <summary>
        /// the properties panel
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">the args</param>
        private void clickProperties(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Coming soon...");
        }

        /// <summary>
        /// the bug panel. here you can send a mail to the developers
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">the args</param>
        private void clickBug(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Coming soon...");
        }

        /// <summary>
        /// the instruction panel. on click it opens the instructions
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">the args</param>
        private void clickInstructions(object sender, RoutedEventArgs e)
        {
            Instructions instruct = new Instructions();
            panel.Children.Clear();
            panel.Children.Add(instruct);
        }

        /// <summary>
        /// open a panel with a few words about the developers
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">the args</param>
        private void clickAboutUs(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Coming soon...");
        }

        /// <summary>
        /// performs the exit command
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">the args</param>
        private void clickExit(object sender, RoutedEventArgs e)
        {
            ExitDialog exit = new ExitDialog("Are you sure you want to exit?");
            if (exit.ShowDialog() == true) //check if we pressed ok
            {
                m_view.activateEvent(sender, new MazeEventArgs("Exit"));
            }
        }

        /// <summary>
        /// create the remove mazes control and show it on the main panel
        /// </summary>
        /// <param name="sender">the sender</param>
        /// <param name="e">the args</param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            RemoveControl remove = new RemoveControl(m_view);
            panel.Children.Clear();
            panel.Children.Add(remove);
        }
    }
}
