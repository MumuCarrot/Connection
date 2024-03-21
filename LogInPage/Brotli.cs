using System.IO;
using System.IO.Compression;

namespace Utilities
{
    public class Brotli
    {
        private static CompressionLevel GetCompressionLevel()
        {
            if (Enum.IsDefined(typeof(CompressionLevel), 3)) // NOTE: CompressionLevel.SmallestSize == 3 is not supported in .NET Core 3.1
            {
                return (CompressionLevel)3;
            }
            return CompressionLevel.Optimal;
        }
        public static byte[] CompressBytes(byte[] bytes) => CompressBytesAsync(bytes).GetAwaiter().GetResult();
        public static async Task<byte[]> CompressBytesAsync(byte[] bytes, CancellationToken cancel = default)
        {
            using var outputStream = new MemoryStream();
            using (var compressionStream = new BrotliStream(outputStream, GetCompressionLevel()))
            {
                await compressionStream.WriteAsync(bytes, cancel);
            }
            return outputStream.ToArray();
        }

        public static byte[] DecompressBytes(byte[] bytes) => DecompressBytesAsync(bytes).GetAwaiter().GetResult();
        public static async Task<byte[]> DecompressBytesAsync(byte[] bytes, CancellationToken cancel = default)
        {
            using var inputStream = new MemoryStream(bytes);
            using var outputStream = new MemoryStream();
            using (var compressionStream = new BrotliStream(inputStream, CompressionMode.Decompress))
            {
                await compressionStream.CopyToAsync(outputStream, cancel);
            }
            return outputStream.ToArray();
        }
    }
}