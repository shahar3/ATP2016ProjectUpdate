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

        /// <summary>
        /// the default constructor
        /// </summary>
        public GenerateControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// the constructor that we use
        /// </summary>
        /// <param name="ui">the view layer</param>
        public GenerateControl(IView ui) : base()
        {
            InitializeComponent();
            m_ui = ui;
        }

        /// <summary>
        /// when we press on generate it collects the data and performes the generate command
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">args</param>
        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text.Trim() == "") //in case we didn't type anything
            {
                MessageBox.Show("You didn't type a name");
                return;
            }
            string[] parameters = getMazeProperties(); //get properties
            m_ui.activateEvent(sender, new MazeEventArgs(parameters)); //tell the presenter to generate
        }

        /// <summary>
        /// collect the properties of the maze
        /// </summary>
        /// <returns>array of properties</returns>
        private string[] getMazeProperties()
        {
            string[] parameters = new string[4];
            parameters[0] = textBox.Text;
            parameters[1] = heightVal.Text;
            parameters[2] = widthVal.Text;
            parameters[3] = levelVal.Text;
            return parameters;
        }
    }
}
