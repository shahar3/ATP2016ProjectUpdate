using ATP2016Project.Model;
using ATP2016Project.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATP2016Project.Controller
{
    /// <summary>
    /// the facade of the contoroller layer
    /// </summary>
    interface IController
    {
        void SetModel(IModel model);
        void SetView(IView view);
        void Output(string output);
        Dictionary<string, ICommand> GetCommands();
    }
}
