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
    /// Interaction logic for DisplayControl.xaml
    /// </summary>
    public partial class DisplayControl : UserControl
    {
        /// <summary>
        /// the default constructor
        /// </summary>
        public DisplayControl()
        {
            InitializeComponent();
        }

        private IView view;

        /// <summary>
        /// the constructor we use
        /// </summary>
        /// <param name="view">the view layer</param>
        public DisplayControl(IView view)
        {
            InitializeComponent();
            this.view = view;
        }

        /// <summary>
        /// when we click on the display button it opens the maze we chose in a maze control
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">args</param>
        private void displayBtn_Click(object sender, RoutedEventArgs e)
        {
            string mazeToShow = mazeBox.SelectedItem.ToString(); //get the maze name
            view.activateEvent(sender, new MazeEventArgs(mazeToShow)); //activate the event in the presenter
            Maze3d myMaze = view.getMaze(); //get the maze
            MazeControl mazeDisplay = new MazeControl(myMaze, view, mazeToShow); //create a maze control
            mazePanel.Children.Clear();
            mazePanel.Children.Add(mazeDisplay);
        }

        /// <summary>
        /// load the maze names into the combo box
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">args</param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string[] commandName = new string[] { "getMazesNames" };
            view.activateEvent(sender, new MazeEventArgs(commandName)); //tell the presenter to get the names
            mazeBox.ItemsSource = view.getMazesNames();//get the names of the mazes
        }

        /// <summary>
        /// enable the display button only after we check some maze
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">args</param>
        private void mazeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            displayMazeBtn.IsEnabled = true; //enable the display button
        }
    }
}
