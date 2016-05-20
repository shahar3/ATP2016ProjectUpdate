using ATP2016Project.Controller;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATP2016Project.View
{
    class CLI : IView
    {
        private Stream m_input;
        private Stream m_output;
        private Dictionary<string, ICommand> m_commands;
        private const string cursor = ">>";
        private IController m_controller;
        private Dictionary<string, ICommand> dictionary;

        public CLI(IController controller)
        {
            m_controller = controller;
            m_input = Console.OpenStandardInput();
            m_output = Console.OpenStandardOutput();
        }

        public CLI(IController controller, Dictionary<string, ICommand> commands) : this(controller)
        {
            this.m_commands = commands;
        }

        public void Start()
        {
            printInstructions();
        }

        private void printInstructions()
        {
            throw new NotImplementedException();
        }

        public void Output(string output)
        {
            throw new NotImplementedException();
        }
    }
}
