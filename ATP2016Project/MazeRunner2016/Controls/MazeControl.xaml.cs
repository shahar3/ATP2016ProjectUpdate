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
        #region class fields
        private int maxLevel, curLevel;
        private Maze3d myMaze;
        private IView view;
        private Dictionary<int, Grid> levelsGrid;
        private Grid prevGrid;
        private List<int> levelsVisited;
        private string mazeName;
        #endregion

        /// <summary>
        /// the defualt constructor
        /// </summary>
        public MazeControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// the constructor we use
        /// </summary>
        /// <param name="mazeObject">the maze to draw</param>
        /// <param name="view">the view layer</param>
        /// <param name="mazeName">the name of the maze</param>
        public MazeControl(Object mazeObject, IView view, string mazeName)
        {
            InitializeComponent();
            levelsGrid = new Dictionary<int, Grid>(); //keeps the grid of each level
            levelsVisited = new List<int>();
            this.mazeName = mazeName;
            this.view = view;
            //get the maze dimensions
            myMaze = mazeObject as Maze3d;
            int x = (myMaze.Maze2DLayers[0] as Maze2d).Grid.GetLength(0);
            int y = (myMaze.Maze2DLayers[0] as Maze2d).Grid.GetLength(1);
            int z = myMaze.ZLength;
            maxLevel = z; //set the max level
            curLevel = 0; //set the cur level
            createGrid(x, y, 0); //create the grid for the level
            initializeGrid(myMaze, levelsGrid[0], 0, true); //draws our maze on it
            prevGrid = levelsGrid[0]; //keep track of the last grid
            levelsVisited.Add(0);
        }

        /// <summary>
        /// create a grid panel to fit in our maze.
        /// </summary>
        /// <param name="x">rows</param>
        /// <param name="y">columns</param>
        /// <param name="level">current level</param>
        private void createGrid(int x, int y, int level)
        {
            //check if grid already created
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

        /// <summary>
        /// initialize the grid and locate our maze controls on it
        /// </summary>
        /// <param name="myMaze">the maze we draw</param>
        /// <param name="mazeGrid">the grid we use</param>
        /// <param name="curLevel">the current level</param>
        /// <param name="firstTime">check if it's the first time</param>
        private void initializeGrid(Maze3d myMaze, Grid mazeGrid, int curLevel, bool firstTime)
        {
            if (!levelsVisited.Contains(curLevel))
            {
                //initialize the grid
                Maze2d myMazeGrid = myMaze.Maze2DLayers[curLevel] as Maze2d; //get the grid
                for (int i = 0; i < myMazeGrid.Grid.GetLength(0); i++)
                {
                    for (int j = 0; j < myMazeGrid.Grid.GetLength(1); j++)
                    {
                        //check if its start point
                        if (myMaze.StartPoint.X * 2 + 1 == i && myMaze.StartPoint.Y * 2 + 1 == j && curLevel == 0)
                        {
                            StartC start = new StartC();
                            placeInGrid(mazeGrid, i, j, start);
                        }
                        //check if its goal point
                        else if (myMaze.GoalPoint.X * 2 + 1 == i && myMaze.GoalPoint.Y * 2 + 1 == j && curLevel == maxLevel - 1)
                        {
                            GoalC goal = new GoalC();
                            placeInGrid(mazeGrid, i, j, goal);
                        }
                        //check if its wall
                        else if (myMazeGrid.Grid[i, j] == 1)
                        {
                            WallC wall = new WallC();
                            placeInGrid(mazeGrid, i, j, wall);
                        }
                        //check if its pass
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

        /// <summary>
        /// place the maze control (start,goal,pass,wall) in the appropriate place in our grid
        /// </summary>
        /// <param name="mazeGrid">the grid</param>
        /// <param name="i">row</param>
        /// <param name="j">column</param>
        /// <param name="ui">the element</param>
        private static void placeInGrid(Grid mazeGrid, int i, int j, UIElement ui)
        {
            Grid.SetColumn(ui, j);
            Grid.SetRow(ui, i);
            mazeGrid.Children.Add(ui);
        }

        /// <summary>
        /// go to the next level and draw the grid
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">args</param>
        private void nextLevelBtn_Click(object sender, RoutedEventArgs e)
        {
            curLevel = Int32.Parse(levelNumber.Text); //gets cur level
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

        /// <summary>
        /// go to the level below our current level and creates the grid
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">args</param>
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

        /// <summary>
        /// opens our game window
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">args</param>
        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            GameWindow gameWindow = new GameWindow(myMaze, mazeName, view);
            gameWindow.Show();
        }
    }
}
