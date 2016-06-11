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
        public DisplayControl()
        {
            InitializeComponent();
        }

        private IModel model;
        private IView view;

        public DisplayControl(IView view, IModel model)
        {
            InitializeComponent();
            this.view = view;
            this.model = model;
        }

        private void displayBtn_Click(object sender, RoutedEventArgs e)
        {
            string mazeToShow = mazeBox.SelectedItem.ToString();
            view.activateEvent(sender, new MazeEventArgs(mazeToShow));
            Maze3d myMaze = view.getMaze();
            MazeControl mazeDisplay = new MazeControl(myMaze, view, model, mazeToShow);
            mazePanel.Children.Clear();
            mazePanel.Children.Add(mazeDisplay);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string[] commandName = new string[] { "getMazesNames" };
            view.activateEvent(sender, new MazeEventArgs(commandName));
            mazeBox.ItemsSource = view.getMazesNames();
        }

        private void mazeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            displayMazeBtn.IsEnabled = true;
        }
    }
}
