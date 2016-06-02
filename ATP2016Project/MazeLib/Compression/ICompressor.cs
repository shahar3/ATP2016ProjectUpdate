using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLib
{
    /// <summary>
    /// this interface allow to compress and decompress array of bytes
    /// with the method compress and decompress
    /// </summary>
    public interface ICompressor
    {
        byte[] compress(byte[] data);
        byte[] decompress(byte[] data);
    }
}
