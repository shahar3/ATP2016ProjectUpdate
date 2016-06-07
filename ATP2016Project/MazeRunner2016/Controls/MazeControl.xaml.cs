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
    /// Interaction logic for MazeControl.xaml
    /// </summary>
    public partial class MazeControl : UserControl
    {
        private int maxLevel;
        private Maze3d myMaze;
        private IView view;
        private IModel model;
        private Dictionary<int, Grid> levelsGrid;
        private Grid prevGrid;
        private List<int> levelsVisited;
        private string mazeName;

        public MazeControl()
        {
            InitializeComponent();
        }

        public MazeControl(Object mazeObject, IView view, IModel model, string mazeName)
        {
            InitializeComponent();
            levelsGrid = new Dictionary<int, Grid>();
            levelsVisited = new List<int>();
            this.mazeName = mazeName;
            this.view = view;
            this.model = model;
            //get the maze dimensions
            myMaze = mazeObject as Maze3d;
            int x = (myMaze.Maze2DLayers[0] as Maze2d).Grid.GetLength(0);
            int y = (myMaze.Maze2DLayers[0] as Maze2d).Grid.GetLength(1);
            int z = myMaze.ZLength;
            maxLevel = z;
            //string msg = string.Format("The maze dimenstions are x:{0} y:{1} z:{2}", x, y, z);
            //MessageBox.Show(msg);
            createGrid(x, y, 0);
            initializeGrid(myMaze, levelsGrid[0], 0, true);
            prevGrid = levelsGrid[0];
            levelsVisited.Add(0);
        }

        private void createGrid(int x, int y, int level)
        {
            if (levelsGrid.ContainsKey(level))
            {
                return;
            }
            //create our maze grid
            Grid mazeGrid = new Grid();
            //create rows
            for (int i = 0; i < x; i++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(1, GridUnitType.Star);
                mazeGrid.RowDefinitions.Add(row);
            }
            //create columns
            for (int i = 0; i < y; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(1, GridUnitType.Star);
                mazeGrid.ColumnDefinitions.Add(col);
            }
            //assign the grid to our dictionary
            levelsGrid[level] = mazeGrid;
        }

        private void initializeGrid(Maze3d myMaze, Grid mazeGrid, int curLevel, bool firstTime)
        {
            if (!levelsVisited.Contains(curLevel))
            {
                //initialize the grid
                Maze2d myMazeGrid = myMaze.Maze2DLayers[curLevel] as Maze2d;
                for (int i = 0; i < myMazeGrid.Grid.GetLength(0); i++)
                {
                    for (int j = 0; j < myMazeGrid.Grid.GetLength(1); j++)
                    {
                        Label cell = new Label();
                        if (myMaze.StartPoint.X * 2 + 1 == i && myMaze.StartPoint.Y * 2 + 1 == j && curLevel == 0)
                        {
                            cell.Content = "S";
                            cell.Foreground = Brushes.LawnGreen;
                            cell.Background = Brushes.LimeGreen;
                        }
                        else if (myMaze.GoalPoint.X * 2 + 1 == i && myMaze.GoalPoint.Y * 2 + 1 == j && curLevel == maxLevel - 1)
                        {
                            cell.Content = "E";
                            cell.Background = Brushes.Red;
                        }
                        else if (myMazeGrid.Grid[i, j] == 0)
                        {
                            cell.Background = Brushes.White;
                        }
                        else if (myMazeGrid.Grid[i, j] == 2)
                        {
                            cell.Background = Brushes.Red;
                        }
                        else
                        {
                            cell.Background = Brushes.Black;
                        }
                        Grid.SetColumn(cell, j);
                        Grid.SetRow(cell, i);
                        mazeGrid.Children.Add(cell);
                    }
                }
                levelsGrid[curLevel] = mazeGrid;
            }
            if (!firstTime)
                mainPanel.Children.Remove(prevGrid);
            mainPanel.Children.Add(levelsGrid[curLevel]);
            prevGrid = levelsGrid[curLevel];
            //mark that we visit the level
            levelsVisited.Add(curLevel);
        }

        private void nextLevelBtn_Click(object sender, RoutedEventArgs e)
        {
            int curLevel = Int32.Parse(levelNumber.Text);
            //check max/min val
            if (++curLevel > maxLevel)
            {
                MessageBox.Show("You are in the maximum level");
                return;
            }
            //raise
            levelNumber.Text = (curLevel).ToString();
            //change the grid
            createGrid(myMaze.XLength * 2 + 1, myMaze.YLength * 2 + 1, curLevel - 1);
            initializeGrid(myMaze, levelsGrid[curLevel - 1], curLevel - 1, false);
        }

        private void prevLevelBtn_Click(object sender, RoutedEventArgs e)
        {
            int curLevel = Int32.Parse(levelNumber.Text);
            //check max/min val
            if (--curLevel < 1)
            {
                MessageBox.Show("You are in the minimum level");
                return;
            }
            //decrease
            levelNumber.Text = (curLevel).ToString();
            //change thr grid
            initializeGrid(myMaze, levelsGrid[curLevel - 1], curLevel - 1, false);
        }

        private void solveMazeBtn_Click(object sender, RoutedEventArgs e)
        {
            string nameAlgo = comboBoxAlgo.SelectionBoxItem as string;
            string[] parameters = new string[2];
            parameters[0] = mazeName;
            parameters[1] = nameAlgo;
            view.activateEvent(sender, new MazeEventArgs(parameters));
            ISearchable searchableMaze = new SearchableMaze3d(myMaze);
            Solution mazeSol = view.getSolution();
            (searchableMaze as SearchableMaze3d).markSolutionInGrid(mazeSol);
            //get the time  to solve
            redrawSolution(myMaze.XLength * 2 + 1, myMaze.YLength * 2 + 1);
            string timeToSolve = view.getTimeToSolve();
            string statesDeveloped = view.getStatesDeveloped();
            SolutionInfoControl solInfo = new SolutionInfoControl(mazeSol, timeToSolve, statesDeveloped);
            Window parentWindow = Application.Current.MainWindow;
            (parentWindow as MainWindow).myDock.Children.Clear();
            (parentWindow as MainWindow).myDock.Children.Add(solInfo);
            MessageBox.Show("solution done");
        }

        private void redrawSolution(int x, int y)
        {
            levelsGrid.Clear();
            levelsVisited.Clear();
            levelNumber.Text = "1";
            mainPanel.Children.Remove(prevGrid);
            createGrid(x, y, 0);
            initializeGrid(myMaze, levelsGrid[0], 0, true);
            prevGrid = levelsGrid[0];
            levelsVisited.Add(0);
        }
    }
}
