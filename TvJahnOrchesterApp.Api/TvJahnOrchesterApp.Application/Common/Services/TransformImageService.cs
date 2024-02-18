using ImageMagick;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvJahnOrchesterApp.Application.Common.Errors;

namespace TvJahnOrchesterApp.Application.Common.Services
{
    internal class TransformImageService
    {
        public static byte[]? ConvertToCompressedByteArray(string? base64String)
        {
            if(base64String == null || base64String.Length == 0)
            {
                return null;
            }
            var imageAsByteArray = ConvertBase64ToByteArray(base64String);
            var targetSizeInBytes = 70000;
            return CompressImage(imageAsByteArray, targetSizeInBytes);
        }

        public static byte[]? ConvertBase64ToByteArray(string? base64String)
        {
            if(base64String != null)
            {
                return Convert.FromBase64String(base64String);
            }
            return null;
        }

        public static string? ConvertByteArrayToBase64(byte[]? byteArray)
        {
            if (byteArray != null)
            {
                return Convert.ToBase64String(byteArray);
            }
            return null;
        }

        public static byte[] CompressImage(byte[] imageBytes, int targetSizeInBytes)
        {
            using (MemoryStream memoryStream = new MemoryStream(imageBytes))
            {
                using (MagickImage image = new MagickImage(memoryStream))
                {
                    // Set compression options
                    image.Quality = 80; // You can adjust the quality value as needed

                    // Compress the image
                    image.Settings.Compression = CompressionMethod.JPEG;
                    using (MemoryStream compressedStream = new MemoryStream())
                    {
                        image.Resize(256, 256);
                        image.Write(compressedStream);
                        compressedStream.Position = 0;
                        int loopCounter = 0;

                        while (compressedStream.Length > targetSizeInBytes)
                        {
                            if(loopCounter > 20)
                            {
                                throw new ImageToBigException();
                            }
                            image.Quality -= 20 + loopCounter; // Adjust the quality by increments
                            compressedStream.SetLength(0);
                            image.Write(compressedStream);
                            compressedStream.Position = 0;
                            loopCounter++;
                        }

                        return compressedStream.ToArray();
                    }
                }
            }
        }
    }
}
