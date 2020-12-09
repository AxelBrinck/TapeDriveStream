using System;
using System.IO;
using NUnit.Framework;
using TapeDriveStream;

namespace TapeDriveStream_Tests
{
    public class Tests
    {
        private static readonly string TestFileName = "test.bin";

        [SetUp]
        public void Setup()
        {
            // Create a dummy file to perform test with
            if (File.Exists(TestFileName)) File.Delete(TestFileName);

            using (var writer = 
                new BinaryWriter(
                    File.Open(
                        TestFileName, 
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
                TestFileName,
                FileMode.Open,
                FileAccess.Write,
                FileShare.None);

            Assert.Throws<InvalidOperationException>(() => {
                new TestTapeDrive(
                    unreadableStream
                );
            }, "Expected TapeDrive to refuse unreadable streams.");

            unreadableStream.Dispose();
        }

        [Test]
        public void Should_Refuse_Unwritable_Streams()
        {
            var unwritableStream = new FileStream(
                TestFileName,
                FileMode.Open,
                FileAccess.Read,
                FileShare.None);

            Assert.Throws<InvalidOperationException>(() => {
                new TestTapeDrive(
                    unwritableStream
                );
            }, "Expected TapeDrive to refuse unwritable streams.");
            
            unwritableStream.Dispose();
        }

        [Test]
        public void Should_Refuse_Zero_FrameSizes()
        {
            var stream = new FileStream(
                TestFileName,
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
                TestFileName,
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
    }

    public class TestTapeDrive : TapeDrive<int>
    {
        public TestTapeDrive(Stream stream) : base(stream, sizeof(int))
        {
            
        }
    }

    public class NegativeFrameSizeTapeDrive : TapeDrive<int>
    {
        public NegativeFrameSizeTapeDrive(Stream stream) : base(stream, -1)
        {
            
        }
    }

    public class ZeroFrameSizeTapeDrive : TapeDrive<int>
    {
        public ZeroFrameSizeTapeDrive(Stream stream) : base(stream, 0)
        {
            
        }
    }
}