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

namespace MazeRunner2016
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IModel model;
        private IView view;

        public MainWindow()
        {
            InitializeComponent();
            model = new Model();
            view = new View(t);
            Presenter p = new Presenter(view, model);
            generate3dMazeBtn.Click += generateClick;
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            EventArgs args = new MazeEventArgs("dana", "10", "10", "2");
            (args as MazeEventArgs).TextBox = t;
        }

        private void generateClick(object sender, RoutedEventArgs e)
        {
            generateWindow w = new generateWindow(view, model);
            w.Show();

        }
    }
}
