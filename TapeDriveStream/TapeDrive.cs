using System;
using System.IO;

namespace TapeDriveStream
{
    public abstract class TapeDrive : IDisposable
    {
        /// <summary>
        /// Access to the underlying stream provided when
        /// <see cref="UnderlyingStream"/> was initialized.
        /// </summary>
        /// <para>
        /// A change in the underlying stream could lead to data-loss or even
        /// data-corruption.
        /// </para>
        /// <value>The underlying stream.</value>
        public Stream UnderlyingStream { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="UnderlyingStream"/> by
        /// providing a stream.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when unreadable/unwritable/unseekable stream is provided.
        /// </exception>
        /// <param name="stream"></param>
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

            if (!stream.CanSeek)
            {
                throw new InvalidOperationException(
                    "Unseekable streams are not supported.");
            }

            UnderlyingStream = stream;
        }

        public void Dispose()
        {
            UnderlyingStream.Dispose();
        }
    }
}
