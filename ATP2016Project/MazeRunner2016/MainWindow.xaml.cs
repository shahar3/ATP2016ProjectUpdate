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
using MazeRunner2016.Controls;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Animation;

namespace MazeRunner2016
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DockPanel myDock;
        private IModel model;
        private IView view;

        public MainWindow()
        {
            InitializeComponent();
            model = new Model();
            view = new View();
            Presenter p = new Presenter(view, model);
            menu.setView(view);
            menu.setPanel(actionPanel);
            myDock = solutionInfoPanel;
            SideMenuControl sideMenuControl = new SideMenuControl(view, model);
            sideMenuPanel.Children.Add(sideMenuControl);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ExitDialog exit = new ExitDialog("Are you sure you want to exit?");
            if (exit.ShowDialog() == true)
            {
                view.activateEvent(sender, new MazeEventArgs("Exit"));
            }
        }
    }
}
