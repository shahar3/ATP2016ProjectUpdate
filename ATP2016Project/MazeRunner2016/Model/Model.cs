using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MazeLib;
using System.IO;

namespace MazeRunner2016
{
    public class Model : IModel
    {
        public event finishedComputing ModelChanged;
        private List<string> m_mazesNames;
        private Dictionary<string, IMaze> m_mazes = new Dictionary<string, IMaze>();
        private Dictionary<Maze3d, Solution> m_mazesSolution = new Dictionary<Maze3d, Solution>();
        private Dictionary<string, string> m_mazesSolveTime = new Dictionary<string, string>();
        private Dictionary<string, int> m_mazesStatesDeveloped = new Dictionary<string, int>();

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
            string timeToSolve = string.Empty;
            if (algoName == "BFS")
            {
                //
                ISearchingAlgorithm BFS = new BreadthFirstSearch();
                m_mazesSolution[m_mazes[mazeName] as Maze3d] = BFS.search(mySearchableMaze, out timeToSolve);
                m_mazesSolveTime[mazeName] = timeToSolve;
                m_mazesStatesDeveloped[mazeName] = BFS.statesDeveloped();
                ModelChanged("solveMaze", mazeName);
            }
            else
            {
                //
                ISearchingAlgorithm DFS = new DepthFirstSearch();
                m_mazesSolution[m_mazes[mazeName] as Maze3d] = DFS.search(mySearchableMaze, out timeToSolve);
                m_mazesSolveTime[mazeName] = timeToSolve;
                m_mazesStatesDeveloped[mazeName] = DFS.statesDeveloped();
                ModelChanged("solveMaze", mazeName);
            }
        }

        public Solution getSolution(string mazeName)
        {
            return m_mazesSolution[m_mazes[mazeName] as Maze3d];
        }

        internal string getSolvedTime(string mazeName)
        {
            return m_mazesSolveTime[mazeName];
        }

        public void saveMaze(string mazeName, string mazePath)
        {
            Maze3d myMaze = m_mazes[mazeName] as Maze3d;
            try
            {
                using (FileStream fs = new FileStream(mazePath, FileMode.Create))
                {
                    using (Stream mazeStream = new MemoryStream(myMaze.toByteArray()))
                    {
                        using (Stream fileStream = new MyCompressorStream(fs, MyCompressorStream.compressionMode.compress))
                        {
                            byte[] byteArray = new byte[100];
                            int r = 0;
                            while ((r = mazeStream.Read(byteArray, 0, byteArray.Length)) != 0)
                            {
                                fileStream.Write(byteArray, 0, 100);
                                fileStream.Flush();
                                byteArray = new byte[100];
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ModelChanged("saveMaze", "errorSave");
            }
            ModelChanged("saveMaze", "The maze saved");
        }

        public int getStatesDeveloped(string mazeName)
        {
            return m_mazesStatesDeveloped[mazeName];
        }
    }
}
