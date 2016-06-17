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
        public RemoveControl(IView view)
        {
            InitializeComponent();
            m_view = view;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            mazesBox.ItemsSource = m_view.getMazesNames();
        }

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
        }

        private void removeAllBtn_Click(object sender, RoutedEventArgs e)
        {
            string[] parameters = new string[1];
            ExitDialog remove = new ExitDialog("Are you sure you want to remove all the mazes ");
            if (remove.ShowDialog() == true)
            {
                m_view.activateEvent(sender, new MazeEventArgs(parameters));
            }
            else
            {
                return;
            }
            m_view.activateEvent(sender, new MazeEventArgs(parameters));
        }
    }
}
