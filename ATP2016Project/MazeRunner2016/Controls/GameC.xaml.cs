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
using System.Diagnostics;
using System.Threading;

namespace MazeRunner2016.Controls
{
    /// <summary>
    /// Interaction logic for GameC.xaml
    /// </summary>
    public partial class GameC : UserControl
    {
        private Maze3d myMaze;
        private int curLevel;
        private double cellHeight;
        private double cellWidth;
        private int prevCol, prevRow;
        private int xLength;
        private int yLength;
        private int maxLevel;
        private double speedX, speedY;
        private double curCol, curRow;
        private PlayerControl player;
        private Label l;
        private BulbC bulb;
        private int numOfSteps;
        private System.Diagnostics.Stopwatch time;
        private bool firstTime;
        private Stopwatch watch;
        private DateTime startingTime;
        private timerC timer;

        public GameC()
        {
            InitializeComponent();
        }

        public GameC(Maze3d maze)
        {
            InitializeComponent();
            myMaze = maze;
            xLength = maze.XLength * 2 + 1;
            yLength = maze.YLength * 2 + 1;
            int z = maze.ZLength;
            numOfSteps = 0;
            curLevel = 0;
            cellHeight = 10;
            cellWidth = 10;
            maxLevel = z;
            timer = new timerC();
            timerPanel.Children.Add(timer);
            firstTime = true;
            prevCol = myMaze.StartPoint.Y * 2 + 1;
            prevRow = myMaze.StartPoint.X * 2 + 1;
            createGrid(xLength, yLength, curLevel);
            createPlayer();
            l = new Label();
            l.Content = "";
            l.Background = Brushes.Khaki;
            l.Width = 200;
            l.Height = 30;
            Canvas.SetBottom(l, 30);
            Canvas.SetLeft(l, 100);
            topPanel.Children.Add(l);
        }

        private void createPlayer()
        {
            player = new PlayerControl();
            player.Width = cellWidth - cellWidth / 10;
            player.Height = cellHeight - cellHeight / 10;
            curCol = myMaze.StartPoint.Y * 2 + 1;
            curRow = myMaze.StartPoint.X * 2 + 1;
            topPanel.Children.Add(player);
        }

        private void createGrid(int x, int y, int curLevel)
        {
            double posX = 0;
            double posY = 0;
            Maze2d maze = myMaze.Maze2DLayers[curLevel] as Maze2d;
            for (int i = 0; i < x; i++)
            {
                posX = 0;
                for (int j = 0; j < y; j++)
                {
                    if (myMaze.GoalPoint.X * 2 + 1 == i && myMaze.GoalPoint.Y * 2 + 1 == j && curLevel + 1 == maxLevel)
                    {
                        GoalC goal = new GoalC();
                        goal.Width = cellWidth;
                        goal.Height = cellHeight;
                        Canvas.SetLeft(goal, posX);
                        Canvas.SetTop(goal, posY);
                        board.Children.Add(goal);
                    }
                    else if (myMaze.StartPoint.X * 2 + 1 == i && myMaze.StartPoint.Y * 2 + 1 == j && curLevel == 0)
                    {
                        StartC start = new StartC();
                        start.Width = cellWidth;
                        start.Height = cellHeight;
                        Canvas.SetLeft(start, posX);
                        Canvas.SetTop(start, posY);
                        board.Children.Add(start);
                    }
                    else if (maze.Grid[i, j] == 1)
                    {
                        WallC wall = new WallC();
                        wall.Width = cellWidth;
                        wall.Height = cellHeight;
                        Canvas.SetTop(wall, posY);
                        Canvas.SetLeft(wall, posX);
                        board.Children.Add(wall);
                    }
                    else
                    {
                        PassC pass = new PassC();
                        pass.Width = cellWidth;
                        pass.Height = cellHeight;
                        Canvas.SetTop(pass, posY);
                        Canvas.SetLeft(pass, posX);
                        board.Children.Add(pass);
                    }
                    posX += cellWidth;
                }
                posY += cellHeight;
            }
            this.curLevel = curLevel;
        }

        private void board_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            board.Children.Clear();
            cellWidth = this.ActualWidth / yLength;
            cellHeight = this.ActualHeight / xLength;
            speedX = cellHeight / 10;
            speedY = cellWidth / 10;
            createGrid(xLength, yLength, curLevel);
            updatePlayer();
            Canvas.SetBottom(l, 0);
            Canvas.SetLeft(l, 0);
            topPanel.Children.Add(l);
            bulb = new BulbC(true);
            bulbPanel.Children.Clear();
            bulbPanel.Children.Add(bulb);
        }

        private void updatePlayer()
        {
            topPanel.Children.Clear();
            player.Width = cellWidth - cellWidth / 10;
            player.Height = cellHeight - cellHeight / 10;
            topPanel.Children.Add(player);
            movePlayer();
        }

        private void movePlayer()
        {
            Canvas.SetLeft(player, curCol * cellWidth);
            Canvas.SetTop(player, curRow * cellHeight);
        }

