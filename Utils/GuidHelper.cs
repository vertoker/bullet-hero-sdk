using System;
using System.Buffers.Binary;

namespace BH.SDK.Utils
{
    public static class GuidHelper
    {
        /// <summary>
        /// Creates a Guid with the given int placed in the last 4 bytes. The first 12 bytes are zero.
        /// </summary>
        public static Guid FromIntAtEnd(int value)
        {
            Span<byte> bytes = stackalloc byte[16]; // all zeros by default
            BinaryPrimitives.WriteInt32BigEndian(bytes.Slice(12, 4), value); // big-endian write
            return new Guid(bytes);
        }
        
        /// <summary>
        /// Extracts the int back from the last 4 bytes of the Guid.
        /// </summary>
        public static int ToIntFromEnd(Guid guid)
        {
            Span<byte> bytes = stackalloc byte[16];
            guid.TryWriteBytes(bytes);
            return BinaryPrimitives.ReadInt32BigEndian(bytes.Slice(12, 4));
        }
    }
}