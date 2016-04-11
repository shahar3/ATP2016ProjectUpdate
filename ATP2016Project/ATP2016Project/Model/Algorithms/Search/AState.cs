using System;

namespace ATP2016Project.Model.Algorithms.Search
{
    abstract class AState
    {
        private string m_state;
        private double m_cost;
        private AState m_previous;

        public AState(AState parentState)
        {
            try
            {
                m_previous = parentState;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// represent the state of our problem
        /// in this case the maze
        /// </summary>
        public string State
        {
            get { return m_state; }
            set { m_state = value; }
        }

        /// <summary>
        /// get the cost of each move
        /// </summary>
        public double Cost
        {
            get { return m_cost; }
            set { m_cost = value; }
        }

        /// <summary>
        /// get the previous state
        /// </summary>
        public AState Previous
        {
            get { return m_previous; }
            set { m_previous = value; }
        }

        /// <summary>
        /// overrides the default constructor 
        /// </summary>
        /// <param name="state"></param>
        public AState(string state)
        {
            m_state = state;
        }

        /// <summary>
        /// we override the equals method of object
        /// in that way we know when we reached the goal state
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return m_state.Equals((obj as AState).m_state);
        }

        public void printState()
        {
            Console.WriteLine(m_state);
        }
    }
}
