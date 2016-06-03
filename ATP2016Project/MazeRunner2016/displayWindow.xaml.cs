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
    /// Interaction logic for displayWindow.xaml
    /// </summary>
    public partial class displayWindow : Window
    {
        private IModel model;
        private IView view;

        public displayWindow()
        {
            InitializeComponent();
        }

        public displayWindow(IView view, IModel model)
        {
            this.view = view;
            this.model = model;
        }
    }
}
