using System;
using System.IO;

namespace TapeDriveStream
{
    public abstract class TapeDrive : IDisposable
    {
        public Stream UnderlyingStream { get; }

        public TapeDrive(Stream stream)
        {
            if (!stream.CanRead)
            {
                throw new InvalidOperationException(
                    "Unreadable stream provided.");
            }

            if (!stream.CanWrite)
            {
                throw new InvalidOperationException(
                    "Unwritable stream provided.");
            }

            UnderlyingStream = stream;
        }

        public void Dispose()
        {
            UnderlyingStream.Flush();
            UnderlyingStream.Close();
        }
    }
}
