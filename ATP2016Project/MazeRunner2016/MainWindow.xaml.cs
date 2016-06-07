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
using MazeRunner2016.Controls;
using Microsoft.Win32;
using System.IO;

namespace MazeRunner2016
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DockPanel myDock;
        private IModel model;
        private IView view;

        public MainWindow()
        {
            InitializeComponent();
            model = new Model();
            view = new View();
            Presenter p = new Presenter(view, model);
            myDock = solutionInfoPanel;
        }

        private void generateClick(object sender, RoutedEventArgs e)
        {
            GenerateControl generateControl = new GenerateControl(view, model);
            actionPanel.Children.Clear();
            actionPanel.Children.Add(generateControl);
        }

        private void loadMazeBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string[] args = new string[2];
                string path = openFileDialog.FileName;
                args[0] = path;
                LoadDialog loadDialog = new LoadDialog("Please enter a name for the maze:", "MazeName");
                if (loadDialog.ShowDialog() == true)
                {
                    args[1] = loadDialog.Answer;
                }
                view.activateEvent(sender, new MazeEventArgs(args));
            }
        }

        private void mazeSizeBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void fileSizeBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void displayMazeBtn_Click(object sender, RoutedEventArgs e)
        {
            DisplayControl displayControl = new DisplayControl(view, model);
            actionPanel.Children.Clear();
            actionPanel.Children.Add(displayControl);
        }

        private void saveMazeBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveControl saveControl = new SaveControl(model, view);
            actionPanel.Children.Clear();
            actionPanel.Children.Add(saveControl);
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