        public void UserControl_KeyDown(object sender, KeyEventArgs e)
        {

            if (firstTime)
            {
                startingTime = DateTime.Now;
                timer.stratingTime = startingTime;
                firstTime = false;
                //Thread thread = new Thread(() =>
                //{
                //    while (!getToGoalPoint())
                //    {
                //        Thread.Sleep(100);
                //        DateTime endTime = DateTime.Now;
                //        TimeSpan difference = endTime - startingTime;
                //        timeBox.Text = difference.TotalSeconds.ToString();
                //    }
                //});
                //thread.Start();
                //firstTime = false;
                //}
            }
            timer.isMove();
            double up = Canvas.GetTop(player);
            double left = Canvas.GetLeft(player);
            if (Double.IsNaN(up))
            {
                up = 0;
            }
            if (Double.IsNaN(left))
            {
                left = 0;
            }
            double scaleHeight = cellHeight / 8;
            double scaleWidth = cellWidth / 8;
            double leftPlayer = left + scaleWidth;
            double upPlayer = up + scaleHeight;
            double down = up + player.ActualHeight - scaleHeight;
            double right = left + player.ActualWidth - scaleWidth;
            bool isPossible = false;
            switch (e.Key)
            {
                case Key.Up:
                    isPossible = isValidMove(Key.Up, upPlayer);
                    if (isPossible)
                    {
                        up -= speedX;
                        down -= speedX;
                    }
                    break;
                case Key.Down:
                    isPossible = isValidMove(Key.Down, down);
                    if (isPossible)
                    {
                        up += speedX;
                        down += speedX;
                    }
                    break;
                case Key.Left:
                    isPossible = isValidMove(Key.Left, leftPlayer);
                    if (isPossible)
                    {
                        left -= speedY;
                        right -= speedY;
                    }
                    break;
                case Key.Right:
                    isPossible = isValidMove(Key.Right, right);
                    if (isPossible)
                    {
                        left += speedY;
                        right += speedY;
                    }
                    break;
                case Key.PageUp:
                    if (getCol(leftPlayer) == getCol(right) && getCol(leftPlayer) % 2 == 1 && getRow(upPlayer) % 2 == 1 && getRow(upPlayer) == getRow(down))
                    {
                        if (curLevel < maxLevel - 1)
                        {
                            board.Children.Clear();
                            createGrid(myMaze.XLength * 2 + 1, myMaze.YLength * 2 + 1, curLevel + 1);
                        }
                    }
                    break;
                case Key.PageDown:
                    if (getCol(leftPlayer) == getCol(right) && getCol(leftPlayer) % 2 == 1 && getRow(upPlayer) % 2 == 1 && getRow(upPlayer) == getRow(down))
                    {
                        if (curLevel > 0)
                        {
                            board.Children.Clear();
                            createGrid(myMaze.XLength * 2 + 1, myMaze.YLength * 2 + 1, curLevel - 1);
                        }
                    }
                    break;
            }
            if (getCol(leftPlayer) == getCol(right) && getCol(leftPlayer) % 2 == 1 && getRow(upPlayer) % 2 == 1 && getRow(upPlayer) == getRow(down))
            {
                bulb.changeMode(true);
            }
            else
            {
                bulb.changeMode(false);
            }
            if (getToGoalPoint())
            {
                DateTime endTime = DateTime.Now;
                TimeSpan difference = endTime - startingTime;
                //timeBox.Text = difference.TotalSeconds.ToString();
                timer.stop();
                MessageBox.Show("Finished" + "\nIt took : " + timer.lblTime.Content + " seconds" + "\nNumber of steps: " + numOfSteps);
            }
            if (!isPossible)
            {
                return;
            }
            curCol = leftPlayer / cellWidth;
            curRow = upPlayer / cellHeight;
            //number of steps
            if (prevCol != (int)curCol || prevRow != (int)curRow)
            {
                numOfSteps++;
                stepsBox.Text = numOfSteps.ToString();
            }
            prevRow = (int)curRow;
            prevCol = (int)curCol;
            string pos = "Row: " + curRow + " Col: " + curCol;
            l.Content = pos;
            Canvas.SetLeft(player, left);
            Canvas.SetTop(player, up);

        }
        private bool getToGoalPoint()
        {
            if ((int)curCol == myMaze.GoalPoint.Y * 2 + 1 && (int)curRow == myMaze.GoalPoint.X * 2 + 1 && curLevel == maxLevel - 1)
            {
                return true;
            }
            return false;
        }

        private int getRow(double pos)
        {
            int row = (int)(pos / cellHeight);
            return row;
        }

        private int getCol(double pos)
        {
            int col = (int)(pos / cellWidth);
            return col;
        }

        private bool isValidMove(Key key, double position)
        {
            int row = 0, col = 0;
            bool ans = false;
            switch (key)
            {
                case Key.Up:
                    row = (int)(position / cellHeight);
                    col = (int)curCol;
                    break;
                case Key.Down:
                    row = (int)(position / cellHeight);
                    col = (int)curCol;
                    break;
                case Key.Left:
                    col = (int)(position / cellWidth);
                    row = (int)curRow;
                    break;
                case Key.Right:
                    col = (int)(position / cellWidth);
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
    }
}
