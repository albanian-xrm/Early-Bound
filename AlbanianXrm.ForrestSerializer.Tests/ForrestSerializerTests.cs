using System.Collections.Generic;
using System.IO;
using Xunit;

namespace AlbanianXrm.Tests
{
    public class ForrestSerializerTests
    {
        [Fact]
        public void CanDeserialize_Case1()
        {
            MemoryStream memoryStream = new MemoryStream(new byte[9000], true);
            int length = 0;
            using (var writer = new StreamWriter(memoryStream))
            {
                writer.Write("╠╦═Entity");
                writer.Flush();
                length = (int)memoryStream.Position;
            }

            Dictionary<string, EntitySelection> result;
            using (var reader = new StreamReader(new MemoryStream(memoryStream.ToArray(), 0, length)))
            {
                result = ForrestSerializer.Deserialize(reader);
            }

            Assert.NotEmpty(result);
            Assert.True(result.ContainsKey("Entity"));
        }
    }
}
