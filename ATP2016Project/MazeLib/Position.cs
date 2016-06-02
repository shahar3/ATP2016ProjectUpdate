using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MazeLib
{
    #region Position
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

    #region Compressing classes
    /// <summary>
    /// this interface allow to compress and decompress array of bytes
    /// with the method compress and decompress
    /// </summary>
    interface ICompressor
    {
        byte[] compress(byte[] data);
        byte[] decompress(byte[] data);
    }

    /// <summary>
    /// This is our stream decorator class. we using the decorator design pattern
    /// to apply the naive compressor that we implemented in myMaze3Dcompressor class
    /// we override the read and write functions and call the compress and decompress
    /// methods from there
    /// </summary>
    class MyCompressorStream : Stream
    {
        private Stream m_io; //the stream that we use as input
        private const int m_bufferSize = 100;
        private byte[] m_byteReadFromStream;
        private MyMaze3Dcompressor m_mazeCompressor;
        private Queue<byte> m_queue; //stores the bytes that we add to the buffer
        private bool firstTime = true; //we change it to false after our first iteration
        private bool add = false; //help us to do the needed adjustments to our buffer 
        public enum compressionMode
        {
            compress = 1,
            decompress
        };
        private compressionMode m_mode;

        /// <summary>
        /// our constructor
        /// gets an input stream that we use and the desired compression mode
        /// </summary>
        /// <param name="io">input stream</param>
        /// <param name="mode">compression action(compress,decompress)</param>
        public MyCompressorStream(Stream io, compressionMode mode)
        {
            m_io = io;
            m_mode = mode; //the compressing action
            m_byteReadFromStream = new byte[m_bufferSize]; //stores the read bytes from the stream after the compressing action
            m_mazeCompressor = new MyMaze3Dcompressor(); //we use our naive compressor
            m_queue = new Queue<byte>();
        }

        //use the stream canRead setting
        public override bool CanRead
        {
            get
            {
                return m_io.CanRead;
            }
        }

        //use the stream canWrite setting
        public override bool CanWrite
        {
            get
            {
                return m_io.CanWrite;
            }
        }

        //use the stream Flush method
        public override void Flush()
        {
            m_io.Flush();
        }

        /// <summary>
        /// read the bytes from the original stream (m_io) and perform the compressing action on them
        /// (compress/decompress) store it in the m_queue and fill the buffer with it
        /// </summary>
        /// <param name="buffer">an empty array of bytes</param>
        /// <param name="offset">from where to start on the buffer</param>
        /// <param name="count">how many bytes to read</param>
        /// <returns>number of read bytes</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (m_mode == compressionMode.compress)
            {
                int bytesRead = 0;
                while (m_queue.Count < count && (bytesRead = m_io.Read(m_byteReadFromStream, 0, m_bufferSize)) != 0)
                {
                    //this functiom copy the data from stream to my array bytes if data
                    byte[] data = copyDataFromStream(bytesRead);
                    //compress the uncompressed data
                    byte[] compressed = m_mazeCompressor.compress(data);
                    //insert the compressed data to queue
                    insertToQueue(compressed);
                }
                int bytesCount = Math.Min(m_queue.Count, count);
                //insert the data to buffer
                insertDataToBuffer(buffer, offset, bytesCount);
                return bytesCount;
            }
            else if (m_mode == compressionMode.decompress)
            {
                int bytesRead = 0;
                while (m_queue.Count < count && (bytesRead = m_io.Read(m_byteReadFromStream, 0, m_bufferSize)) != 0)
                {
                    byte[] data = new byte[bytesRead];
                    int addition = 0;
                    if (add)
                    {
                        modifyBuffer(bytesRead, out data, out addition);
                    }
                    add = true;
                    for (int i = 0; i < bytesRead; i++)
                    {
                        data[i + addition] = m_byteReadFromStream[i];
                    }
                    byte[] decompressed = m_mazeCompressor.decompress(data);
                    //add the decompressed data to queue
                    insertToQueue(decompressed);

                }
                int bytesCount = Math.Min(m_queue.Count, count);
                //inser the data to buffer
                insertDataToBuffer(buffer, offset, bytesCount);
                firstTime = false;
                return bytesCount;
            }
            return 0;
        }

        #region helping methods for read

        /// <summary>
        /// this function expands the data array and insert in the start 0
        /// </summary>
        /// <param name="bytesRead">number of bytes read</param>
        /// <param name="data">the array of bytes,here we extands him</param>
        /// <param name="addition">change</param>
        private static void modifyBuffer(int bytesRead, out byte[] data, out int addition)
        {
            addition = 1;
            data = new byte[bytesRead + 1];
            data[0] = 0;
        }

        /// <summary>
        /// this function insert the data from queue to the buffer
        /// </summary>
        /// <param name="buffer">we copy the data from queue to him</param>
        /// <param name="offset">from where we need to insert</param>
        /// <param name="bytesCount">the number of bytes we need to copy</param>
        private void insertDataToBuffer(byte[] buffer, int offset, int bytesCount)
        {
            for (int i = 0; i < bytesCount; i++)
            {
                buffer[i + offset] = m_queue.Dequeue();
            }
        }

        /// <summary>
        /// this function use to insert the compressed data to queue
        /// </summary>
        /// <param name="compressed">array of compressed data</param>
        private void insertToQueue(byte[] compressed)
        {
            foreach (byte b in compressed)
            {
                m_queue.Enqueue(b);
            }
        }
        /// <summary>
        /// this function create a new array of bytes and copy the data from the stream
        /// </summary>
        /// <param name="bytesRead">number of read bytes</param>
        /// <returns>copy bytes array</return>
        private byte[] copyDataFromStream(int bytesRead)
        {
            byte[] data = new byte[bytesRead];
            for (int i = 0; i < bytesRead; i++)
            {
                data[i] = m_byteReadFromStream[i];
            }

            return data;
        }

        #endregion

        /// <summary>
        /// gets a full buffer and perform the compressing action on it(compress/decompress)
        /// and write the result on the stream
        /// </summary>
        /// <param name="buffer">full array of bytes</param>
        /// <param name="offset">the place we start on the buffer</param>
        /// <param name="count">number of bytes we read from the buffer</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            //we get uncompressed data and need to compress him 
            if (m_mode == compressionMode.compress)
            {
                //copy the data from the buffer we get to array of bytes
                byte[] data = copyDataFromBuffer(buffer, offset, count);
                //compress the data we et
                byte[] compressed = m_mazeCompressor.compress(data);
                //check if the final digit in the data is 0
                //if it is we expands the array and add the digit 0
                //else we dont change the array
                if (data[count - 1] == 0)
                {
                    byte[] newCompressed = addZeroToArray(compressed);
                    m_io.Write(newCompressed, 0, newCompressed.Length);
                }
                else
                {
                    m_io.Write(compressed, 0, compressed.Length);
                }
            }
            else
            {
                //we get compressed data and need to decompress him
                if (m_mode == compressionMode.decompress)
                {
                    byte[] data = copyDataFromBuffer(buffer, offset, count);
                    byte[] decompressed = m_mazeCompressor.decompress(data);
                    m_io.Write(decompressed, 0, decompressed.Length);
                }
            }

        }

        #region helping methods for write

        /// <summary>
        /// this function expands the array and add 0 to the and of him
        /// </summary>
        /// <param name="compressed">the old array of compressed data</param>
        /// <returns>new compressed data after the expands and adding</returns>
        private static byte[] addZeroToArray(byte[] compressed)
        {
            byte[] newCompressed = new byte[compressed.Length + 1];
            for (int i = 0; i < compressed.Length; i++)
            {
                newCompressed[i] = compressed[i];
            }
            newCompressed[compressed.Length] = 0;
            return newCompressed;
        }
        /// <summary>
        /// this function copy the uncompressed data from buffer to a new array of bytes
        /// </summary>
        /// <param name="buffer">handle the all data uncompressed</param>
        /// <param name="offset">from where we copy</param>
        /// <param name="count">how much bytes we copied</param>
        /// <returns>the array of bytes after the copied</returns>
        private static byte[] copyDataFromBuffer(byte[] buffer, int offset, int count)
        {
            byte[] data = new byte[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = buffer[i + offset];
            }

            return data;
        }

        #endregion

        #region non-relevant
        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }
        public override long Length
        {
            get
            {
                return -1;
            }
        }
        public override long Position
        {
            get
            {
                return -1;
            }

            set
            {
                Position = -1;
            }
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            return -1;
        }

        public override void SetLength(long value)
        {
            return;
        }
        #endregion
    }

    /// <summary>
    /// In this class we implement our naive compressor for the representation of our maze
    /// the maze represent as a byte array in the following structure:
    /// [maze settings(dimensions,starting point, goal point), maze bytes array]
    /// the setting took 9 places so we have to start compressing from the 9th cell and on
    /// 
    /// we choose the naive compressing method as follow: 
    /// we assume that we start with 0. we write the sequence of zeros than the sequence of ones, than back we write the sequence of zeros
    /// and etc..
    /// for example: before compression we get the input: 1,1,1,0,0,1,1,1 => after compression: 0,3,2,3
    /// </summary>
    class MyMaze3Dcompressor : ICompressor
    {
        private bool firstTimeCompress = true; //help us to determine if it is the first time we use the compressor
        private bool firstTimeDecompress = true;

        /// <summary>
        /// this function gets an array of uncompressed bytes array and compress them using the naive compression method
        /// as mention above in the description
        /// </summary>
        /// <param name="data">array of uncompressed bytes that represent the maze</param>
        /// <returns>compressed maze bytes array</returns>
        public byte[] compress(byte[] data)
        {
            //this list will contain the compress data
            List<byte> compressed = new List<byte>();
            //compress the settings of the maze(dimensions,start point,goal point)
            int i = 0;
            //check if it's the first iteration
            //if it is, we add the settings of the maze (first 9 places) normally
            if (firstTimeCompress)
            {
                i = enterMazeSettings(data, compressed);
            }
            int swapDigit = 0; //help us to track if the current sequnce is of zeros or ones
            while (i < data.Length)
            {
                //take a byte from the data
                byte byteFromData = data[i];
                byte count = 0;
                //checks if the current byte is the same like our swapDigit and updateds the counters accordingly
                while (i < data.Length && data[i] == swapDigit % 2 && count < byte.MaxValue)
                {
                    count++;
                    i++;
                }
                swapDigit++; //change the current digit of sequence (from 0 to 1 and from 1 to 0)
                compressed.Add(count);
            }
            firstTimeCompress = false; //to assure that next time we will not enter the first 9 cells of the bytes array without compressing
            return compressed.ToArray();
        }

        /// <summary>
        /// an helper method
        /// enter the settings of the maze to the compressed list
        /// </summary>
        /// <param name="data">the original bytes array of the maze</param>
        /// <param name="compressed">the empty compressed list</param>
        /// <returns>the compressed list with the 9 bytes of settings</returns>
        private static int enterMazeSettings(byte[] data, List<byte> compressed)
        {
            int i;
            int numOfSettings = 9;
            for (int j = 0; j < numOfSettings; j++)
            {
                compressed.Add(data[j]);
            }
            //update the i to be 9
            i = numOfSettings;
            return i;
        }

        /// <summary>
        /// gets a compressed bytes array 
        /// and decompress it to a list and returns it
        /// </summary>
        /// <param name="data">the compressed byte array</param>
        /// <returns>decompressed bytes array</returns>
        public byte[] decompress(byte[] data)
        {
            List<byte> decompressed = new List<byte>();
            //decompress the settings of the maze(dimensions,start point,goal point)
            int i = 0;
            //check if it's the first iteration
            //if it is, we add the settings of the maze (first 9 places) normally
            if (firstTimeDecompress)
            {
                i = enterMazeSettings(data, decompressed);
            }
            byte swapDigit = 0;
            while (i < data.Length)
            {
                fillDigitSequence(data, decompressed, i, swapDigit);
                i++;
                swapDigit = (byte)((swapDigit + 1) % 2); //updates 1 to 0 and 0 to 1
            }
            firstTimeDecompress = false;
            return decompressed.ToArray();
        }

        /// <summary>
        /// loop the number of sequence of the specific digit
        /// and add it to the decompressed list
        /// </summary>
        /// <param name="data">the compressed bytes array</param>
        /// <param name="decompressed">list of decompressed bytes</param>
        /// <param name="i">the location in the compressed bytes array</param>
        /// <param name="swapDigit">the desired digit to add to the decompressed list</param>
        private static void fillDigitSequence(byte[] data, List<byte> decompressed, int i, byte swapDigit)
        {
            for (int counter = data[i]; counter > 0; counter--)
            {
                decompressed.Add(swapDigit);
            }
        }
    }

    #endregion

    #region generating classes
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

    interface IMaze
    {
        void print();
    }

    interface IMazeGenerator
    {
        //generate a maze given the dimensions
        Maze generate(int x, int y, int z);
        //measure time for generating the maze
        string measureAlgorithmTime(int x, int y, int z);
    }

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
        private int[,] m_grid; //useful for printing prim's algorithm


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
                Maze2d mazeLayer = new Maze2d(m_xLength, m_yLength);
                mazeLayer.Grid = new int[m_xLength * 2 + 1, m_yLength * 2 + 1];
                m_maze2DLayers.Add(mazeLayer);
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

        /// <summary>
        /// determine how many layers (the z axis) we will have in the maze
        /// *2d maze has 1 layer
        /// </summary>
        public ArrayList Maze2DLayers
        {
            get { return m_maze2DLayers; }
            set { m_maze2DLayers = value; }
        }

        /// <summary>
        /// we use it in the 3d maze to represent the maze with the walls
        /// </summary>
        public int[,] Grid
        {
            get { return m_grid; }
            set { m_grid = value; }
        }

        /// <summary>
        /// Print the maze array (2d and 3d)
        /// we did it abstract because the 2d and 3d algorithms are different
        /// and uses a different method. (3d using a grid layout to print)
        /// </summary>
        public abstract void print();

        public Position getGoalPosition()
        {
            return this.GoalPoint;
        }

        public Position getStartPosition()
        {
            return this.StartPoint;
        }
    }

    class Maze2d : Maze
    {
        /// <summary>
        /// Use the base constructor from the maze class
        /// </summary>
        /// <param name="xLength"></param>
        /// <param name="yLength"></param>
        public Maze2d(int xLength, int yLength) : base(xLength, yLength)
        {
        }

        /// <summary>
        /// the default constructor
        /// </summary>
        public Maze2d() : base()
        {

        }

        /// <summary>
        /// the naive algorithm printing method
        /// </summary>
        public override void print()
        {
            {
                string empty = " ";
                string wall = "█";
                for (int j = 0; j < YLength; j++)
                {
                    for (int k = 0; k < XLength; k++)
                    {
                        if (new Position(k, j, 0).Equals(this.StartPoint))
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write("S");
                            Console.ResetColor();
                        }
                        else if (new Position(k, j, 0).Equals(this.GoalPoint))
                        {
                            Console.Write("E");
                        }
                        else if (this.MazeArray[k, j] == 0)
                        {
                            Console.Write(empty);
                        }
                        else
                        {
                            Console.Write(wall);
                        }
                    }
                    Console.WriteLine("");
                }
                Console.WriteLine("");
                Console.WriteLine("");
            }
        }
    }

    class Maze3d : Maze
    {

        /// <summary>
        /// Uses the base constructor for 3d maze in the maze class
        /// </summary>
        /// <param name="xLength"></param>
        /// <param name="yLength"></param>
        /// <param name="zLength"></param>
        public Maze3d(int xLength, int yLength, int zLength) : base(xLength, yLength, zLength)
        {

        }

        public Maze3d(byte[] unCompressMaze) : this(unCompressMaze[0], unCompressMaze[1], unCompressMaze[2])
        {
            Position startPoint = new Position(unCompressMaze[3], unCompressMaze[4], unCompressMaze[5]);
            Position goalPoint = new Position(unCompressMaze[6], unCompressMaze[7], unCompressMaze[8]);
            this.StartPoint = startPoint;
            this.GoalPoint = goalPoint;
            //
            int lastPosition = 9;
            for (int level = 0; level < ZLength; level++)
            {
                Maze2d maze = Maze2DLayers[level] as Maze2d;
                for (int i = 0; i < maze.Grid.GetLength(0); i++)
                {
                    for (int j = 0; j < maze.Grid.GetLength(1); j++)
                    {
                        maze.Grid[i, j] = unCompressMaze[lastPosition++];
                    }
                }
            }
        }

        /// <summary>
        /// the "smart" algorithm printing method
        /// printing s at start point and e at goal point
        /// wrapping the maze with a frame
        /// </summary>
        public override void print()
        {
            string wall = "██";
            string space = "  ";
            for (int level = 0; level < ZLength; level++)
            {
                Maze2d maze = Maze2DLayers[level] as Maze2d;
                int rowLength = maze.Grid.GetLength(0);
                int colLength = maze.Grid.GetLength(1);
                Console.WriteLine("****LEVEL {0}****", level + 1);
                for (int i = 0; i < rowLength; i++)
                {
                    for (int j = 0; j < colLength; j++)
                    {
                        if (level == 0 && i == StartPoint.X * 2 + 1 && j == StartPoint.Y * 2 + 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("S ");
                            Console.ResetColor();
                        }
                        else if (level == ZLength - 1 && i == GoalPoint.X * 2 + 1 && j == GoalPoint.Y * 2 + 1)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write("E ");
                            Console.ResetColor();
                        }
                        else if (maze.Grid[i, j] == 1) //put wall
                        {
                            Console.Write(wall);
                        }
                        else if (maze.Grid[i, j] == 0) //there is a space
                        {
                            Console.Write(space);
                        }
                        else //solution path
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.Write(space);
                            Console.ResetColor();
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        public byte[] toByteArray()
        {
            List<byte> byteArray = new List<byte>();
            //add the dimensions,start point and goal point
            //add the dimensions
            byteArray.Add((byte)(this.XLength));
            byteArray.Add((byte)(this.YLength));
            byteArray.Add((byte)(this.ZLength));
            //add the start point
            byteArray.Add((byte)this.StartPoint.X);
            byteArray.Add((byte)this.StartPoint.Y);
            byteArray.Add((byte)this.StartPoint.Z);
            //add the goal point
            byteArray.Add((byte)this.GoalPoint.X);
            byteArray.Add((byte)this.GoalPoint.Y);
            byteArray.Add((byte)this.GoalPoint.Z);
            //add the maze
            for (int i = 0; i < ZLength; i++)
            {
                Maze2d maze = this.Maze2DLayers[i] as Maze2d;
                for (int j = 0; j < maze.Grid.GetLength(0); j++)
                {
                    for (int k = 0; k < maze.Grid.GetLength(1); k++)
                    {
                        byteArray.Add((byte)maze.Grid[j, k]);
                    }
                }
            }
            return byteArray.ToArray();
        }
    }

    class MyMaze3dGenerator : AMazeGenerator
    {
        private PrimAlgorithm m_alg;
        private Maze3d myMaze;

        /// <summary>
        /// This method override the generate method from AMazeGenerator implementing
        /// it by our algorithm (prim's random MST algorithm)
        /// </summary>
        /// <param name="maze"></param>
        /// <param name="alg"></param>
        /// <returns></returns>
        public override Maze generate(int x, int y, int z)
        {
            IMaze maze = new Maze3d(x, y, z);
            myMaze = maze as Maze3d; //cast the maze to 3dMaze
            for (int i = 0; i < z; i++)
            {

                Maze2d maze2DLayer = new Maze2d(x, y);
                m_alg = new PrimAlgorithm(maze2DLayer);
                m_alg.startGenerating(); //activate the algorithm of prim to generate a random maze
                maze2DLayer.Grid = m_alg.Grid;
                myMaze.Maze2DLayers[i] = maze2DLayer;
                Thread.Sleep(10);
            }
            generateRandomGoalPoint(); //choose a random goal point
            generateRandomStartPoint(); //choose a random start point
            return myMaze;
        }

        private void generateRandomStartPoint()
        {
            Random r = new Random();
            int randomNumber = r.Next(myMaze.YLength);
            //3d start point
            myMaze.StartPoint = new Position(0, randomNumber, 0);
        }

        /// <summary>
        /// This method generate a random goal point 
        /// </summary>
        private void generateRandomGoalPoint()
        {
            int lastRow = myMaze.XLength - 1;
            int lastLvl = myMaze.ZLength - 1;
            Random r = new Random();
            int randomNumber = r.Next(myMaze.YLength);
            //3d goal point
            myMaze.GoalPoint = new Position(lastRow, randomNumber, lastLvl);
        }
    }

    class SimpleMaze2dGenerator : AMazeGenerator
    {
        private Maze2d myMaze;
        private const int PercentOfWalls = 25; //set the chance to break a wall

        /// <summary>
        /// This is the main method in this class
        /// from this method we call all the other helping methods to implement our algorithm
        /// </summary>
        /// <param name="maze"></param>
        /// <param name="algo"></param>
        /// <returns></returns>
        public override Maze generate(int x, int y, int z)
        {
            //create 2d maze ignoring the z value
            IMaze maze = new Maze2d(x, y);
            myMaze = maze as Maze2d;
            //init the maze with walls (1)
            initMazeToBeFullWithWalls();
            //set the starting point and goal point to be free (0)
            setStartPointAndGoalPoint();
            //build the goal path
            buildGoalPath();
            //surround the maze with walls randomly
            breakWallsRandomlyMaze(PercentOfWalls);
            return myMaze;
        }

        /// <summary>
        /// We iterate through all the walls and break randomly with a given chance rate the walls
        /// </summary>
        /// <param name="percent"></param>
        private void breakWallsRandomlyMaze(int percent)
        {
            for (int i = 0; i < myMaze.XLength; i++)
            {
                for (int j = 0; j < myMaze.YLength; j++)
                {
                    if (myMaze.MazeArray[i, j] == 1)
                    {
                        Thread.Sleep(5);
                        Random rand = new Random();
                        int randomNumber = rand.Next(100);
                        if (randomNumber < percent) //25% to break a wall
                        {
                            myMaze.MazeArray[i, j] = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// The wrapper method that calls the recursive method to build a path
        /// </summary>
        private void buildGoalPath()
        {
            buildPathRec(myMaze.StartPoint);
        }

        /// <summary>
        /// given a starting point, this function builds a simple path to
        /// the goal point of the maze
        /// </summary>
        /// <param name="curPoint"></param>
        private void buildPathRec(Position curPoint)
        {
            //the break statement. if the current position is a neighbour of the goal position we finish to build the path
            if (isNeighbour(curPoint))
            {
                return;
            }
            else
            {
                Random rand = new Random();
                Thread.Sleep(6); //to make it completely random
                int direction = rand.Next(3);
                int goalX = myMaze.GoalPoint.X;
                int goalY = myMaze.GoalPoint.Y;
                switch (direction)
                {
                    //up
                    case 0:
                        if (curPoint.Y < goalY)
                        {
                            break;
                        }
                        breakWall(curPoint.X, curPoint.Y - 1, out curPoint);
                        break;
                    //right
                    case 1:
                        breakWall(curPoint.X + 1, curPoint.Y, out curPoint);
                        break;
                    //down
                    case 2:
                        if (curPoint.Y > goalY)
                        {
                            break;
                        }
                        breakWall(curPoint.X, curPoint.Y + 1, out curPoint);
                        break;
                }
                buildPathRec(curPoint);
            }
        }

        /// <summary>
        /// checks a certain position, and if it possible we break the wall (putting 0)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="mazePoint"></param>
        private void breakWall(int x, int y, out Position mazePoint)
        {
            mazePoint = new Position(x, y);
            if (checkIfPossible(x, y))
            {
                myMaze.MazeArray[mazePoint.X, mazePoint.Y] = 0;
            }
        }

        private bool isNeighbour(Position curPoint)
        {
            int x = curPoint.X, y = curPoint.Y;
            int goalX = myMaze.GoalPoint.X, goalY = myMaze.GoalPoint.Y;
            if (x + 1 == goalX && y == goalY || x - 1 == goalX && y == goalY || y - 1 == goalY && x == goalX || y + 1 == goalY && x == goalX)
            {
                return true;
            }
            return false;
        }

        private bool checkIfPossible(int x, int y)
        {
            int maxX = myMaze.XLength;
            int maxY = myMaze.YLength;
            if (x >= 0 && x < maxX && y >= 0 && y < maxY)
            {
                return true;
            }
            return false;
        }

        private void setStartPointAndGoalPoint()
        {
            try
            {
                chooseStartAndGoalPoints();
                int xStart = myMaze.StartPoint.X;
                int yStart = myMaze.StartPoint.Y;
                int xGoal = myMaze.GoalPoint.X;
                int yGoal = myMaze.GoalPoint.Y;
                myMaze.MazeArray[xStart, yStart] = 2;
                myMaze.MazeArray[xGoal, yGoal] = 3;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void chooseStartAndGoalPoints()
        {
            Random rnd = new Random();
            //set start point
            myMaze.StartPoint = new Position();
            myMaze.StartPoint.X = 0;
            myMaze.StartPoint.Y = rnd.Next(0, myMaze.YLength);
            //set goal point
            myMaze.GoalPoint = new Position();
            myMaze.GoalPoint.X = myMaze.XLength - 1;
            myMaze.GoalPoint.Y = rnd.Next(1, myMaze.YLength - 1);
        }

        private void initMazeToBeFullWithWalls()
        {
            try
            {
                for (int i = 0; i < myMaze.XLength; i++)
                {
                    for (int j = 0; j < myMaze.YLength; j++)
                    {
                        myMaze.MazeArray[i, j] = 1;
                    }
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }
    }

    #endregion

    #region searching classes
    /// <summary>
    /// this is our abstract class for searching algorithms
    /// implemets the searching algorithms interface
    /// support our main function (search) alongside functions for the
    /// statistics(states developed)
    /// </summary>
    abstract class ASearchingAlgorithm : ISearchingAlgorithm
    {
        //our class members
        protected static int statesCounter = 0;
        private Queue<AState> m_openList;
        private Queue<AState> m_closeList;
        private ISearchable m_searchable;
        private List<AState> m_successors;
        private Solution m_solution;
        protected AState currentState;

        /// <summary>
        /// the defualt constructor
        /// calls the initialize function
        /// </summary>
        public ASearchingAlgorithm()
        {
            //first we initialize the lists and the solution
            initLists();
        }

        /// <summary>
        /// init our lists that relevant to our algorithms
        /// </summary>
        private void initLists()
        {
            this.OpenList = new Queue<AState>();
            this.CloseList = new Queue<AState>();
            this.Successors = new List<AState>();
            this.Solution = new Solution();
        }

        /// <summary>
        /// our property to solution
        /// </summary>
        public Solution Solution
        {
            get { return m_solution; }
            set { m_solution = value; }
        }


        /// <summary>
        /// the successors property
        /// we chose to keep them in a list
        /// </summary>
        public List<AState> Successors
        {
            get { return m_successors; }
            set { m_successors = value; }
        }

        /// <summary>
        /// the searchable property
        /// we get an object that inherit from the interface ISearchable
        /// </summary>
        public ISearchable Searchable
        {
            get { return m_searchable; }
            set { m_searchable = value; }
        }


        /// <summary>
        /// the closeList property
        /// we keep all the states that are done in this queue
        /// </summary>
        public Queue<AState> CloseList
        {
            get { return m_closeList; }
            set { m_closeList = value; }
        }

        /// <summary>
        /// the openList property
        /// we keep all the states that we have to use in this queue
        /// </summary>
        public Queue<AState> OpenList
        {
            get { return m_openList; }
            set { m_openList = value; }
        }

        /// <summary>
        /// the main method for any searching algorithm
        /// each algorithm implement it different and hence we keep it abstract
        /// </summary>
        /// <param name="searchable"></param>
        /// <returns>the solution to the maze</returns>
        public abstract Solution search(ISearchable searchable);

        /// <summary>
        /// usefull for the statistics, 
        /// in that way we can determine which algorithm is more efficient
        /// </summary>
        /// <returns>number of states developed</returns>
        public int statesDeveloped()
        {
            return statesCounter;
        }

        /// <summary>
        /// also a good function to determine which algorithm is better
        /// </summary>
        /// <param name="searchable"></param>
        /// <returns>time elapsed since the algorithm start to solve the problem till the end</returns>
        public string timeToSolve(ISearchable searchable)
        {
            searchable.initializeGrid();
            DateTime startingTime = DateTime.Now;
            search(searchable);
            DateTime endTime = DateTime.Now;
            TimeSpan difference = endTime - startingTime;
            string result = difference.TotalSeconds.ToString();
            return "It took " + result + " seconds to solve";
        }

        /// <summary>
        /// this function is essential for giving the user the correct solution
        /// </summary>
        private void backtraceSolution()
        {
            //iterate untill we get to the start point
            while (currentState.Previous != null)
            {
                //markInGrid();
                //add the state to the solution
                this.Solution.addState(currentState);
                currentState = currentState.Previous;
            }
        }
    }

    /// <summary>
    /// this class represents the general form of each move in our problem
    /// it has the varibales of state (string represention), 
    /// cost - how much each move cost us
    /// and previous - the previous state to our current state
    /// </summary>
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

        /// <summary>
        /// printing the current state
        /// for debugging porpuse
        /// </summary>
        public void printState()
        {
            Console.WriteLine(m_state);
        }
    }

    class BreadthFirstSearch : ASearchingAlgorithm
    {
        /// <summary>
        /// this is our implementation for generic BFS
        /// we follow the psuedo algorithm:
        /// 1 - add the initial state to the open list
        /// 2 - while open list isn't empty do
        /// 2.1 - get the next state from the open list
        /// 2.2 - if the state is equal to the goal state we backtrace the solution and finish the algorithm
        /// 2.3 - else - find all of his neighbours and add the relevants to the open list
        /// 2.4 - add the current state to the close list
        /// 3 - return false if the program reach this point
        /// </summary>
        public BreadthFirstSearch() : base()
        {
        }

        /// <summary>
        /// the main function
        /// from here we implement our algorithm. get as parameter an object
        /// that inherit from the interface ISearchable
        /// </summary>
        /// <param name="searchable"></param>
        /// <returns>the solution to the searchable problem</returns>
        public override Solution search(ISearchable searchable)
        {
            searchable.initializeGrid(); //init the grid
            Console.WriteLine("start from {0} and need to get to {1}", searchable.getInitialState().State, searchable.getGoalState().State);
            this.Searchable = searchable;
            //add the first state to the open list
            addInitialState();
            //run the algorithm until we dont have anything in the open list
            while (this.OpenList.Count != 0)
            {
                statesCounter++;
                //get the next state from the open list
                currentState = this.OpenList.Dequeue();
                //check if the current state is the goal state
                //if it is, we finish the algorithm otherwise we continue
                if (currentState.Equals(this.Searchable.getGoalState()))
                {
                    //we finished and now we backtrace the solution
                    backtraceSolution();
                    this.Solution.ReverseSolution();
                    return this.Solution;
                }
                //find all the successors states
                this.Successors = this.Searchable.getAllPossibleStates(currentState);
                //add the current state to the close list
                CloseList.Enqueue(currentState);
                foreach (AState successor in this.Successors)
                {
                    if (!this.OpenList.Contains(successor) && !this.CloseList.Contains(successor))
                    {
                        //successor.Previous = currentState;
                        OpenList.Enqueue(successor);
                    }
                }
            }
            //if there is no solution
            return null;
        }

        /// <summary>
        /// backtracing the solution
        /// starting from the last state (the goal state) and iterating backwards
        /// </summary>
        private void backtraceSolution()
        {
            //iterate untill we get to the start point
            while (currentState.Previous != null)
            {
                //add the state to the solution
                this.Solution.addState(currentState);
                currentState = currentState.Previous;
            }
        }

        /// <summary>
        /// Add the initial state to be the first in the open list
        /// </summary>
        private void addInitialState()
        {
            this.OpenList.Enqueue(this.Searchable.getInitialState());
        }
    }

    class DepthFirstSearch : ASearchingAlgorithm
    {
        /// <summary>
        /// this is our implemntion for DFS algorithm
        /// we follow the psuedo algorithm:
        /// 1-add the initial state to the open list
        /// 2-while the open list is no empty do:
        /// 2.1-if the current state is the goal state we finish
        /// 2.2- get all the sucsessors have on current state
        /// 3.2- choose random sucsessor and insert to open list 
        /// </summary>
        private Random rnd = new Random();
        private Dictionary<AState, List<AState>> statesByPosition;

        public DepthFirstSearch() : base()
        {
            statesByPosition = new Dictionary<AState, List<AState>>();
        }

        /// <summary>
        ///  the main function
        /// from here we implement our algorithm. get as parameter an object
        /// that inherit from the interface ISearchable
        /// </summary>
        /// <param name="searchable"></param>
        /// <returns></returns>
        public override Solution search(ISearchable searchable)
        {
            searchable.initializeGrid(); //init the grid
            Console.WriteLine("start from {0} and need to get to {1}", searchable.getInitialState().State, searchable.getGoalState().State);
            this.Searchable = searchable;
            //add the first state to the open list
            addInitialState();
            //run the algorithm until we dont have anything in the open list
            while (this.OpenList.Count != 0)
            {
                statesCounter++;
                //get the next state from the open list
                currentState = this.OpenList.Dequeue();
                //check if the current state is the goal state
                //if it is, we finish the algorithm otherwise we continue
                if (currentState.Equals(this.Searchable.getGoalState()))
                {
                    //we finished and now we backtrace the solution
                    backtraceSolution();
                    this.Solution.ReverseSolution();
                    return this.Solution;
                }
                //check if we visited this state before
                if (!statesByPosition.ContainsKey(currentState))
                {
                    //find all the successors states
                    statesByPosition.Add(currentState, this.Searchable.getAllPossibleStates(currentState));
                }
                //filter the close list states
                filterCloseListSuccessors();
                //check if there are neighbours exists
                while (statesByPosition[currentState].Count == 0)
                {
                    CloseList.Enqueue(currentState);
                    currentState = currentState.Previous;
                }
                //add the current state to the close list
                //CloseList.Enqueue(currentState);
                //choose random successor
                currentState = randomSuccsessor();
                //add the random state to the open list
                OpenList.Enqueue(currentState);
            }
            return null;
        }

        /// <summary>
        /// check from all the successors which one is already in the close list
        /// </summary>
        private void filterCloseListSuccessors()
        {
            List<AState> statesToRemove = new List<AState>();
            foreach (AState state in statesByPosition[currentState])
            {
                if (CloseList.Contains(state) || currentState.Previous != null && state.State == currentState.Previous.State)
                {
                    statesToRemove.Add(state);
                }
            }
            foreach (AState state in statesToRemove)
            {
                statesByPosition[currentState].Remove(state);
            }
        }
        /// <summary>
        /// this is function that find random sucsessor
        /// </summary>
        /// <returns></returns>
        private AState randomSuccsessor()
        {
            int length = statesByPosition[currentState].Count;
            int randomStateNum = rnd.Next(length);
            AState randomState = statesByPosition[currentState][randomStateNum];
            statesByPosition[currentState].Remove(randomState);
            return randomState;
        }
        /// <summary>
        /// in this function we build the solution 
        /// we start from the current state(here this the goal state) and add
        /// the state to the solution and go to the previous state until we arrive to start state
        /// after this function we reverse this solution and get the real solution
        /// </summary>
        private void backtraceSolution()
        {
            //iterate untill we get to the start point
            while (currentState.Previous != null)
            {
                //add the state to the solution
                this.Solution.addState(currentState);
                currentState = currentState.Previous;
            }
        }

        /// <summary>
        /// Add the initial state to be the first in the open list
        /// </summary>
        private void addInitialState()
        {
            this.OpenList.Enqueue(this.Searchable.getInitialState());
        }
    }

    /// <summary>
    /// the general interface to help us to perform object adaption
    /// that way we can translate our specific problem to a general form of problem
    /// </summary>
    interface ISearchable
    {
        /// <summary>
        /// this function help us to get the initial state of the problem.
        /// the starting point of our game
        /// </summary>
        /// <returns>the initial state</returns>
        AState getInitialState();
        /// <summary>
        /// this function help us to get the goal state of the problem
        /// that way we can now when we reached our goal position to stop
        /// the searching algorithm
        /// </summary>
        /// <returns>the goal state</returns>
        AState getGoalState();
        /// <summary>
        /// this function find all the "neighbours" - the states that can be reached
        /// within one move from the current state
        /// </summary>
        /// <param name="state"></param>
        /// <returns>all the possible solutions we can reach from the current state</returns>
        List<AState> getAllPossibleStates(AState state);
        void initializeGrid();
    }

    /// <summary>
    /// the basic interface for any searching algorithm
    /// includes the main function search that solve the searching problem
    /// and 2 helping methods to determine the efficiency level of each
    /// algorithm
    interface ISearchingAlgorithm
    {
        /// <summary>
        /// in this function we implement the search algorithm on a general searching problem
        /// </summary>
        /// <param name="searchable">can vary through many different searching problem</param>
        /// <returns>the solution for the problem</returns>
        Solution search(ISearchable searchable);
        /// <summary>
        /// help us to determine if the algorithm is effective compared to other algorithms
        /// </summary>
        /// <returns>how many states was developed during the search process</returns>
        int statesDeveloped();
        /// <summary>
        /// help us to determine how good is the algorithm time-wise.
        /// </summary>
        /// <param name="searchable"></param>
        /// <returns>the time elapsed since we start to solve the problem untill it solved</returns>
        String timeToSolve(ISearchable searchable);
    }

    /// <summary>
    /// this class is used as the object adapter for the abstract class state
    /// we translate the functions of state to our maze problem
    /// </summary>
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

    /// <summary>
    /// this class is the object adapter for our maze problem
    /// the class inherit the ISearchable interface and implement the
    /// functions from there. that way we can use our searching algorithms 
    /// to solve our specific maze problem
    /// </summary>
    class SearchableMaze3d : ISearchable
    {
        private Maze m_maze; //we keep our maze in this variable
        private List<AState> m_successors; //list of all the neighbours of the state



        /// <summary>
        /// the default constructor
        /// cast the maze to the abstract class Maze and store it in our member variable
        /// </summary>
        /// <param name="maze"></param>
        public SearchableMaze3d(IMaze maze)
        {
            m_maze = maze as Maze;
        }

        /// <summary>
        /// the maze property
        /// this way we can access our maze from different classes
        /// </summary>
        public Maze MyMaze
        {
            get { return m_maze; }
            set { m_maze = value; }
        }

        /// <summary>
        /// this function finds all the neighbours of a given state
        /// and store the valid ones in a list
        /// </summary>
        /// <param name="state"></param>
        /// <returns>list of valid states</returns>
        public List<AState> getAllPossibleStates(AState state)
        {
            //get the current position from the MazeState
            Position currentPosition = (state as MazeState).Position;
            m_successors = new List<AState>(); //init the successors list 
            //up position
            Position upPosition = new Position(currentPosition.X - 2, currentPosition.Y, currentPosition.Z);
            if (checkIfValid(upPosition) && !checkIfThereIsWall(upPosition, currentPosition))
            {
                AState stateToAdd = new MazeState(upPosition, state);
                m_successors.Add(stateToAdd);
            }
            //down position
            Position downPosition = new Position(currentPosition.X + 2, currentPosition.Y, currentPosition.Z);
            if (checkIfValid(downPosition) && !checkIfThereIsWall(downPosition, currentPosition))
            {
                AState stateToAdd = new MazeState(downPosition, state);
                m_successors.Add(stateToAdd);
            }
            //left position
            Position leftPosition = new Position(currentPosition.X, currentPosition.Y - 2, currentPosition.Z);
            if (checkIfValid(leftPosition) && !checkIfThereIsWall(leftPosition, currentPosition))
            {
                AState stateToAdd = new MazeState(leftPosition, state);
                m_successors.Add(stateToAdd);
            }
            //right position
            Position rightPosition = new Position(currentPosition.X, currentPosition.Y + 2, currentPosition.Z);
            if (checkIfValid(rightPosition) && !checkIfThereIsWall(rightPosition, currentPosition))
            {
                AState stateToAdd = new MazeState(rightPosition, state);
                m_successors.Add(stateToAdd);
            }
            //above position (z+1)
            Position abovePosition = new Position(currentPosition.X, currentPosition.Y, currentPosition.Z + 1);
            if (checkIfValid(abovePosition) && !checkIfThereIsWall(abovePosition))
            {
                AState stateToAdd = new MazeState(abovePosition, state);
                m_successors.Add(stateToAdd);
            }
            return m_successors;

        }

        /// <summary>
        /// check if the wall between 2 adjacent cells is broken
        /// </summary>
        /// <param name="newPos"></param>
        /// <param name="curPos"></param>
        /// <returns></returns>
        private bool checkIfThereIsWall(Position newPos, Position curPos)
        {
            if (newPos.X == curPos.X) //they are horizional
            {
                if (newPos.Y < curPos.Y)
                {
                    Position wallPosToCheck = new Position(newPos.X, newPos.Y + 1, newPos.Z);
                    return checkIfThereIsWall(wallPosToCheck);
                }
                else
                {
                    Position wallPosToCheck = new Position(curPos.X, curPos.Y + 1, newPos.Z);
                    return checkIfThereIsWall(wallPosToCheck);
                }
            }
            else //they are vertical
            {
                if (newPos.X < curPos.X)
                {
                    Position wallPosToCheck = new Position(newPos.X + 1, newPos.Y, newPos.Z);
                    return checkIfThereIsWall(wallPosToCheck);
                }
                else
                {
                    Position wallPosToCheck = new Position(curPos.X + 1, curPos.Y, newPos.Z);
                    return checkIfThereIsWall(wallPosToCheck);
                }
            }
        }

        /// <summary>
        /// this function check it the given position is a wall or not.
        /// that way we can know if the position is valid or not
        /// </summary>
        /// <param name="newPosition"></param>
        /// <returns></returns>
        private bool checkIfThereIsWall(Position newPosition)
        {
            int level = newPosition.Z;
            return (m_maze.Maze2DLayers[level] as Maze).Grid[newPosition.X, newPosition.Y] == 1;
        }

        /// <summary>
        /// this function determine if a given position is inside the maze boundaries
        /// </summary>
        /// <param name="posToCheck"></param>
        /// <returns>if the position is inside or not</returns>
        private bool checkIfValid(Position posToCheck)
        {
            bool inside = posToCheck.Z < m_maze.ZLength;
            return posToCheck.X > 0 && posToCheck.X < m_maze.XLength * 2 + 1 && posToCheck.Y > 0 && posToCheck.Y < m_maze.YLength * 2 + 1 && inside;
        }

        /// <summary>
        /// this function returns the goal state of the maze. that
        /// way we can check when we reached our goal point
        /// </summary>
        /// <returns></returns>
        public AState getGoalState()
        {
            AState goalState = new MazeState(new Position(m_maze.GoalPoint.X * 2 + 1, m_maze.GoalPoint.Y * 2 + 1, m_maze.GoalPoint.Z), null);
            return goalState;
        }

        /// <summary>
        /// this function return the starting point of our maze
        /// </summary>
        /// <returns>the starting point of the maze</returns>
        public AState getInitialState()
        {
            AState initialState = new MazeState(new Position(m_maze.StartPoint.X * 2 + 1, m_maze.StartPoint.Y * 2 + 1), null);
            return initialState;
        }

        /// <summary>
        /// this function help the user to see the path of the solution in a more visual way
        /// </summary>
        /// <param name="currentState"></param>
        private void markInGrid(MazeState currentState)
        {
            Position position = (currentState as MazeState).Position;
            (this.MyMaze.Maze2DLayers[position.Z] as Maze).Grid[position.X, position.Y] = 2;
        }

        /// <summary>
        /// this is the function that color the trace of the solution path
        /// help the user to see the solution more clearly
        /// </summary>
        /// <param name="solution"></param>
        public void markSolutionInGrid(Solution solution)
        {
            foreach (MazeState state in solution.getSolutionPath())
            {
                if (state.Previous != null)
                {
                    bool upOneLevel = !((state.Previous as MazeState).Position.Z == state.Position.Z);//check if we went to the upper level
                    if (!upOneLevel)
                        markInGrid(state, state.Previous);
                }
                markInGrid(state);
            }
        }

        /// <summary>
        /// paint also the wall between 2 cells
        /// </summary>
        /// <param name="state"></param>
        /// <param name="previous"></param>
        private void markInGrid(MazeState curState, AState prevState)
        {
            if (curState.Position.X == (prevState as MazeState).Position.X) //horizional wall
            {
                if (curState.Position.Y < (prevState as MazeState).Position.Y)
                {
                    Position posToPaint = new Position(curState.Position.X, curState.Position.Y + 1, curState.Position.Z);
                    (this.MyMaze.Maze2DLayers[posToPaint.Z] as Maze).Grid[posToPaint.X, posToPaint.Y] = 2;
                }
                else
                {
                    Position posToPaint = new Position(curState.Position.X, curState.Position.Y - 1, curState.Position.Z);
                    (this.MyMaze.Maze2DLayers[posToPaint.Z] as Maze).Grid[posToPaint.X, posToPaint.Y] = 2;
                }
            }
            else //vertical wall
            {
                if (curState.Position.X < (prevState as MazeState).Position.X)
                {
                    Position posToPaint = new Position(curState.Position.X + 1, curState.Position.Y, curState.Position.Z);
                    (this.MyMaze.Maze2DLayers[posToPaint.Z] as Maze).Grid[posToPaint.X, posToPaint.Y] = 2;
                }
                else
                {
                    Position posToPaint = new Position(curState.Position.X - 1, curState.Position.Y, curState.Position.Z);
                    (this.MyMaze.Maze2DLayers[posToPaint.Z] as Maze).Grid[posToPaint.X, posToPaint.Y] = 2;
                }
            }
        }

        /// <summary>
        /// remove all the 2's - the solution path
        /// and restoring the default maze
        /// </summary>
        public void initializeGrid()
        {
            foreach (Maze mazeLayer in MyMaze.Maze2DLayers)
            {
                for (int i = 0; i < mazeLayer.Grid.GetLength(0); i++)
                {
                    for (int j = 0; j < mazeLayer.Grid.GetLength(1); j++)
                    {
                        if (mazeLayer.Grid[i, j] == 2)
                        {
                            mazeLayer.Grid[i, j] = 0;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// this class represent our solution to the searching problems
    /// the solution is build from a sequence of states that together compose the
    /// required solution. this solution is not the only solution. there can be
    /// many different and correct solutions
    /// we support the basic functions of add state to the solution, print the solution
    /// and check if a solution to the problem exists.
    /// </summary>
    class Solution
    {
        List<AState> m_pathOfSolution; //keep the states of the solution

        /// <summary>
        /// the default constructor 
        /// init the list of the path of the solution
        /// </summary>
        public Solution()
        {
            m_pathOfSolution = new List<AState>();
        }

        /// <summary>
        /// add the state to the solution's path
        /// </summary>
        /// <param name="state">the state that we want to add to the solution</param>
        public void addState(AState state)
        {
            m_pathOfSolution.Add(state);
        }

        /// <summary>
        /// check if there is solution
        /// if there are no states in the solution's path list, there
        /// is no valid solution to our problem.
        /// </summary>
        /// <returns></returns>
        public bool isSolutionExist()
        {
            return m_pathOfSolution.Count > 0;
        }

        /// <summary>
        /// get the list of state of our solution
        /// </summary>
        /// <returns></returns>
        public List<AState> getSolutionPath()
        {
            return m_pathOfSolution;
        }

        /// <summary>
        /// get the number of steps of the solution
        /// help us to see if the algorithm if efficient
        /// </summary>
        /// <returns></returns>
        public int getSolutionNumberOfSteps()
        {
            return m_pathOfSolution.Count;
        }

        /// <summary>
        /// this function is crucial to our users of the program
        /// because the list store the states from the last one to the first state
        /// we need to reverse it.
        /// </summary>
        public void ReverseSolution()
        {
            m_pathOfSolution.Reverse();
        }

        /// <summary>
        /// show the solution state by state
        /// </summary>
        public void printSolution()
        {
            foreach (AState state in m_pathOfSolution)
            {
                Console.WriteLine(state.State);
            }
        }
    }

    #endregion

    #region prim algorithm
    /// <summary>
    /// Implementation of prim algorithm (modified version)
    /// we followed the steps:
    /// 1-choose a random point in the maze
    /// 2-add the point to close list
    /// 3-while the close list doesn't have all the points of the maze do:
    /// 3.1-find all the neighbors of the points in the close list
    /// 3.2-choose one randomly and "break the wall" between him and the most adjacent point in the close list
    /// 3.3-in the case there are more than one adjacent point, choose one randomly
    /// 3.4-add the neighbour you chose to the close list
    /// 4-return the grid as the generated maze
    /// </summary>
    class PrimAlgorithm
    {
        private Maze myMaze;
        private ArrayList m_closePositions, m_neighbours;
        private int[,] grid;
        private Random r = new Random();

        /// <summary>
        /// Our prim constructor
        /// call the initialize function that translate the maze to fit our algorithm
        /// </summary>
        /// <param name="maze"></param>
        public PrimAlgorithm(IMaze maze)
        {
            myMaze = maze as Maze;
            grid = new int[myMaze.XLength * 2 + 1, myMaze.YLength * 2 + 1];
            m_closePositions = new ArrayList();
            m_neighbours = new ArrayList();
            initTheMazeGridToBeEmpty(); //mark the cells with 1's
        }

        /// <summary>
        /// initialize the maze to fit our algorithm
        /// </summary>
        private void initTheMazeGridToBeEmpty()
        {
            int rows = grid.GetLength(0);
            int cols = grid.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (i % 2 == 0)
                    {
                        grid[i, j] = 1;
                    }
                    if (i % 2 == 1 && j % 2 == 0)
                    {
                        grid[i, j] = 1;
                    }
                }
            }
        }

        /// <summary>
        /// generate the maze randomly
        /// </summary>
        public void startGenerating()
        {
            int numberOfPositions = myMaze.XLength * myMaze.YLength;
            chooseRandomStartPosition(); //choose a random start point
            addPointToTheMaze(myMaze.StartPoint); //add the start point to the maze
            Position nextPos = new Position();
            while (m_closePositions.Count < numberOfPositions)
            {
                findNeighbours(); //find all the neighbours of this cell
                nextPos = chooseOneNeighbourRandomly();
                addPointToTheMaze(nextPos);
                breakWallBetweenCells(nextPos);
            }
        }

        /// <summary>
        /// add a given point to the maze by marking it with 1
        /// </summary>
        /// <param name="posToAdd"></param>
        private void addPointToTheMaze(Position posToAdd)
        {
            //grid[posToAdd.X, posToAdd.Y] = 1;
            m_closePositions.Add(posToAdd);
        }

        private void breakWallBetweenCells(Position curPos)
        {
            Position adjacent = findAdjacentCell(curPos);
            //we mark the cell with 1 to indicate a broken wall
            calculatePositionOfTheWall(curPos, adjacent);
        }

        private void calculatePositionOfTheWall(Position curPos, Position adjacent)
        {
            //they are in the same row
            if (curPos.X == adjacent.X)
            {
                //we use the formula minY*2+1 = the place to break
                int placeToBreak = Math.Min(curPos.Y, adjacent.Y) * 2 + 2;
                //break
                grid[curPos.X * 2 + 1, placeToBreak] = 0;
            }
            //they are in the same column
            else
            {
                int placeToBreak = Math.Min(curPos.X, adjacent.X) * 2 + 2;
                //break
                grid[placeToBreak, curPos.Y * 2 + 1] = 0;
            }
        }

        private Position findAdjacentCell(Position curPos)
        {
            Position positionReturn = new Position();
            ArrayList neighboursInCloseList = new ArrayList();
            foreach (Position pos in m_closePositions)
            {
                if (curPos.isNeighbour(pos))
                {
                    neighboursInCloseList.Add(pos);
                }
            }
            return chooseNeighbourToBreakWallWithRandomly(neighboursInCloseList);
        }

        private Position chooseNeighbourToBreakWallWithRandomly(ArrayList neighboursInCloseList)
        {
            int numOfNeighboursToChooseFrom = neighboursInCloseList.Count;
            int randomNumber = r.Next(numOfNeighboursToChooseFrom);
            return neighboursInCloseList[randomNumber] as Position;
        }

        private void findNeighbours()
        {
            int closePositionsSize = m_closePositions.Count - 1;
            Position currentPositin = m_closePositions[closePositionsSize] as Position;
            //find right neighbour and add to arrayList
            Position rightPosition = new Position(currentPositin.X, currentPositin.Y + 1, currentPositin.Z);
            //check if this point eligible
            isEligible(rightPosition);
            //find left neighbour and add to arrayList
            Position leftPosition = new Position(currentPositin.X, currentPositin.Y - 1, currentPositin.Z);
            isEligible(leftPosition);
            //find above neighbour and add to araayList
            Position abovePosition = new Position(currentPositin.X - 1, currentPositin.Y, currentPositin.Z);
            isEligible(abovePosition);
            //find below neighbour and add to arrayList
            Position belowPositin = new Position(currentPositin.X + 1, currentPositin.Y, currentPositin.Z);
            isEligible(belowPositin);
        }

        private void isEligible(Position posToCheck)
        {
            if ((!ClosePositions.Contains(posToCheck)) && (!m_neighbours.Contains(posToCheck)) && isInsideTheMaze(posToCheck))
            {
                m_neighbours.Add(posToCheck);
            }
        }



        private bool isInsideTheMaze(Position posToCheck)
        {
            if (posToCheck.X < 0 || posToCheck.X >= myMaze.XLength || posToCheck.Y < 0 || posToCheck.Y >= myMaze.YLength)
            {
                return false;
            }
            return true;
        }

        private Position chooseOneNeighbourRandomly()
        {
            int numOfNeighbours = m_neighbours.Count;
            //Thread.Sleep(1);
            int randomNeigbourNum = r.Next(numOfNeighbours);
            Position neighbour = m_neighbours[randomNeigbourNum] as Position;
            m_neighbours.Remove(neighbour);
            return neighbour;
        }

        private void chooseRandomStartPosition()
        {
            myMaze.StartPoint = new Position();
            Random r = new Random();
            int row = 0;
            int col = r.Next(myMaze.YLength);
            myMaze.StartPoint.X = row;
            myMaze.StartPoint.Y = col;
        }

        public ArrayList ClosePositions
        {
            get { return m_closePositions; }
            set { m_closePositions = value; }
        }

        public int[,] Grid
        {
            get { return grid; }
            set { grid = value; }
        }
    }

    #endregion

    #endregion
}





