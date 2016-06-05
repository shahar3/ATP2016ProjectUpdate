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

namespace MazeRunner2016
{
    /// <summary>
    /// Interaction logic for generateWindow.xaml
    /// </summary>
    public partial class generateWindow : Window
    {
        private IView m_ui;
        private IModel m_model;
        public generateWindow()
        {
            InitializeComponent();
        }

        public generateWindow(IView ui, IModel model) : base()
        {
            InitializeComponent();
            m_ui = ui;
            m_model = model;
        }

        private void WidthMaze_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            widthVal.Text = ((int)WidthMaze.Value).ToString();
        }

        private void HeightMaze_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            heightVal.Text = ((int)HeightMaze.Value).ToString();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string[] parameters = new string[4];
            parameters[0] = textBox.Text;
            parameters[1] = widthVal.Text;
            parameters[2] = heightVal.Text;
            parameters[3] = "2";
            m_ui.activateEvent(sender, new MazeEventArgs(parameters));
            this.Close();
        }
    }
}
