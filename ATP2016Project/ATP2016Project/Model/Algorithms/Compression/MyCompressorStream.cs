using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATP2016Project.Model.Algorithms.Compression
{
    class MyCompressorStream : Stream
    {
        private Stream m_io;
        private const int m_bufferSize = 100;
        private byte[] m_byteReadFromStream;
        private MyMaze3Dcompressor m_mazeCompressor;
        private Queue<byte> m_queue;
        private bool firstTime = true;
        private bool add = false;
        public enum compressionMode
        {
            compress = 1,
            decompress
        };
        private compressionMode m_mode;

        public MyCompressorStream(Stream io, compressionMode mode)
        {
            m_io = io;
            m_mode = mode;
            m_byteReadFromStream = new byte[m_bufferSize];
            m_mazeCompressor = new MyMaze3Dcompressor();
            m_queue = new Queue<byte>();
        }

        public override bool CanRead
        {

            get
            {
                return m_io.CanRead;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return m_io.CanWrite;
            }
        }

        public override void Flush()
        {
            m_io.Flush();
        }

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

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (m_mode == compressionMode.compress)
            {
                byte[] data = new byte[count];
                for (int i = 0; i < count; i++)
                {
                    data[i] = buffer[i + offset];
                }
                byte[] compressed = m_mazeCompressor.compress(data);
                if (data[count - 1] == 0)
                {
                    byte[] newCompressed = new byte[compressed.Length + 1];
                    for (int i = 0; i < compressed.Length; i++)
                    {
                        newCompressed[i] = compressed[i];
                    }
                    newCompressed[compressed.Length] = 0;
                    m_io.Write(newCompressed, 0, newCompressed.Length);
                }
                else
                {
                    m_io.Write(compressed, 0, compressed.Length);
                }
            }
            else
            {
                if (m_mode == compressionMode.decompress)
                {
                    byte[] data = new byte[count];
                    for (int i = 0; i < count; i++)
                    {
                        data[i] = buffer[i + offset];
                    }
                    byte[] decompressed = m_mazeCompressor.decompress(data);
                    m_io.Write(decompressed, 0, decompressed.Length);
                }
            }

        }

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
}
