namespace ATP2016Project.Model.Algorithms.MazeGenerators
{

    ///this class create a position in maze

    class Position
    {
        private int m_x;
        private int m_y;
        private int m_z;

        /// <summary>
        /// the default constructor , we initialize the default position to (0,0,0) 
        /// </summary>
        public Position()
        {
            m_x = 0;
            m_y = 0;
            m_z = 0;
        }
        /// <summary>
        /// the constructur for 3d points 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Position(int x, int y, int z)
        {
            m_x = x;
            m_y = y;
            m_z = z;
        }
        /// <summary>
        /// the consructor for 2d points
        /// the z is initialized with -1
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Position(int x, int y)
        {
            m_x = x;
            m_y = y;
            m_z = -1;
        }
        /// <summary>
        /// the getters and setters
        /// </summary>
        public int X
        {
            get { return m_x; }
            set { m_x = value; }
        }

        public int Y
        {
            get { return m_y; }
            set { m_y = value; }
        }

        public int Z
        {
            get { return m_z; }
            set { m_z = value; }
        }



    }
}
