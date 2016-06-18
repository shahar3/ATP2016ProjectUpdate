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
    /// Interaction logic for SideMenuControl.xaml
    /// </summary>
    /// fdg
    /// 
    public partial class SideMenuControl : UserControl
    {
        private IView m_view;
        private IModel m_model;

        public SideMenuControl()
        {
            InitializeComponent();
        }

        public SideMenuControl(IView view, IModel model)
        {
            InitializeComponent();
            m_view = view;
            m_model = model;
            m_model.activateEvent("getFunctions", "");
            sideMenu.ItemsSource = m_view.getFunctions();
        }

        public void sideMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sideMenu.SelectedItem == null)
            {
                return;
            }
            string msg = sideMenu.SelectedItem.ToString();
            Window mainWindow = App.Current.MainWindow;
            DockPanel actionPanel = (mainWindow as MainWindow).actionPanel;
            switch (msg)
            {
                case "Home":
                    Home home = new Home();
                    actionPanel.Children.Clear();
                    actionPanel.Children.Add(home);
                    break;
                case "Play":
                case "Display maze":
                    DisplayControl displayControl = new DisplayControl(m_view, m_model);
                    actionPanel.Children.Clear();
                    actionPanel.Children.Add(displayControl);
                    break;
                case "Load maze":
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
                    break;
                case "Save maze":
                    SaveControl saveControl = new SaveControl(m_view);
                    actionPanel.Children.Clear();
                    actionPanel.Children.Add(saveControl);
                    break;
                case "Remove maze":
                    RemoveControl remove = new RemoveControl(m_view);
                    actionPanel.Children.Clear();
                    actionPanel.Children.Add(remove);
                    break;
                case "Generate maze":
                    GenerateControl generateControl = new GenerateControl(m_view);
                    actionPanel.Children.Clear();
                    actionPanel.Children.Add(generateControl);
                    break;
                case "Exit":
                    ExitDialog exit = new ExitDialog("Are you sure you want to exit?");
                    if (exit.ShowDialog() == true)
                    {
                        m_view.activateEvent(sender, new MazeEventArgs("Exit"));
                    }
                    break;
                default:
                    break;
            }
            sideMenu.SelectedItem = null;

        }




    }
}
