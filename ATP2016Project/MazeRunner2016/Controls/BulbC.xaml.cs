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
        /// The constructor get a boolean to determine the bulb mode (on or off)
        /// </summary>
        /// <param name="mode">true=on,false=off</param>
        public BulbC(bool mode)
        {
            InitializeComponent();
            if (mode) //the bulb is on
            {
                Image on = new Image();
                BitmapImage bitImg = new BitmapImage(new Uri(@"/Pics\light on.png", UriKind.Relative));
                on.Source = bitImg;
                main.Children.Add(on);
            }
            else //the bulb is off
            {
                Image off = new Image();
                BitmapImage bitImg = new BitmapImage(new Uri(@"/Pics\light off.png", UriKind.Relative));
                off.Source = bitImg;
                main.Children.Add(off);
            }
        }

        /// <summary>
        /// with this function we can toggle between the modes of the bulb
        /// </summary>
        /// <param name="mode"></param>
        public void changeMode(bool mode)
        {
            main.Children.Clear();
            if (mode) //bulb is on
            {
                Image on = new Image();
                BitmapImage bitImg = new BitmapImage(new Uri(@"/Pics\light on.png", UriKind.Relative));
                on.Source = bitImg;
                main.Children.Add(on);
            }
            else //bulb is off
            {
                Image off = new Image();
                BitmapImage bitImg = new BitmapImage(new Uri(@"/Pics\light off.png", UriKind.Relative));
                off.Source = bitImg;
                main.Children.Add(off);
            }
        }
    }
}
