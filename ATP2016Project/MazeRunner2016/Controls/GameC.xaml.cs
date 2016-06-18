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
using MazeRunner2016;
using System.Windows.Controls.Primitives;

namespace MazeRunner2016.Controls
{
    /// <summary>
    /// Interaction logic for GameC.xaml
    ///this user control represent  our maze game
    ///we are define what will happend when the user press on the game button,or if the size window change and more
    /// </summary>
    public partial class GameC : UserControl
    {
        private View m_view;

        #region maze fields
        private Maze3d myMaze;
        private int curLevel;
        private int xLength;
        private int yLength;
        private int maxLevel;
        private string m_mazeName;
        #endregion
        #region player fields
        private PlayerControl player;
        private double cellHeight;
        private double cellWidth;
        private int prevCol, prevRow;
        private bool isDragged;
        private double speedX, speedY;
        private double curCol, curRow;
        private double downBoundry;
        private double rightBoundry;
        private double leftBoundry;
        private double upBoundry;
        private double xZoom;
        private double yZoom;
        private bool firstTimeScaling;
        private object movingObject;
        #endregion
        #region details panel fields
        private BulbC bulb;
        private DateTime startingTime;
        private int numOfSteps;
        private System.Diagnostics.Stopwatch time;
        private bool firstTime;
        private timerC timer;
        #endregion
        private bool withSolution;
        private bool thereIsSolution;
        private double up;
        private double left;
        private double gameHeight;
        private double gameWidth;

        public double FirstXPos { get; private set; }
        public double FirstYPos { get; private set; }
        public double FirstArrowXPos { get; private set; }
        public double FirstArrowYPos { get; private set; }

        public GameC()
        {
            InitializeComponent();
        }
        /// <summary>
        /// constructor 
        /// </summary>
        /// <param name="maze">maze</param>
        /// <param name="mazeName">maze name</param>
        /// <param name="view">view</param>
        public GameC(Maze3d maze, string mazeName, View view)
        {
            InitializeComponent();
            m_view = view;
            myMaze = maze;
            m_mazeName = mazeName;
            xLength = maze.XLength * 2 + 1;
            yLength = maze.YLength * 2 + 1;
            int z = maze.ZLength;
            withSolution = false;
            numOfSteps = 0;
            curLevel = 0;
            cellHeight = 10;
            cellWidth = 10;
            maxLevel = z;
            this.PreviewMouseWheel += mouseWheel;//this event use for zoom in and out in the game place
            xZoom = 1;
            yZoom = 1;
            firstTimeScaling = true;
            thereIsSolution = false;
            isDragged = false;
            timer = new timerC();
            timerPanel.Children.Add(timer);
            timer.ToolTip = "Time in seconds";
            firstTime = true;
            prevCol = myMaze.StartPoint.Y * 2 + 1;
            prevRow = myMaze.StartPoint.X * 2 + 1;
            createGrid(xLength, yLength, curLevel);//create the grid that use for our game
            createPlayer();//create the player that use for our game
        }

        #region drag player and zoom in/out
        /// <summary>
        /// this function activated and check if is zoom in or out ,and deal with it
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">mouse wheel</param>
        private void mouseWheel(object sender, MouseWheelEventArgs e)
        {
            //check if the Ctrl button and whell mouse are pressed and scroll
            bool handle = (Keyboard.Modifiers & ModifierKeys.Control) > 0;
            if (!handle)
                return;
            //if its scroll up we increase the zoom
            if (e.Delta > 0)
            {
                itsZoomIn();
            }
            else//if its scroll up we increase the zoom
            {
                istZoomOut();
            }
            //scaleTransform = new ScaleTransform(xZoom, yZoom);
            if (firstTimeScaling)
            {
                gameWidth = gameZone.ActualWidth;
                gameHeight = gameZone.ActualHeight;
                gameZone.Width = gameZone.ActualWidth;
                gameZone.Height = gameZone.ActualHeight;
                firstTimeScaling = false;
            }//this is our limitation for max zoom in
            if (xZoom > 2.0)
            {
                xZoom = 2;
                yZoom = 2;
                return;
            }
            //this is our limitation for max zoom out
            else if (xZoom < 0.5)
            {
                xZoom = 0.5;
                yZoom = 0.5;
            }
            //gameZone.RenderTransform = scaleTransform;
            gameZone.UpdateLayout();
        }
        /// <summary>
        /// this function is Auxiliary function for zoom in
        /// </summary>
        private void istZoomOut()
        {
            xZoom /= 1.1;
            yZoom /= 1.1;
            gameZone.Width /= 1.1;
            gameZone.Height /= 1.1;
        }
        /// <summary>
        /// this function is Auxiliary function for zoom out
        /// </summary>
        private void itsZoomIn()
        {
            xZoom *= 1.1;
            yZoom *= 1.1;
            gameZone.Width *= 1.1;
            gameZone.Height *= 1.1;
        }

