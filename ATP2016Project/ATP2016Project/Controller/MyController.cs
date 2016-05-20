using ATP2016Project.Model;
using ATP2016Project.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATP2016Project.Controller
{
    class MyController : IController
    {
        private IView m_view;
        private IModel m_model;

        public MyController()
        {
        }

        public void SetModel(IModel model)
        {
            m_model = model;
        }

        public void SetView(IView view)
        {
            m_view = view;
        }

        public void Output(string output)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, ICommand> GetCommands()
        {
            throw new NotImplementedException();
        }
    }
}
