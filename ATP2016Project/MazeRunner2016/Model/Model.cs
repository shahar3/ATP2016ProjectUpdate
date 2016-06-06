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
        private List<string> m_mazesNames;
        private Dictionary<string, IMaze> m_mazes = new Dictionary<string, IMaze>();
        private Dictionary<Maze3d, Solution> m_mazesSolution = new Dictionary<Maze3d, Solution>();

        public void generateMaze(int x, int y, int z, string mazeName)
        {
            MazeLib.IMazeGenerator generateMaze = new MyMaze3dGenerator();
            m_mazes[mazeName] = generateMaze.generate(x, y, z);

        }

        public void activateEvent(string commandName, string otherInformation)
        {
            ModelChanged(commandName, otherInformation);
        }

        public void prepareMazesNames()
        {
            m_mazesNames = new List<string>();
            m_mazesNames.AddRange(m_mazes.Keys);
            ModelChanged("getMazesNames", "done");
        }

        public string[] getMazesNames()
        {
            return m_mazesNames.ToArray();
        }

        public object getMaze(string nameOfTheMaze)
        {
            return m_mazes[nameOfTheMaze];
        }

        public void solveMaze(string mazeName, string algoName)
        {
            ISearchable mySearchableMaze = new SearchableMaze3d(m_mazes[mazeName]);
            if (algoName == "BFS")
            {
                //
                ISearchingAlgorithm BFS = new BreadthFirstSearch();
                m_mazesSolution[m_mazes[mazeName] as Maze3d] = BFS.search(mySearchableMaze);
                ModelChanged("solveMaze", mazeName);
            }
            else
            {
                //
                ISearchingAlgorithm DFS = new DepthFirstSearch();
                m_mazesSolution[m_mazes[mazeName] as Maze3d] = DFS.search(mySearchableMaze);
                ModelChanged("solveMaze", mazeName);
            }
        }

        public Solution getSolution(string mazeName)
        {
            return m_mazesSolution[m_mazes[mazeName] as Maze3d];
        }
    }
}
