using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATP2016Project.Model.Algorithms.Compression
{
    /// <summary>
    /// this interface allow to compress and decompress array of bytes
    /// with the method compress and decompress
    /// </summary>
    interface ICompressor
    {
        /// <summary>
        /// Compress array of bytes with the naive method and returns the compress array
        /// </summary>
        /// <param name="data">array of bytes</param>
        /// <returns>compressed array of bytes</returns>
        byte[] compress(byte[] data);
        /// <summary>
        /// Decompress array of bytes with the naive method and returns the decompress array
        /// </summary>
        /// <param name="data">array of compressed bytes</param>
        /// <returns>decompress array of bytes</returns>
        byte[] decompress(byte[] data);
    }
}
