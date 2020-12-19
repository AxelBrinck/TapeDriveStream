﻿using System;
using System.IO;

namespace TapeDriveStream
{
    public abstract class TapeDrive<T> : IDisposable
    {
        /// <summary>
        /// Access to the underlying stream provided when
        /// <see cref="_underlyingStream"/> was initialized.
        /// </summary>
        /// <para>
        /// A change in the underlying stream could lead to data-loss or even
        /// data-corruption.
        /// </para>
        /// <value>The underlying stream.</value>
        private Stream _underlyingStream;

        /// <summary>
        /// The size in bytes for each element.
        /// </summary>
        /// <para>
        /// Used to seek to X element and determinate stream corruption.
        /// </para>
        /// <value>The size for each element.</value>
        private int _frameSize { get; }

        /// <summary>
        /// Retrieves a cached integer representing the stream length.
        /// </summary>
        /// <para>
        /// This number gets updated whenever we write additional frames to the
        /// tape. 
        /// TODO: Feature yet to be written.
        /// </para>
        /// <value>The stream length.</value>
        public long StreamLength { get; }

        /// <summary>
        /// A representation of a chunk in a stream.
        /// </summary>
        /// <para>
        /// The stream buffer will lower the amount of disk-IO operations.
        /// Gets generated by calling <see cref="Serialize"/>.
        /// </para>
        /// <value></value>
        private T[] _buffer { get; }

        private readonly int _maxFramesToBuffer;

        /// <summary>
        /// Initializes a new instance of <see cref="TapeDrive"/>.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown when unreadable/unwritable/unseekable stream is provided.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// When invalid frame-size is provided.
        /// </exception>
        /// <exception cref="System.IO.InvalidDataException">
        /// Thrown when corrupted stream.
        /// </exception>
        /// <param name="stream">The source stream to use.</param>
        /// <param name="frameSize">The size in bytes for each frame.</param>
        /// <param name="maxFramesToBuffer">
        /// The maximum number of frames to read at once from the stream.
        /// </param>
        public TapeDrive(Stream stream, int frameSize, int maxFramesToBuffer)
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

            if (StreamLength % frameSize != 0)
            {
                throw new InvalidDataException(
                    "Stream-length mod frame-size not zero."
                );
            }

            _frameSize = frameSize;
            _underlyingStream = stream;
            _maxFramesToBuffer = maxFramesToBuffer;
        }

        /// <summary>
        /// Converts a byte-array to object-array.
        /// </summary>
        /// <param name="serial">The byte array to be converted.</param>
        /// <returns>The converted array of objects.</returns>
        protected abstract T[] Deserialize(byte[] serial);

        /// <summary>
        /// Converts an array of objects to byte-array.
        /// </summary>
        /// <param name="objects">The array of objects to be converted.</param>
        /// <returns>The resulting byte array.</returns>
        protected abstract byte[] Serialize(T[] objects);

        public void Dispose()
        {
            _underlyingStream.Dispose();
        }
    }
}
