using System;
using System.IO;
using NUnit.Framework;
using TapeDriveStream;

namespace TapeDriveStream_Tests
{
    public class Tests
    {
        private static readonly string SingleDecimalTestFileName
            = "1-decimal-test.bin";
        private static readonly string Int32TestFileName
            = "1M-int32-test.bin";

        [SetUp]
        public void Setup()
        {
            if (File.Exists(SingleDecimalTestFileName))
            {
                File.Delete(SingleDecimalTestFileName);
            }

            if (File.Exists(Int32TestFileName)) 
            {
                File.Delete(Int32TestFileName);
            }

            using (var writer = 
                new BinaryWriter(
                    File.Open(
                        SingleDecimalTestFileName, 
                        FileMode.Create,
                        FileAccess.Write,
                        FileShare.None)))
            {
                writer.Write((byte) 0);
            }

            using (var writer = 
                new BinaryWriter(
                    File.Open(
                        Int32TestFileName, 
                        FileMode.Create,
                        FileAccess.Write,
                        FileShare.None)))
            {
                for (var i = 0m; i < (1024 * 1024) / sizeof(decimal); i++)
                {   
                    writer.Write(i);
                }
            }
        }

        [Test]
        public void Should_Refuse_Unreadable_Streams()
        {
            var unreadableStream = new FileStream(
                Int32TestFileName,
                FileMode.Open,
                FileAccess.Write,
                FileShare.None);

            Assert.Throws<InvalidOperationException>(() => {
                new Int32TestTapeDrive(
                    unreadableStream
                );
            }, "Expected TapeDrive to refuse unreadable streams.");

            unreadableStream.Dispose();
        }

        [Test]
        public void Should_Refuse_Unwritable_Streams()
        {
            var unwritableStream = new FileStream(
                Int32TestFileName,
                FileMode.Open,
                FileAccess.Read,
                FileShare.None);

            Assert.Throws<InvalidOperationException>(() => {
                new Int32TestTapeDrive(
                    unwritableStream
                );
            }, "Expected TapeDrive to refuse unwritable streams.");
            
            unwritableStream.Dispose();
        }

        [Test]
        public void Should_Refuse_Zero_FrameSizes()
        {
            var stream = new FileStream(
                Int32TestFileName,
                FileMode.Open,
                FileAccess.ReadWrite,
                FileShare.None);

            Assert.Throws<ArgumentException>(() => {
                new ZeroFrameSizeTapeDrive(
                    stream
                );
            }, "Argument frameSize expects numbers above 0.");

            stream.Dispose();
        }

        [Test]
        public void Should_Refuse_Negative_FrameSizes()
        {
            var stream = new FileStream(
                Int32TestFileName,
                FileMode.Open,
                FileAccess.ReadWrite,
                FileShare.None);

            Assert.Throws<ArgumentException>(() => {
                new NegativeFrameSizeTapeDrive(
                    stream
                );
            }, "Argument frameSize expects numbers above 0.");
            
            stream.Dispose();
        }
        
        [Test]
        public void Should_Refuse_NonModZero_Streams()
        {
            var stream = new FileStream(
                SingleDecimalTestFileName,
                FileMode.Open,
                FileAccess.ReadWrite,
                FileShare.None);

            Assert.Throws<InvalidDataException>(() => {
                new Int32TestTapeDrive(
                    stream
                );
            }, "Stream-length mod frame-size not zero.");

            stream.Dispose();
        }
    }

    public class Int32TestTapeDrive : TapeDrive<int>
    {
        public Int32TestTapeDrive(Stream stream) : base(stream, sizeof(int))
        {
            
        }

        protected override int[] Deserialize(byte[] serial)
        {
            throw new NotImplementedException();
        }

        protected override byte[] Serialize(int[] objects)
        {
            throw new NotImplementedException();
        }
    }

    public class NegativeFrameSizeTapeDrive : TapeDrive<int>
    {
        public NegativeFrameSizeTapeDrive(Stream stream) : base(stream, -1)
        {
            
        }

        protected override int[] Deserialize(byte[] serial)
        {
            throw new NotImplementedException();
        }

        protected override byte[] Serialize(int[] objects)
        {
            throw new NotImplementedException();
        }
    }

    public class ZeroFrameSizeTapeDrive : TapeDrive<int>
    {
        public ZeroFrameSizeTapeDrive(Stream stream) : base(stream, 0)
        {
            
        }

        protected override int[] Deserialize(byte[] serial)
        {
            throw new NotImplementedException();
        }

        protected override byte[] Serialize(int[] objects)
        {
            throw new NotImplementedException();
        }
    }
}