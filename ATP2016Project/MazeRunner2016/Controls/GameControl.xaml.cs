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

namespace MazeRunner2016.Controls
{
    /// <summary>
    /// Interaction logic for GameControl.xaml
    /// </summary>
    public partial class GameControl : UserControl
    {
        int curLevel;
        private Maze3d myMaze;
        private Dictionary<int, Grid> levelsGrid;
        private List<int> levelsVisited;
        private Grid prevGrid;
        private int maxLevel;
        private double curCol;
        private double curRow;
        private double up, left;
        private PlayerControl myPlayer;
        private double height;
        private double width;
        private double speed;
        private TextBox tb;
        private TextBox cellBox;
        private TextBox dimBox;

        public GameControl()
        {
            InitializeComponent();
        }

        public GameControl(Maze3d myMaze)
        {
            InitializeComponent();
            this.myMaze = myMaze;
            levelsGrid = new Dictionary<int, Grid>();
            levelsVisited = new List<int>();
            int x = (myMaze.Maze2DLayers[0] as Maze2d).Grid.GetLength(0);
            int y = (myMaze.Maze2DLayers[0] as Maze2d).Grid.GetLength(1);
            int z = myMaze.ZLength;
            maxLevel = z;
            curLevel = 0;
            curCol = myMaze.StartPoint.Y * 2 + 1;
            curRow = myMaze.StartPoint.X * 2 + 1;
            createGrid(x, y, 0);
            initializeGrid(myMaze, levelsGrid[0], 0, true);
            UpdateLayout();
            levelsGrid[0].UpdateLayout();
            prevGrid = levelsGrid[0];
            debug();
            prevGrid.AddHandler(Grid.LoadedEvent, new RoutedEventHandler(grid_Loaded));
            prevGrid.AddHandler(Grid.SizeChangedEvent, new RoutedEventHandler(sizeChanged));
        }

        private void debug()
        {
            tb = new TextBox();
            tb.Text = "this is a test";
            tb.Focusable = false;
            Canvas.SetBottom(tb, 0);
            Canvas.SetLeft(tb, 0);
            topPanel.Children.Add(tb);
            cellBox = new TextBox();
            cellBox.Text = "Cell number";
            cellBox.Focusable = false;
            Canvas.SetBottom(cellBox, 0);
            Canvas.SetLeft(cellBox, 50);
            topPanel.Children.Add(cellBox);
            dimBox = new TextBox();
            dimBox.Text = "dimensions";
            dimBox.Focusable = false;
            Canvas.SetBottom(dimBox, 0);
            Canvas.SetLeft(dimBox, 250);
            topPanel.Children.Add(dimBox);
        }

        private void grid_Loaded(object sender, RoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            width = grid.ActualWidth;
            height = grid.ActualHeight;
            width = width / levelsGrid[0].RowDefinitions.Count;
            height = height / levelsGrid[0].RowDefinitions.Count;
            createPlayer();
        }

        private void sizeChanged(object sender, RoutedEventArgs e)
        {
            Grid grid = sender as Grid;
            width = grid.ActualWidth;
            height = grid.ActualHeight;
            width = width / levelsGrid[0].RowDefinitions.Count;
            height = height / levelsGrid[0].RowDefinitions.Count;
            createPlayer();
            movePlayer(curRow, curCol);
        }

        private void movePlayer(double row, double col)
        {
            Canvas.SetLeft(myPlayer, col * width);
            Canvas.SetTop(myPlayer, row * height);
        }

        private void createPlayer()
        {
            topPanel.Children.Remove(myPlayer);
            myPlayer = new PlayerControl();
            myPlayer.Width = width;
            myPlayer.Height = height;
            topPanel.Children.Add(myPlayer);
            movePlayer(myMaze.StartPoint.X * 2 + 1, myMaze.StartPoint.Y * 2 + 1);
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
                        else if (myMazeGrid.Grid[i, j] == 0 || myMazeGrid.Grid[i, j] == 2)
                        {
                            PassC pass = new PassC();
                            placeInGrid(mazeGrid, i, j, pass);
                        }
                        else
                        {
                            WallC wall = new WallC();
                            placeInGrid(mazeGrid, i, j, wall);
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
            UpdateLayout();
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            double scaledHeightLength = height / 4;
            double scaledWidthLength = width / 4;
            double up = Canvas.GetTop(myPlayer);
            double left = Canvas.GetLeft(myPlayer);
            double bottomPlayer = up + height - scaledHeightLength;
            double rightPlayer = left + width - scaledWidthLength;
            double leftPlayer = left + scaledWidthLength;
            double upPlayer = up + scaledHeightLength;
            speed = width / 12;
            if (Double.IsNaN(up))
            {
                up = 0;
            }
            if (Double.IsNaN(left))
            {
                left = 0;
            }
            bool isPossiable = false;
            switch (e.Key)
            {
                case Key.Down:
                    isPossiable = isValidMove(e.Key, bottomPlayer);
                    if (!isPossiable)
                    {
                        return;
                    }
                    up += speed;
                    bottomPlayer += speed;
                    break;
                case Key.Up:
                    isPossiable = isValidMove(e.Key, upPlayer);
                    if (!isPossiable)
                    {
                        return;
                    }
                    up -= speed;
                    bottomPlayer -= speed;
                    break;
                case Key.Left:
                    isPossiable = isValidMove(e.Key, leftPlayer);
                    if (!isPossiable)
                    {
                        return;
                    }
                    left -= speed;
                    break;
                case Key.Right:
                    isPossiable = isValidMove(e.Key, rightPlayer);
                    if (!isPossiable)
                    {
                        return;
                    }
                    left += speed;
                    break;
                default:
                    break;
            }
            tb.Text = "x: " + (int)left + " y: " + (int)up;
            curRow = upPlayer / height;
            curCol = leftPlayer / width;
            cellBox.Text = "Row: " + curRow + " Col:" + curCol;
            dimBox.Text = "top: " + up + " Bottom: " + bottomPlayer + " Left: " + leftPlayer + " right: " + rightPlayer;
            Canvas.SetTop(myPlayer, up);
            Canvas.SetLeft(myPlayer, left);
        }

        private bool isValidMove(Key key, double position)
        {
            int row = 0, col = 0;
            bool ans = false;
            switch (key)
            {
                case Key.Up:
                    row = (int)(position / height);
                    col = (int)curCol;
                    break;
                case Key.Down:
                    row = (int)(position / height);
                    col = (int)curCol;
                    break;
                case Key.Left:
                    col = (int)(position / width);
                    row = (int)curRow;
                    break;
                case Key.Right:
                    col = (int)(position / width);
                    row = (int)curRow;
                    break;
            }
            ans = isWall(row, col);
            return !ans;
        }

        private bool isWall(int row, int col)
        {
            if (!isInside(row, col))
            {
                return true;
            }
            Maze2d curMaze = (myMaze.Maze2DLayers[curLevel] as Maze2d);
            if (curMaze.Grid[row, col] == 1)
            {
                return true;
            }
            return false;
        }

        private bool isInside(int row, int col)
        {
            if (row < 0 || col < 0 || row > myMaze.XLength * 2 + 1 || col > myMaze.YLength * 2 + 1)
            {
                return false;
            }
            return true;
        }

        private void TheGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            theGrid.Focus();
        }
    }
}
