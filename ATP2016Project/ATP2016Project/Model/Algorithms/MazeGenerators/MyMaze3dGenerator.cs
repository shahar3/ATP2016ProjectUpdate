using System;

namespace ATP2016Project.Model.Algorithms.MazeGenerators
{
    class MyMaze3dGenerator : AMazeGenerator
    {
        private PrimAlgorithm m_alg;
        private Maze3d myMaze;
        public override Maze generate(IMaze maze, PrimAlgorithm alg)
        {
            m_alg = alg;
            myMaze = maze as Maze3d;
            alg.startGenerating();
            myMaze.Grid = alg.Grid;
            generateRandomGoalPoint();
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
