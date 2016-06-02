using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MazeRunner2016
{
    class Presenter
    {
        private View m_ui;
        private Model m_model;

        public Presenter(IView ui, IModel model)
        {
            m_ui = ui as View;
            m_model = model as Model;
            //initialize the events for the view and model layer
            initEvents();
        }

        private void initEvents()
        {
            m_ui.ViewChanged += delegate (Object sender, EventArgs e)
            {
                Button b = sender as Button;
                if (b.Name == "b1")
                {
                    b.FontSize = 30;
                }
                if (b.Name == "b2")
                {
                    b.FontSize = 20;
                }
                if (b.Name == "b3")
                {
                    b.FontSize = 10;
                }
            };
        }
    }
}
