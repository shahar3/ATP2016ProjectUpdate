namespace ATP2016Project.Model.Algorithms.MazeGenerators
{
    interface IMazeGenerator
    {
        //generate a maze given the dimensions
        Maze generate(int x, int y, int z);
        //measure time for generating the maze
        string measureAlgorithmTime(int x, int y, int z);
    }
}
