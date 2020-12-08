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
            Assert.Throws<InvalidOperationException>(() => {
                new Int32TapeDrive(
                    new FileStream(
                        TestFileName,
                        FileMode.Open,
                        FileAccess.Write,
                        FileShare.None)
                );
            }, $@"An unreadable stream pointing to {TestFileName} has been
            provided and TapeDrive did not threw.");
        }
    }
}