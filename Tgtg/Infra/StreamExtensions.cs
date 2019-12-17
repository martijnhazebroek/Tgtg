using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace Hazebroek.Tgtg.Infra
{
    internal static class StreamExtensions
    {
        public static T ReadAndDeserializeFromJson<T>(this Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (!stream.CanRead) throw new NotSupportedException("Can't read from this stream.");

            using var streamReader = new StreamReader(stream, Encoding.UTF8, true, 1024, false);
            using var jsonTextReader = new JsonTextReader(streamReader);
            var jsonSerializer = new JsonSerializer();
            return jsonSerializer.Deserialize<T>(jsonTextReader);
        }
    }
}