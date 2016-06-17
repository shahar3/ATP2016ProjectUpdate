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
using System.Windows.Shapes;
using MazeLib;
using MazeRunner2016.Controls;
using System.Windows.Media.Animation;

namespace MazeRunner2016
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private GameC myGame;
        private Maze3d myMaze;

        public GameWindow()
        {
            InitializeComponent();
        }

        public GameWindow(Maze3d myMaze, string mazeName, IView view)
        {
            InitializeComponent();
            this.myMaze = myMaze;
            myGame = new GameC(myMaze, mazeName, view as View);
            myGame.Focus();
            mazePanel.Children.Add(myGame);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            myGame.UserControl_KeyDown(sender, e);
        }
    }
}
