using System.IO;

namespace TapeDriveStream
{
    public class Int32TapeDrive : TapeDrive<int>
    {
        public Int32TapeDrive(Stream stream) : base(stream, sizeof(int))
        {
            
        }
    }
}