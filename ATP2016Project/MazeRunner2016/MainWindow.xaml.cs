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
using MazeLib;

namespace MazeRunner2016
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IModel model;
        private IView view;

        public MainWindow()
        {
            InitializeComponent();
            model = new Model();
            view = new View(t);
            Presenter p = new Presenter(view, model);
        }

        private void generateClick(object sender, RoutedEventArgs e)
        {
            generateWindow w = new generateWindow(view, model);
            w.Show();
        }

        private void loadMazeBtn_Click(object sender, RoutedEventArgs e)
        {
            displayWindow displayW = new displayWindow(view, model);
        }

        private void mazeSizeBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void fileSizeBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void displayMazeBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void saveMazeBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void solveMazeBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void displaySolutionBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
