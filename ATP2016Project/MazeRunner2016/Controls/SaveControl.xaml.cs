using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for SaveControl.xaml
    /// </summary>
    public partial class SaveControl : UserControl
    {
        private IView m_view;
        private IModel m_model;

        public SaveControl()
        {
            InitializeComponent();
        }

        public SaveControl(IModel model, IView view)
        {
            InitializeComponent();
            m_model = model;
            m_view = view;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string[] commandName = new string[] { "getMazesNames" };
            m_view.activateEvent(sender, new MazeEventArgs(commandName));
            comboBoxSave.ItemsSource = m_view.getMazesNames();
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            //the name maze that will save
            string mazeToSave = comboBoxSave.SelectedItem.ToString();
            //get tha path
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.ShowDialog();
            string fullPath = sfd.FileName;
            string[] parameters = new string[2];
            parameters[0] = mazeToSave;
            parameters[1] = fullPath;
            //event
            m_view.activateEvent(sender, new MazeEventArgs(parameters));
            string saveMessage = m_view.getSaveMessage();
            MessageBox.Show(saveMessage);
        }
    }
}
