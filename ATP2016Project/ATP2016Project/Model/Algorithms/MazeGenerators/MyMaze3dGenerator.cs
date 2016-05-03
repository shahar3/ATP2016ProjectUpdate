using System;

namespace ATP2016Project.Model.Algorithms.MazeGenerators
{
    class MyMaze3dGenerator : AMazeGenerator
    {
        private PrimAlgorithm m_alg;
        private Maze3d myMaze;
        /// <summary>
        /// This method override the generate method from AMazeGenerator implementing
        /// it by our algorithm (prim's random MST algorithm)
        /// </summary>
        /// <param name="maze"></param>
        /// <param name="alg"></param>
        /// <returns></returns>
        public override Maze generate(int x, int y, int z)
        {
            IMaze maze = new Maze3d(x, y, z);
            myMaze = maze as Maze3d; //cast the maze to 3dMaze
            for (int i = 0; i < z; i++)
            {
                Maze2d maze2DLayer = new Maze2d(x, y);
                m_alg = new PrimAlgorithm(maze2DLayer);
                m_alg.startGenerating(); //activate the algorithm of prim to generate a random maze
                maze2DLayer.Grid = m_alg.Grid;
                myMaze.Maze2DLayers[i] = maze2DLayer;
            }
            generateRandomGoalPoint(); //choose a random goal point
            generateRandomStartPoint(); //choose a random start point
            return myMaze;
        }

        private void generateRandomStartPoint()
        {
            Random r = new Random();
            int randomNumber = r.Next(myMaze.YLength);
            //3d start point
            myMaze.StartPoint = new Position(0, randomNumber, 0);
        }

        /// <summary>
        /// This method generate a random goal point 
        /// </summary>
        private void generateRandomGoalPoint()
        {
            int lastRow = myMaze.XLength - 1;
            int lastLvl = myMaze.ZLength - 1;
            Random r = new Random();
            int randomNumber = r.Next(myMaze.YLength);
            //3d goal point
            myMaze.GoalPoint = new Position(lastRow, randomNumber, lastLvl);
        }
    }
}
