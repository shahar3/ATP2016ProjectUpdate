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
    /// Interaction logic for GenerateControl.xaml
    /// </summary>
    public partial class GenerateControl : UserControl
    {
        private IView m_ui;

        public GenerateControl()
        {
            InitializeComponent();
        }

        public GenerateControl(IView ui) : base()
        {
            InitializeComponent();
            m_ui = ui;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string[] parameters = new string[4];
            parameters[0] = textBox.Text;
            parameters[1] = heightVal.Text;
            parameters[2] = widthVal.Text;
            parameters[3] = levelVal.Text;
            m_ui.activateEvent(sender, new MazeEventArgs(parameters));
        }
    }
}
