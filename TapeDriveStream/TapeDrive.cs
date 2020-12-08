using System;
using System.IO;

namespace TapeDriveStream
{
    public abstract class TapeDrive
    {
        public Stream UnderlyingStream { get; }

        public TapeDrive(Stream stream)
        {
            if (!stream.CanRead)
            {
                throw new InvalidOperationException(
                    "Unreadable stream provided.");
            }

            UnderlyingStream = stream;
        }
    }
}
