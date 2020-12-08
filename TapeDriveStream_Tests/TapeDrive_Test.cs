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
                for (var i = 0L; i < (1024 * 1024) / sizeof(decimal); i++)
                {   
                    writer.Write(i);
                }
            }
        }

        [Test]
        public void Should_Refuse_Unreadable_Streams()
        {
            var stream = new FileStream(
                TestFileName,
                FileMode.Open,
                FileAccess.Write,
                FileShare.None);

            Assert.Throws<InvalidOperationException>(() => {
                new TestTapeDrive(
                    stream
                );
            }, "Expected TapeDrive to refuse unreadable streams.");

            stream.Dispose();
        }

        [Test]
        public void Should_Refuse_Unwritable_Streams()
        {
            var stream = new FileStream(
                TestFileName,
                FileMode.Open,
                FileAccess.Read,
                FileShare.None);

            Assert.Throws<InvalidOperationException>(() => {
                new TestTapeDrive(
                    stream
                );
            }, "Expected TapeDrive to refuse unwritable streams.");
            
            stream.Dispose();
        }
    }

    public class TestTapeDrive : TapeDrive
    {
        public TestTapeDrive(Stream stream) : base(stream)
        {
            
        }
    }
}