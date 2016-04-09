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
            m_alg = new PrimAlgorithm(myMaze);
            m_alg.startGenerating(); //activate the algorithm of prim to generate a random maze
            myMaze.Grid = m_alg.Grid; //this is what we print to represent the maze
            generateRandomGoalPoint(); //choose a random goal point
            return myMaze;
        }

        /// <summary>
        /// This method generate a random goal point 
        /// </summary>
        private void generateRandomGoalPoint()
        {
            int lastRow = myMaze.XLength - 1;
            Random r = new Random();
            int randomNumber = r.Next(myMaze.YLength);
            //2d goal point
            myMaze.GoalPoint = new Position(lastRow, randomNumber);
        }
    }
}
