using System.Drawing.Imaging;

namespace Tiveria.Common.Extensions
{
    public static class ImageExtensions
    {
        public static byte[] ToByteArray(this System.Drawing.Image imageIn, ImageFormat format, int quality = 100)
        {
            var ms = new System.IO.MemoryStream();
            EncoderParameters encoderParameters = new EncoderParameters(1);
            quality = (quality < 5 || quality > 100) ? 100 : quality;
            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, quality);
            imageIn.Save(ms, GetEncoder(format), encoderParameters);
            return ms.ToArray();
        }

        public static byte[] ToByteArray(this System.Drawing.Image imageIn)
        {
            var ms = new System.IO.MemoryStream();
            imageIn.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID== format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}