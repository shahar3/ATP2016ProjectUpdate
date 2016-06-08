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
    /// Interaction logic for ExitDialog.xaml
    /// </summary>
    public partial class ExitDialog : Window
    {
        public ExitDialog(string question)
        {
            InitializeComponent();
            lblQuestion.Content = question;
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}

