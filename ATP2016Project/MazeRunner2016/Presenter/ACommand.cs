using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MazeRunner2016
{
    public abstract class ACommand : ICommand
    {
        protected IModel m_model;
        protected IView m_view;
        protected static List<Thread> m_threads = new List<Thread>();

        /// <summary>
        /// The constructor
        /// gets a model and a view objects and set them to be
        /// the controller model and view layers
        /// </summary>
        /// <param name="model">model object</param>
        /// <param name="view">view object</param>
        public ACommand(IModel model, IView view)
        {
            m_model = model;
            m_view = view;
        }
        public abstract void DoCommand(params string[] parameters);

        public abstract string GetDescription();

        public abstract string GetName();

        /// <summary>
        /// 
        /// </summary>
    }
}
