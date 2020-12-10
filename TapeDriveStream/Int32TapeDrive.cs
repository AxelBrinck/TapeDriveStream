using System.IO;

namespace TapeDriveStream
{
    public class Int32TapeDrive : TapeDrive<int>
    {
        public Int32TapeDrive(Stream stream) : base(stream, sizeof(int))
        {
            
        }

        protected override int[] Deserialize(byte[] serial)
        {
            throw new System.NotImplementedException();
        }

        protected override byte[] Serialize(int[] objects)
        {
            throw new System.NotImplementedException();
        }
    }
}