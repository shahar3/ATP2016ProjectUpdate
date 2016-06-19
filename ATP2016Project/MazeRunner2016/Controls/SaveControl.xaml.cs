using MazeLib;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for SaveControl.xaml
    /// </summary>
    public partial class SaveControl : UserControl
    {
        private IView m_view;

        /// <summary>
        /// the default constructor
        /// </summary>
        public SaveControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// constructor that update our view layer
        /// </summary>
        /// <param name="view">view layer</param>
        public SaveControl(IView view)
        {
            InitializeComponent();
            m_view = view;
        }

        /// <summary>
        /// update the combo box
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">args</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string[] commandName = new string[] { "getMazesNames" };
            m_view.activateEvent(sender, new MazeEventArgs(commandName));
            comboBoxSave.ItemsSource = m_view.getMazesNames();
        }

        /// <summary>
        /// perform the save, and call the command in the presenter layer
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">args</param>
        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxSave.SelectedItem != null)
            {
                //the name maze that will save
                string mazeToSave = comboBoxSave.SelectedItem.ToString();
                //get tha path
                SaveFileDialog sfd = new SaveFileDialog();
                //our filter
                sfd.Filter = "Text file (*.txt)|*.txt|C# file (*.cs)|*.cs";
                sfd.InitialDirectory = @"c:\";
                sfd.FileName = ".txt";
                string fullPath = sfd.FileName;
                string[] parameters = new string[2];
                parameters[0] = mazeToSave;
                parameters[1] = fullPath;
                //event
                m_view.activateEvent(sender, new MazeEventArgs(parameters));
                string saveMessage = m_view.getSaveMessage();
                if (sfd.ShowDialog() == true)
                {
                    MessageBox.Show(saveMessage);
                }
            }
            else
            {
                MessageBox.Show("You must choose maze from the list");
            }
        }

        /// <summary>
        /// show a sneak preview of the maze
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">args</param>
        private void previewBtn_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxSave.SelectedItem != null) //only if we check something
            {
                string mazeToShow = comboBoxSave.SelectedItem.ToString();
                (sender as Button).Name = "displayMazeBtn";
                m_view.activateEvent(sender, new MazeEventArgs(mazeToShow));
                Maze3d myMaze = m_view.getMaze();
                Mazepreview mazePreview = new Mazepreview(myMaze, m_view, mazeToShow);
                mazeDisplayPanel.Children.Add(mazePreview);
            }
            else
            {
                MessageBox.Show("You must choose maze fromthe list");
            }
        }
    }
}
