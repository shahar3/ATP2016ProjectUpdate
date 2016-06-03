using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner2016
{
    class MazeEventArgs : EventArgs
    {
        private string[] m_params;

        public string[] Params
        {
            get { return m_params; }
            set { m_params = value; }
        }

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
