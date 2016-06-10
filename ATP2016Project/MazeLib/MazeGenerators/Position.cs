using System;

namespace MazeLib
{

    ///this class create a position in maze

    public class Position
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
        /// the constructor for 3d points 
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
        /// the constructor for 2d points
        /// the z is initialized with -1
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Position(int x, int y)
        {
            m_x = x;
            m_y = y;
            m_z = 0;
        }

        /// <summary>
        /// Build a position from a string
        /// </summary>
        /// <param name="positionString">represent the position</param>
        public Position(string positionString)
        {
            string positionTrimmed = positionString.Trim('(', ')');
            string[] dimensions = positionTrimmed.Split(',');
            m_x = Int32.Parse(dimensions[0])*2+1;
            m_y = Int32.Parse(dimensions[1])*2+1;
            m_z = Int32.Parse(dimensions[2]);
        }
        /// <summary>
        /// the getters and setters
        /// </summary>
        public int X
        {
            get { return m_x; }
            set { m_x = value; }
        }

        internal bool isNeighbour(Position pos)
        {
            if ((m_x == pos.m_x && m_y == pos.Y + 1) || (m_x == pos.m_x && m_y == pos.Y - 1) || (m_x == pos.X + 1 && m_y == pos.Y) || (m_x == pos.X - 1 && m_y == pos.Y))
            {
                return true;
            }
            return false;
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

        /// <summary>
        /// we override the object method equals. in that way we can compare 2 different positions
        /// the program use it later in the function contain of the ArrayList
        /// </summary>
        /// <param name="otherObj"></param>
        /// <returns></returns>
        public override bool Equals(Object otherObj)
        {
            Position other = otherObj as Position;
            if (other.X == m_x && other.Y == m_y && other.Z == m_z)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// The program use it for the function contain in the ArrayList
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// overrides the toString function of object
        /// now we can print the position easily and convert it to string with ease
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "(" + m_x / 2 + "," + m_y / 2 + "," + m_z + ")";
        }

        public void print()
        {
            Console.WriteLine(this);
        }
    }
}
