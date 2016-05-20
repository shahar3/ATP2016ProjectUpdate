using ATP2016Project.Model;
using ATP2016Project.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATP2016Project.Controller
{
    abstract class ACommand : ICommand
    {
        protected IModel m_model;
        protected IView m_view;
        public ACommand(IModel model, IView view)
        {
            m_model = model;
            m_view = view;
        }
        public abstract void DoCommand(params string[] parameters);

        public Dictionary<string, ICommand> GetCommands()
        {
            throw new NotImplementedException();
        }

        public string GetDescription()
        {
            throw new NotImplementedException();
        }

        public abstract string GetName();
    }
}
