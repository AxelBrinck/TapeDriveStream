using System;
using System.IO;

namespace TapeDriveStream
{
    public abstract class TapeDrive<T> : IDisposable
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
        internal Stream UnderlyingStream { get; }

        internal int FrameSize { get; }

        /// <summary>
        /// Retrieves a cached integer representing the stream length.
        /// This number gets updated whenever we write additional frames to the
        /// tape. 
        /// TODO: Procedure to be written.
        /// </summary>
        /// <value>The stream length.</value>
        public long StreamLength { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="UnderlyingStream"/> by
        /// providing a stream.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when unreadable/unwritable/unseekable stream is provided.
        /// </exception>
        /// <param name="stream">The source stream to use.</param>
        /// <param name="frameSize">The size in bytes of each frame.</param>
        public TapeDrive(Stream stream, int frameSize)
        {
            if (!stream.CanRead)
            {
                throw new InvalidOperationException(
                    "Unreadable stream provided."
                );
            }

            if (!stream.CanWrite)
            {
                throw new InvalidOperationException(
                    "Unwritable stream provided."
                );
            }

            if (!stream.CanSeek)
            {
                throw new InvalidOperationException(
                    "Unseekable streams are not supported."
                );
            }

            StreamLength = stream.Length;

            if (frameSize < 1)
            {
                throw new ArgumentException(
                    $"Argument {nameof(frameSize)} expects numbers above 0."
                );
            }

            Console.WriteLine(StreamLength);
            Console.WriteLine(frameSize);

            if (StreamLength % frameSize != 0)
            {
                throw new InvalidDataException(
                    "Stream-length mod frame-size not zero."
                );
            }

            FrameSize = frameSize;
            UnderlyingStream = stream;
        }

        public void Dispose()
        {
            UnderlyingStream.Dispose();
        }
    }
}
