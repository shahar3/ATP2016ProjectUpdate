﻿using System;

namespace ATP2016Project.Model.Algorithms.MazeGenerators
{

    ///this class create a position in maze

    class Position : IComparable
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
            m_z = 0;
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

        public int CompareTo(object obj)
        {
            Position other = obj as Position;
            if (other.X == m_x && other.Y == m_y && other.Z == m_z)
            {
                return 0;
            }
            else
            {
                return -1;
            }

        }

        public override bool Equals(Object otherObj)
        {
            Position other = otherObj as Position;
            if (other.X == m_x && other.Y == m_y && other.Z == m_z)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
