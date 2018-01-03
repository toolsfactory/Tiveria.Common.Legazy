
namespace Tiveria.Common.Extensions
{
    public static class ByteExtensions
    {
        public static System.Drawing.Image ToImage(this byte[] byteArrayIn)
        {
            var ms = new System.IO.MemoryStream();
            var returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }

        public static bool IsBitSet(this byte b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }

        public static bool IsBitSet(this int b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }
    }
}
