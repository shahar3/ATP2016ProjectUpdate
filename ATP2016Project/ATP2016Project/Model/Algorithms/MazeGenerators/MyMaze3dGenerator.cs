namespace ATP2016Project.Model.Algorithms.MazeGenerators
{
    class MyMaze3dGenerator : AMazeGenerator
    {
        private PrimAlgorithm m_alg;
        public override Maze generate(IMaze maze, PrimAlgorithm alg)
        {
            m_alg = alg;
            return null;
        }


    }
}
