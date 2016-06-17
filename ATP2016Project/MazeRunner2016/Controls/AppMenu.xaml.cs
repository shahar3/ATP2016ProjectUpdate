using Microsoft.Win32;
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
    /// Interaction logic for AppMenu.xaml
    /// </summary>
    public partial class AppMenu : UserControl
    {
        private IView m_view;
        private Panel panel;
        public AppMenu()
        {
            //init
            InitializeComponent();
        }

        public void setView(IView view)
        {
            m_view = view;
        }

        public void setPanel(Panel actionPanel)
        {
            panel = actionPanel;
        }

        private void clickLoad(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Maze files (*.maze)|*.maze";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            if (openFileDialog.ShowDialog() == true)
            {
                string[] args = new string[2];
                string path = openFileDialog.FileName;
                args[0] = path; //maze path
                CustomDialog loadDialog = new CustomDialog("Please enter a name for the maze:", "MazeName");
                if (loadDialog.ShowDialog() == true)
                {
                    args[1] = loadDialog.Answer; //maze name
                    m_view.activateEvent(sender, new MazeEventArgs(args));
                    string msgToShow = m_view.getLoadMessage();
                    MessageBox.Show(msgToShow);
                }
            }
        }

        private void clickSave(object sender, RoutedEventArgs e)
        {
            SaveControl saveControl = new SaveControl(m_view);
            panel.Children.Clear();
            panel.Children.Add(saveControl);
        }

        private void clickNew(object sender, RoutedEventArgs e)
        {
            GenerateControl generateControl = new GenerateControl(m_view);
            panel.Children.Clear();
            panel.Children.Add(generateControl);
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {

        }
    }
}
