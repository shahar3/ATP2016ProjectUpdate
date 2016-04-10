using ATP2016Project.Model.Algorithms.MazeGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATP2016Project.Model.Algorithms.Search
{
    class MazeState : AState
    {
        private Position m_position;

        public Position Position
        {
            get { return m_position; }
            set { m_position = value; }
        }

        public MazeState(Position currentPosition, AState parentState) : base(parentState)
        {
            try
            {
                this.State = buildMazePositionString(currentPosition);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private string buildMazePositionString(Position curPos)
        {
            m_position = curPos;
            string mazePositionString = curPos.ToString();
            return mazePositionString;
        }
    }
}
