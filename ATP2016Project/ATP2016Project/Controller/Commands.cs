using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ATP2016Project.Model;
using ATP2016Project.View;
using System.IO;

namespace ATP2016Project.Controller
{
    class CommandDir : ACommand
    {
        public CommandDir(IModel model, IView view) : base(model, view)
        {

        }

        public override void DoCommand(params string[] parameters)
        {
            Console.WriteLine("Showing dir of {0}", parameters[0]);
            string path = parameters[0];
            try
            {

            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public override string GetDescription()
        {
            return "Dir <path> - show all the file and folders inside the path";
        }

        public override string GetName()
        {
            return "dir";
        }
    }

    class CommandGenerate3dMaze : ACommand
    {
        public CommandGenerate3dMaze(IModel model, IView view) : base(model, view)
        {
        }

        public override void DoCommand(params string[] parameters)
        {
            throw new NotImplementedException();
        }

        public override string GetDescription()
        {
            return "generate 3d maze <maze name> <dimensions(x y z)> \n generate new maze with the name and the dimensions that you enter. \n if exist a maze with a same name the new maze will override it";
        }

        public override string GetName()
        {
            return "generate3dMaze";
        }
    }

    class CommandDisplay : ACommand
    {
        public CommandDisplay(IModel model, IView view) : base(model, view)
        {
        }

        public override void DoCommand(params string[] parameters)
        {
            throw new NotImplementedException();
        }

        public override string GetDescription()
        {
            return "display <maze name> - will print the maze";
        }

        public override string GetName()
        {
            return "display";
        }
    }

    class CommandDisplayCrossSectionBy : ACommand
    {
        public CommandDisplayCrossSectionBy(IModel model, IView view) : base(model, view)
        {
        }

        public override void DoCommand(params string[] parameters)
        {
        }

        public override string GetDescription()
        {
            return "";
        }

        public override string GetName()
        {
            return "displayCrossSectionBy";
        }
    }

    class CommandSaveMaze : ACommand
    {
        public CommandSaveMaze(IModel model, IView view) : base(model, view)
        {
        }

        public override void DoCommand(params string[] parameters)
        {
            throw new NotImplementedException();
        }

        public override string GetDescription()
        {
            return "save maze <maze name> <file path> - save the maze with a given name to a given file path(full path)";
        }

        public override string GetName()
        {
            return "saveMaze";
        }
    }

    class CommandLoadMaze : ACommand
    {
        public CommandLoadMaze(IModel model, IView view) : base(model, view)
        {
        }

        public override void DoCommand(params string[] parameters)
        {
            throw new NotImplementedException();
        }

        public override string GetDescription()
        {
            return "load maze <file path> <maze name> - load the maze from the path";
        }

        public override string GetName()
        {
            return "loadMaze";
        }
    }

    class CommandMazeSize : ACommand
    {
        public CommandMazeSize(IModel model, IView view) : base(model, view)
        {
        }

        public override void DoCommand(params string[] parameters)
        {
            throw new NotImplementedException();
        }

        public override string GetDescription()
        {
            return "maze size <maze name> - display the size of the maze in bytes";
        }

        public override string GetName()
        {
            return "mazeSize";
        }
    }

    class CommandFileSize : ACommand
    {
        public CommandFileSize(IModel model, IView view) : base(model, view)
        {
        }

        public override void DoCommand(params string[] parameters)
        {
            throw new NotImplementedException();
        }

        public override string GetDescription()
        {
            return "file size <file path> - display the size of the file in bytes";
        }

        public override string GetName()
        {
            return "fileSize";
        }
    }

    class CommandSolveMaze : ACommand
    {
        public CommandSolveMaze(IModel model, IView view) : base(model, view)
        {
        }

        public override void DoCommand(params string[] parameters)
        {

        }

        public override string GetDescription()
        {
            return "solve maze <maze name> <algorithm(BFS or DFS)> - solvethe maze with specific algorithm";
        }

        public override string GetName()
        {
            return "solveMaze";
        }
    }

    class CommandDisplaySolution : ACommand
    {
        public CommandDisplaySolution(IModel model, IView view) : base(model, view)
        {
        }

        public override void DoCommand(params string[] parameters)
        {
        }

        public override string GetDescription()
        {
            return "display solution <maze name> -display the steps of the solution in format (x,y,z)";

        }

        public override string GetName()
        {
            return "displaySolution";
        }
    }

    class CommandExit : ACommand
    {
        public CommandExit(IModel model, IView view) : base(model, view)
        {
        }

        public override void DoCommand(params string[] parameters)
        {
            Console.WriteLine("Exiting the program...\nThank you for using MazeRunner v1.0");
        }

        public override string GetDescription()
        {
            return "exit - exit the program";
        }

        public override string GetName()
        {
            return "exit";
        }
    }
}
