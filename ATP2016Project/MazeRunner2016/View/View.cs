using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MazeRunner2016
{
    public class View : IView
    {
        private object m_output;

        public View(object output)
        {
            m_output = output;
        }

        public event somethingHappened ViewChanged;

        public void activateEvent(object sender, EventArgs e)
        {
            ViewChanged(sender, e);
        }

        internal void displayInTextBox(string notification)
        {
            (m_output as TextBox).Text = notification;
        }
    }
}
