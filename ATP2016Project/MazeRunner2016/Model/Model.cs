using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeLib;

namespace MazeRunner2016
{
    public class Model : IModel
    {
        public event finishedComputing ModelChanged;
        private Dictionary<string, IMaze> m_mazes = new Dictionary<string, IMaze>();

        public void generateMaze(int x, int y, int z, string mazeName)
        {
            MazeLib.IMazeGenerator generateMaze = new MyMaze3dGenerator();
            m_mazes[mazeName] = generateMaze.generate(x, y, z);

        }

        public void activateEvent(string commandName, string otherInformation)
        {
            ModelChanged(commandName, otherInformation);
        }


    }
}
