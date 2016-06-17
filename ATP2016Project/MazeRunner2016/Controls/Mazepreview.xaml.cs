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
    /// Interaction logic for Mazepreview.xaml
    /// </summary>
    public partial class Mazepreview : UserControl
    {
        private string mazeToShow;
        private Maze3d myMaze;
        private IView m_view;
        private Dictionary<int, Grid> levelsGrid;

        public Mazepreview(Maze3d myMaze, IView view, string mazeToShow)
        {
            InitializeComponent();
            this.m_view = view;
            levelsGrid = new Dictionary<int, Grid>();
            //get the maze dimensions
            myMaze = myMaze as Maze3d;
            int x = (myMaze.Maze2DLayers[0] as Maze2d).Grid.GetLength(0);
            int y = (myMaze.Maze2DLayers[0] as Maze2d).Grid.GetLength(1);
            int z = myMaze.ZLength;
            createGrid(x, y, 0);
            initializeGrid(myMaze, levelsGrid[0], 0);
            mazePanel.Children.Add(levelsGrid[0]);
        }

        private void createGrid(int x, int y, int level)
        {
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

        private void initializeGrid(Maze3d myMaze, Grid mazeGrid, int curLevel)
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
                    else if (myMazeGrid.Grid[i, j] == 0)
                    {
                        cell.Background = Brushes.White;
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
    }
}
