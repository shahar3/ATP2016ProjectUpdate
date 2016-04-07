using System.Collections;

namespace ATP2016Project.Model.Algorithms.MazeGenerators
{
    /// <summary>
    /// in this class we implement the general maze structure
    /// we have several constructors for 2d and 3d maze
    /// </summary>
    abstract class Maze : IMaze
    {
        private Position m_startPoint;
        private Position m_goalPoint;
        private ArrayList m_maze2DLayers;
        private int m_xLength;
        private int m_yLength;
        private int m_zLength;
        private int[,] m_mazeArray;
        private int[,] m_grid; //usefull for printing prim's algorithm


        //default values
        private const int MINIMAL_X_LENGTH = 4;
        private const int MINIMAL_Y_LENGTH = 4;
        private const int MINIMAL_Z_LENGTH = 1; //the number of layers

        /// <summary>
        /// the default constructor initialized with default values
        /// </summary>
        public Maze()
        {
            m_startPoint = new Position(0, 0);
            m_goalPoint = new Position(MINIMAL_X_LENGTH - 1, MINIMAL_Y_LENGTH - 1);
            m_xLength = MINIMAL_X_LENGTH;
            m_yLength = MINIMAL_Y_LENGTH;
            m_mazeArray = new int[MINIMAL_X_LENGTH, MINIMAL_Y_LENGTH];
            m_maze2DLayers = new ArrayList();
            m_maze2DLayers.Add(m_mazeArray);
        }
        /// <summary>
        /// the 2d maze constructor without start point and goal point
        /// </summary>
        /// <param name="xLength"></param>
        /// <param name="yLength"></param>
        public Maze(int xLength, int yLength)
        {
            m_startPoint = null;
            m_goalPoint = null;
            m_xLength = xLength;
            m_yLength = yLength;
            m_mazeArray = new int[m_xLength, m_yLength];
            m_maze2DLayers = new ArrayList();
            m_maze2DLayers.Add(m_mazeArray);
        }
        /// <summary>
        /// the 3d maze constructor
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="goalPoint"></param>
        /// <param name="xLength"></param>
        /// <param name="yLength"></param>
        /// <param name="zLength"></param>
        public Maze(Position startPoint, Position goalPoint, int xLength, int yLength, int zLength)
        {
            m_startPoint = startPoint;
            m_goalPoint = goalPoint;
            m_xLength = xLength;
            m_yLength = yLength;
            m_zLength = zLength;
            m_maze2DLayers = new ArrayList();
            for (int i = 0; i < zLength; i++)
            {
                m_mazeArray = new int[m_xLength, m_yLength];
                m_maze2DLayers.Add(m_mazeArray);
            }
        }
        /// <summary>
        /// the 3d maze constructor without start point and goal point
        /// </summary>
        /// <param name="xLength"></param>
        /// <param name="yLength"></param>
        /// <param name="zLength"></param>
        public Maze(int xLength, int yLength, int zLength)
        {
            m_xLength = xLength;
            m_yLength = yLength;
            m_zLength = zLength;
            m_startPoint = null;
            m_goalPoint = null;
            m_maze2DLayers = new ArrayList();
            for (int i = 0; i < zLength; i++)
            {
                m_mazeArray = new int[m_xLength, m_yLength];
                m_maze2DLayers.Add(m_mazeArray);
            }
        }


        public int[,] MazeArray
        {
            get { return m_mazeArray; }
            set { m_mazeArray = value; }
        }


        /// <summary>
        /// the getters and setters
        /// </summary>
        public Position StartPoint
        {
            get { return m_startPoint; }
            set { m_startPoint = value; }
        }


        public Position GoalPoint
        {
            get { return m_goalPoint; }
            set { m_goalPoint = value; }
        }


        public int XLength
        {
            get { return m_xLength; }
            set { m_xLength = value; }
        }


        public int YLength
        {
            get { return m_yLength; }
            set { m_yLength = value; }
        }


        public int ZLength
        {
            get { return m_zLength; }
            set { m_zLength = value; }
        }


        public ArrayList Maze2DLayers
        {
            get { return m_maze2DLayers; }
            set { m_maze2DLayers = value; }
        }

        public int[,] Grid
        {
            get { return m_grid; }
            set { m_grid = value; }
        }

        /// <summary>
        /// Print the maze array (2d and 3d)
        /// </summary>
        public abstract void print();
    }
}
