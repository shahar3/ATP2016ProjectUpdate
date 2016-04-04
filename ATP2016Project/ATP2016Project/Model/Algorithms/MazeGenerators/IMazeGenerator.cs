namespace ATP2016Project.Model.Algorithms.MazeGenerators
{
    interface IMazeGenerator
    {
        Maze generate(IMaze maze);
        string measureAlgeorithemTime();
    }
}
