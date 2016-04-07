namespace ATP2016Project.Model.Algorithms.MazeGenerators
{
    interface IMazeGenerator
    {
        Maze generate(IMaze maze, PrimAlgorithm algo);
        string measureAlgorithmTime(IMaze maze, PrimAlgorithm algo);
    }
}
