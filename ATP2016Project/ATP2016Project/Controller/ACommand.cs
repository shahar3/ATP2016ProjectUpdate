using ATP2016Project.Model;
using ATP2016Project.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ATP2016Project.Controller
{
    abstract class ACommand : ICommand
    {
        protected IModel m_model;
        protected IView m_view;
        protected object myLock = new object();
        protected static List<Thread> m_threads = new List<Thread>();

        public ACommand(IModel model, IView view)
        {
            m_model = model;
            m_view = view;
        }
        public abstract void DoCommand(params string[] parameters);

        public abstract string GetDescription();

        public abstract string GetName();

        public Object getLock()
        {
            return myLock;
        }
    }
}
