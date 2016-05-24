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
    /// interacts with the model layer and the view layer
    /// in our MVC design pattern
    /// </summary>
    interface IController
    {
        /// <summary>
        /// set the model layer to be the object we get
        /// </summary>
        /// <param name="model">the model object</param>
        void SetModel(IModel model);
        /// <summary>
        /// set the view layer to be the object we get
        /// </summary>
        /// <param name="view">the view object</param>
        void SetView(IView view);
        /// <summary>
        /// get the commands for our CLI that we set in our dictionary
        /// </summary>
        /// <returns>the dictionary initialized with the commands</returns>
        Dictionary<string, ICommand> GetCommands();
    }
}
