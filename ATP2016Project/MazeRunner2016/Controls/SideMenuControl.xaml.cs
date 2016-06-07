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
    }
}
