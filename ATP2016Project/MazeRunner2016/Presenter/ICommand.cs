using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MazeRunner2016
{
    /// <summary>
    /// one of the main cores of our CLI
    /// set the interface for the commands we have in our system.
    /// we can get the relevant information about the commands and
    /// perform them
    /// </summary>
    public interface ICommand
    {
        void DoCommand(params string[] parameters);
        string GetName();
        string GetDescription();
    }
}
