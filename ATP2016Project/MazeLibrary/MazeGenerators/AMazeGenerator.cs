using System;

namespace MazeLibrary
{
    abstract class AMazeGenerator : IMazeGenerator
    {
        abstract public Maze generate(int x, int y, int z);

        public string measureAlgorithmTime(int x, int y, int z)
        {
            DateTime startingTime = DateTime.Now;
            generate(x, y, z);
            DateTime endTime = DateTime.Now;
            TimeSpan difference = endTime - startingTime;
            string result = difference.TotalSeconds.ToString();
            return "It took " + result + "seconds";
        }
    }
}