        /// <summary>
        /// this function deal with drag player with the mouse in the game zone
        /// and avoid collision with the walls
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">mouse event</param>
        private void mouseMove(object sender, MouseEventArgs e)
        {
            //check if the left mouse button is click
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isDragged = true;
                left = e.GetPosition((movingObject as FrameworkElement).Parent as FrameworkElement).X - FirstXPos;
                up = e.GetPosition((movingObject as FrameworkElement).Parent as FrameworkElement).Y - FirstYPos;
                boundaryPlayerAndCurPosition();
                //collision detection
                if (!isValidMove(Key.Left, leftBoundry) || !isValidMove(Key.Up, upBoundry) || !isValidMove(Key.Down, downBoundry) || !isValidMove(Key.Right, rightBoundry))
                {
                    isDragged = false;
                    return;
                }
                if (getToGoalPoint())
                {
                    goalPointFinishMsg();
                }
                //number of steps
                if (prevCol != (int)curCol || prevRow != (int)curRow)
                {
                    numOfSteps++;
                    stepsBox.Content = numOfSteps.ToString();
                }
                prevRow = (int)curRow;
                prevCol = (int)curCol;
                (movingObject as FrameworkElement).SetValue(Canvas.LeftProperty, left);
                (movingObject as FrameworkElement).SetValue(Canvas.TopProperty, up);
            }
        }

        /// <summary>
        /// this function is Auxiliary function that defined the boundary of the player and 
        /// current position that help us to detection collision
        /// </summary>
        private void boundaryPlayerAndCurPosition()
        {
            downBoundry = up + cellHeight - cellHeight / 3;
            rightBoundry = left + cellWidth - cellWidth / 3;
            upBoundry = up + cellHeight / 3;
            leftBoundry = left + cellWidth / 3;
            curCol = leftBoundry / cellWidth;
            curRow = upBoundry / cellHeight;
        }

