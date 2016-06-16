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
    /// Interaction logic for BulbC.xaml
    /// </summary>
    public partial class BulbC : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        public BulbC(bool mode)
        {
            InitializeComponent();
            if (mode)
            {
                Image on = new Image();
                BitmapImage bitImg = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"Resources\light on.png", UriKind.Absolute));
                on.Source = bitImg;
                main.Children.Add(on);
            }
            else
            {
                Image off = new Image();
                BitmapImage bitImg = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"Resources\light off.png", UriKind.Absolute));
                off.Source = bitImg;
                main.Children.Add(off);
            }
        }

        public void changeMode(bool mode)
        {
            main.Children.Clear();
            if (mode)
            {
                Image on = new Image();
                BitmapImage bitImg = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"Resources\light on.png", UriKind.Absolute));
                on.Source = bitImg;
                main.Children.Add(on);
            }
            else
            {
                Image off = new Image();
                BitmapImage bitImg = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"Resources\light off.png", UriKind.Absolute));
                off.Source = bitImg;
                main.Children.Add(off);
            }
        }
    }
}
