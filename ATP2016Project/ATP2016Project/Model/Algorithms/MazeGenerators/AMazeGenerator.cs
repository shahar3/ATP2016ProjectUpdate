using System;

namespace ATP2016Project.Model.Algorithms.MazeGenerators
{
    abstract class AMazeGenerator : IMazeGenerator
    {
        abstract public Maze generate(IMaze maze, PrimAlgorithm algo);

        public string measureAlgorithmTime(IMaze maze)
        {
            DateTime startingTime = DateTime.Now;
            //generate();
            DateTime endTime = DateTime.Now;
            TimeSpan difference = endTime - startingTime;
            string result = difference.TotalSeconds.ToString();
            return "It took " + result + "seconds";
        }
    }
}
