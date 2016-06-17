using MazeLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
        private int maxLevel, curLevel;
        private Maze3d myMaze;
        private IView view;
        private Dictionary<int, Grid> levelsGrid;
        private Grid prevGrid;
        private List<int> levelsVisited;
        private string mazeName;
        private PlayerControl player;

        public MazeControl()
        {
            InitializeComponent();
        }

        public MazeControl(Object mazeObject, IView view, string mazeName)
        {
            InitializeComponent();
            levelsGrid = new Dictionary<int, Grid>();
            levelsVisited = new List<int>();
            this.mazeName = mazeName;
            this.view = view;
            //get the maze dimensions
            myMaze = mazeObject as Maze3d;
            int x = (myMaze.Maze2DLayers[0] as Maze2d).Grid.GetLength(0);
            int y = (myMaze.Maze2DLayers[0] as Maze2d).Grid.GetLength(1);
            int z = myMaze.ZLength;
            maxLevel = z;
            curLevel = 0;
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
                        if (myMaze.StartPoint.X * 2 + 1 == i && myMaze.StartPoint.Y * 2 + 1 == j && curLevel == 0)
                        {
                            StartC start = new StartC();
                            placeInGrid(mazeGrid, i, j, start);
                        }
                        else if (myMaze.GoalPoint.X * 2 + 1 == i && myMaze.GoalPoint.Y * 2 + 1 == j && curLevel == maxLevel - 1)
                        {
                            GoalC goal = new GoalC();
                            placeInGrid(mazeGrid, i, j, goal);
                        }
                        else if (myMazeGrid.Grid[i, j] == 1)
                        {
                            WallC wall = new WallC();
                            placeInGrid(mazeGrid, i, j, wall);
                        }
                        else
                        {
                            PassC pass = new PassC();
                            placeInGrid(mazeGrid, i, j, pass);
                        }
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

        private static void placeInGrid(Grid mazeGrid, int i, int j, UIElement ui)
        {
            Grid.SetColumn(ui, j);
            Grid.SetRow(ui, i);
            mazeGrid.Children.Add(ui);
        }

        private void nextLevelBtn_Click(object sender, RoutedEventArgs e)
        {
            curLevel = Int32.Parse(levelNumber.Text);
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
            curLevel = Int32.Parse(levelNumber.Text);
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

        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            GameWindow gameWindow = new GameWindow(myMaze, mazeName, view);
            gameWindow.Show();
        }
    }
}
