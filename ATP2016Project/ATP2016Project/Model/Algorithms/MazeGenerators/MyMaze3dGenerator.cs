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
            return myMaze;
        }


    }
}
