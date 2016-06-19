using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner2016
{
    /// <summary>
    /// this is a costum made class. 
    /// it's only purpose is to pass the command name when the view layer trigger an event
    /// </summary>
    class MazeEventArgs : EventArgs
    {
        private string[] m_params;

        public string[] Params
        {
            get { return m_params; }
            set { m_params = value; }
        }

        /// <summary>
        /// the defualt constructor. 
        /// get the details of the event
        /// </summary>
        /// <param name="args">the event arguments</param>
        public MazeEventArgs(params string[] args)
        {
            m_params = args;
        }

        private Object m_textBox;

        public Object TextBox
        {
            get { return m_textBox; }
            set { m_textBox = value; }
        }

    }
}
