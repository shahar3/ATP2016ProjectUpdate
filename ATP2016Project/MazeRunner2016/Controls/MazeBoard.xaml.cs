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
    /// Interaction logic for MazeBoard.xaml
    /// </summary>
    public partial class MazeBoard : UserControl
    {
        private IView view;
        private IModel model;
        private Maze3d myMaze;

        public MazeBoard()
        {
            InitializeComponent();
        }

        public MazeBoard(IView view, IModel model, Object mazeObject)
        {
            this.view = view;
            this.model = model;
            //get the maze dimensions
            myMaze = mazeObject as Maze3d;
            int x = (myMaze.Maze2DLayers[0] as Maze2d).Grid.GetLength(0);
            int y = (myMaze.Maze2DLayers[0] as Maze2d).Grid.GetLength(1);
            int z = myMaze.ZLength;
            //maxLevel = z;
            //string msg = string.Format("The maze dimenstions are x:{0} y:{1} z:{2}", x, y, z);
            //MessageBox.Show(msg);
            //createGrid(x, y, 0);
            //initializeGrid(myMaze, levelsGrid[0], 0, true);
        }
    }
}
