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
using System.Windows.Threading;

namespace MazeRunner2016.Controls
{
    /// <summary>
    /// Interaction logic for timerC.xaml
    /// </summary>
    public partial class timerC : UserControl
    {
        public DateTime stratingTime;
        private bool move;
        private string prevTime;
        private bool isStop;

        public timerC()
        {
            InitializeComponent();
            move = false;
            prevTime = "0";
            isStop = false;
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            if (!isStop)
            {

                if (!move)
                {
                    lblTime.Content = 0;
                }
                else
                {
                    lblTime.Content = ((int)(DateTime.Now - stratingTime).TotalSeconds).ToString();
                    prevTime = ((int)(DateTime.Now - stratingTime).TotalSeconds).ToString();
                }
            }
            else
            {
                lblTime.Content = prevTime;
            }
        }
        public void isMove()
        {
            move = true;
        }
        public void stop()
        {
            isStop = true;
        }
    }
}
