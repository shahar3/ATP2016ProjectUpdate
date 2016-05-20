using ATP2016Project.Controller;
using ATP2016Project.Model.Algorithms.Compression;
using ATP2016Project.Model.Algorithms.MazeGenerators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATP2016Project.Model
{
    class MyModel : IModel
    {
        private IController m_controller;
        private Dictionary<string, IMaze> m_mazes;

        public MyModel(IController controller)
        {
            m_controller = controller;
            m_mazes = new Dictionary<string, IMaze>();
        }

        public void generateMaze(int x, int y, int z, string name)
        {
            IMazeGenerator mazeGenerator = new MyMaze3dGenerator();
            m_mazes[name] = mazeGenerator.generate(x, y, z);
        }

        public IMaze getMaze(string name)
        {
            if (m_mazes.ContainsKey(name))
            {
                return m_mazes[name];
            }
            else
            {
                return null;
            }
        }

        public void saveMaze(string mazeName, string path)
        {
            string filePath = path + mazeName + ".maze";
            Maze3d myMaze = getMaze(mazeName) as Maze3d;
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
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
    }
}