        /// <summary>
        /// this function defined what are we do if the mouse left button is up
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">mouse event</param>
        private void mouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            movingObject = null;
            isDragged = false;
            //player.ReleaseMouseCapture();
        }

        /// <summary>
        /// this function defined what are we do if the mouse left button is down
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">mouse event</param>
        private void mouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (firstTime)
            {
                activatedTheStopper();
            }
            FirstXPos = e.GetPosition(sender as Control).X;
            FirstYPos = e.GetPosition(sender as Control).Y;
            FirstArrowXPos = e.GetPosition((sender as Control).Parent as Control).X - FirstXPos;
            FirstArrowYPos = e.GetPosition((sender as Control).Parent as Control).Y - FirstYPos;
            movingObject = sender;
            //player.CaptureMouse();
        }

        /// <summary>
        /// this function is auxiliary function that activate the stopper
        /// that help the user to know how much time he's playing 
        /// </summary>
        private void activatedTheStopper()
        {
            startingTime = DateTime.Now;
            timer.stratingTime = startingTime;
            timer.isMove();
            firstTime = false;
        }

        #endregion

        /// <summary>
        /// this function check if the user move with the arrows button or with the
        /// mouse are valid
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="position">position</param>
        /// <returns>valid or not</returns>
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


        /// <summary>
        /// this function create our player thet represent in the maze
        /// </summary>
        private void createPlayer()
        {
            player = new PlayerControl();//new player control
            //events for move eith the mouse 
            player.MouseLeftButtonDown += mouseLeftButtonDown;
            player.MouseLeftButtonUp += mouseLeftButtonUp;
            player.MouseMove += mouseMove;
            player.Width = cellWidth - cellWidth / 10;
            player.Height = cellHeight - cellHeight / 10;
            curCol = myMaze.StartPoint.Y * 2 + 1;
            curRow = myMaze.StartPoint.X * 2 + 1;
            topPanel.Children.Add(player);
        }

        /// <summary>
        /// This function created the board game(the maze board)
        /// it activated from the constructor with level 1 and if the user up level or down
        /// </summary>
        /// <param name="x">x length</param>
        /// <param name="y">y length</param>
        /// <param name="curLevel">which layer</param>
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
                    //check if we in the goal point, if it is we set there sign(user control with picture)
                    if (myMaze.GoalPoint.X * 2 + 1 == i && myMaze.GoalPoint.Y * 2 + 1 == j && curLevel + 1 == maxLevel)
                    {
                        setGoalPointInGrid(posX, posY);
                    }
                    //check if we in the start point, if it is we set there sign(user control with picture)
                    else if (myMaze.StartPoint.X * 2 + 1 == i && myMaze.StartPoint.Y * 2 + 1 == j && curLevel == 0)
                    {
                        setStartPoint(posX, posY);
                    }
                    //check if we in the wall position, if it is we set there sign(user control with picture)
                    else if (maze.Grid[i, j] == 1)
                    {
                        WallC wall = new WallC();
                        wall.Width = cellWidth;
                        wall.Height = cellHeight;
                        Canvas.SetTop(wall, posY);
                        Canvas.SetLeft(wall, posX);
                        board.Children.Add(wall);
                    }
                    else if (maze.Grid[i, j] == 2 && withSolution)
                    {
                        solutionC solution = new solutionC();
                        solution.Width = cellWidth;
                        solution.Height = cellHeight;
                        Canvas.SetTop(solution, posY);
                        Canvas.SetLeft(solution, posX);
                        board.Children.Add(solution);

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

        private void setStartPoint(double posX, double posY)
        {
            StartC start = new StartC();
            start.Width = cellWidth;
            start.Height = cellHeight;
            Canvas.SetLeft(start, posX);
            Canvas.SetTop(start, posY);
            board.Children.Add(start);
        }

        private void setGoalPointInGrid(double posX, double posY)
        {
            GoalC goal = new GoalC();
            goal.Width = cellWidth;
            goal.Height = cellHeight;
            Canvas.SetLeft(goal, posX);
            Canvas.SetTop(goal, posY);
            board.Children.Add(goal);
        }

        private void board_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            board.Children.Clear();
            cellWidth = board.ActualWidth / yLength;
            cellHeight = board.ActualHeight / xLength;
            speedX = cellHeight / 10;
            speedY = cellWidth / 10;
            createGrid(xLength, yLength, curLevel);
            updatePlayer();
            bulb = new BulbC(true);
            bulbPanel.Children.Clear();
            bulbPanel.Children.Add(bulb);
            if (thereIsSolution)
            {
                solBoard.Children.Clear();
                markSolution();
            }
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
            if (isDragged)
            {
                return;
            }
            if (firstTime)
            {
                startingTime = DateTime.Now;
                timer.stratingTime = startingTime;
                firstTime = false;
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
                            if (thereIsSolution)
                            {
                                solBoard.Children.Clear();
                                markSolution();
                            }
                        }
                        else
                        {
                            MessageBox.Show("You are in the maximum level");
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
                            if (thereIsSolution)
                            {
                                solBoard.Children.Clear();
                                markSolution();
                            }
                        }
                        else
                        {
                            MessageBox.Show("You are in the minimum level");
                        }
                    }
                    break;
                case Key.R:
                    gameZone.Width = gameWidth;
                    gameZone.Height = gameHeight;
                    gameZone.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Hidden);
                    gameZone.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Hidden);
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
                goalPointFinishMsg();
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
                stepsBox.Content = numOfSteps.ToString();
            }
            prevRow = (int)curRow;
            prevCol = (int)curCol;
            string pos = "Row: " + curRow + " Col: " + curCol;
            Canvas.SetLeft(player, left);
            Canvas.SetTop(player, up);

        }

        private void goalPointFinishMsg()
        {
            DateTime endTime = DateTime.Now;
            TimeSpan difference = endTime - startingTime;
            //timeBox.Text = difference.TotalSeconds.ToString();
            MessageBox.Show("Finished" + "\nIt took : " + timer.lblTime.Content + " seconds" + "\nNumber of steps: " + numOfSteps);
            timer.stop();
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

        private void displaySolutionCB_Checked(object sender, RoutedEventArgs e)
        {
            solBoard.Visibility = Visibility.Hidden;
        }

        private void restartBtn_Click(object sender, RoutedEventArgs e)
        {
            board.Children.Clear();
            withSolution = false;
            firstTime = true;
            thereIsSolution = false;
            isDragged = false;
            firstTimeScaling = true;
            createGrid(myMaze.XLength * 2 + 1, myMaze.YLength * 2 + 1, 0);
            topPanel.Children.Remove(player);
            createPlayer();
            curLevel = 0;
            movePlayer();
            stepsBox.Content = "0";
            numOfSteps = 0;
            timer.stop();
            timer.start();
        }

        private void solveBtn_Click(object sender, RoutedEventArgs e)
        {
            string nameAlgo = comboBoxAlgo.SelectionBoxItem as string;
            string[] parameters = new string[2];
            parameters[0] = m_mazeName;
            parameters[1] = nameAlgo;
            m_view.SolutionRetrieved += delegate ()
            {
                Dispatcher.Invoke(new Action(markSolution));
            };
            m_view.activateEvent(sender, new MazeEventArgs(parameters));
            if (m_view.anotherThread())
            {
                markSolution();
                m_view.isMainThread = false;
            }
            thereIsSolution = true;
        }

        public void solutionRetrieved()
        {
            markSolution();
        }

        public void markSolution()
        {
            ISearchable searchableMaze = new SearchableMaze3d(myMaze);
            //we need to wait for solution from the thread
            Solution mazeSol = m_view.getSolution();
            (searchableMaze as SearchableMaze3d).markSolutionInGrid(mazeSol);
            markSolInGrid(mazeSol);
        }

        private void displaySolutionCB_Unchecked(object sender, RoutedEventArgs e)
        {
            solBoard.Visibility = Visibility.Visible;
        }



        private void markSolInGrid(Solution sol)
        {
            foreach (MazeState state in sol.getSolutionPath())
            {
                solutionC solCBetween = new solutionC();
                solCBetween.Width = cellWidth;
                solCBetween.Height = cellHeight;
                Position posToMark = (state as MazeState).Position;
                if (state.Previous != null)
                {
                    bool upOneLevel = !((state.Previous as MazeState).Position.Z == state.Position.Z);//check if we went to the upper level
                    if (!upOneLevel)
                        placeBetweenGrid(state.Position, (state.Previous as MazeState).Position, solCBetween);
                }
                solutionC solC = new solutionC();
                solC.Width = cellWidth;
                solC.Height = cellHeight;
                placeInGrid(posToMark.X, posToMark.Y, posToMark.Z, solC);
            }
        }

        private void placeBetweenGrid(Position prev, Position cur, UIElement elem)
        {
            if (prev.X == cur.X)
            {
                if (prev.Y > cur.Y)
                {
                    placeInGrid(prev.X, prev.Y - 1, cur.Z, elem);
                }
                else
                {
                    placeInGrid(prev.X, prev.Y + 1, cur.Z, elem);
                }
            }
            else
            {
                if (prev.X > cur.X)
                {
                    placeInGrid(prev.X - 1, prev.Y, cur.Z, elem);
                }
                else
                {
                    placeInGrid(prev.X + 1, prev.Y, cur.Z, elem);
                }
            }
        }

        private void placeInGrid(int row, int col, int level, UIElement element)
        {
            if (level == curLevel)
            {
                solBoard.Children.Add(element);
                Canvas.SetLeft(element, col * cellWidth);
                Canvas.SetTop(element, row * cellHeight);
            }
        }
    }
}
