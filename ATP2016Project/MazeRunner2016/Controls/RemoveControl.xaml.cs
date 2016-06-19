using MazeLib;
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
    /// Interaction logic for RemoveControl.xaml
    /// </summary>
    public partial class RemoveControl : UserControl
    {
        private IView m_view;

        //the items that we show in the combobox
        public List<string> Items { get; private set; }

        /// <summary>
        /// our constructor
        /// </summary>
        /// <param name="view">view layer</param>
        public RemoveControl(IView view)
        {
            InitializeComponent();
            m_view = view;
        }

        /// <summary>
        /// loaded the mazes names to the combo box
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">args</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string[] commandName = new string[] { "getMazesNames" };
            m_view.activateEvent(sender, new MazeEventArgs(commandName));
            Items = m_view.getMazesNames().ToList<string>();
            mazesBox.ItemsSource = Items;
        }

        /// <summary>
        /// display the maze that we chose
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">args</param>
        private void displayBtn_Click(object sender, RoutedEventArgs e)
        {
            string[] parameters = new string[1];
            parameters[0] = mazesBox.SelectedItem.ToString();
            m_view.activateEvent(sender, new MazeEventArgs(parameters));
            Maze3d myMaze = m_view.getMaze();
            MazeControl mazeDisplay = new MazeControl(myMaze, m_view, parameters[0]);
            mazePanel.Children.Clear();
            mazePanel.Children.Add(mazeDisplay);
        }

        /// <summary>
        /// remove the chosen maze
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">args</param>
        private void removeBtn_Click(object sender, RoutedEventArgs e)
        {
            string[] parameters = new string[1];
            parameters[0] = mazesBox.SelectedItem.ToString();
            ExitDialog remove = new ExitDialog("Are you sure you want to remove maze " + parameters[0]);
            if (remove.ShowDialog() == true)
            {
                m_view.activateEvent(sender, new MazeEventArgs(parameters));
            }
            else
            {
                return;
            }
            Items.Remove(parameters[0]);
            mazesBox.ItemsSource = Items;
            mazesBox.Items.Refresh();
        }

        /// <summary>
        /// removes all the mazes in our system
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">args</param>
        private void removeAllBtn_Click(object sender, RoutedEventArgs e)
        {
            string[] parameters = new string[1];
            ExitDialog remove = new ExitDialog("Are you sure you want to remove all the mazes ");
            if (remove.ShowDialog() == true) //if we press ok
            {
                m_view.activateEvent(sender, new MazeEventArgs(parameters));
            }
            else
            {
                return;
            }
            m_view.activateEvent(sender, new MazeEventArgs(parameters));
            Items.Clear(); //clear all the mazes
            mazesBox.ItemsSource = Items;
            mazesBox.Items.Refresh(); //update
        }

        /// <summary>
        /// enable the buttons
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">args</param>
        private void mazesBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            removeBtn.IsEnabled = true;
            displayMazeBtn.IsEnabled = true;
        }
    }
}
