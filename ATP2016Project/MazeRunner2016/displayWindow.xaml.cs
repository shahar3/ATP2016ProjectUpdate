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
using System.Windows.Shapes;

namespace MazeRunner2016
{
    /// <summary>
    /// Interaction logic for displayWindow.xaml
    /// </summary>
    public partial class displayWindow : Window
    {
        private IModel model;
        private IView view;

        public displayWindow()
        {
            InitializeComponent();
        }

        public displayWindow(IView view, IModel model)
        {
            InitializeComponent();
            this.view = view;
            this.model = model;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string[] commandName = new string[] { "getMazesNames" };
            view.activateEvent(sender, new MazeEventArgs(commandName));
            mazeBox.ItemsSource = view.getMazesNames();
        }

        private void displayBtn_Click(object sender, RoutedEventArgs e)
        {
            string mazeToShow = mazeBox.SelectedItem.ToString();
            view.activateEvent(sender, new MazeEventArgs(mazeToShow));
            byte[] myMaze = view.getMazeBytes();
            mazeDisplay.Text = myMaze.ToString();
        }
    }
}
