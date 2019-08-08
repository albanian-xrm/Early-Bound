using AlbanianXrm;
using AlbanianXrm.Common.Shared;
using Microsoft.Crm.Services.Utility;
using Microsoft.IO;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Xunit;

namespace Tests
{
    public class OrganizationMetadataTests
    {
        [Fact]
        public void OrganizationMetadataCanBeSerialized()
        {
            OrganizationMetadata organizationMetadata =
                new OrganizationMetadata(
                    new EntityMetadata[] {
                        new EntityMetadata()
                        {
                            LogicalName = "test"
                        }
                    },
                    new OptionSetMetadataBase[]{
                        new OptionSetMetadata()
                        {
                            Name ="test option"
                        }
                    },
                    new SdkMessages(new Dictionary<Guid, SdkMessage>()));

            var manager = new RecyclableMemoryStreamManager();
            string noise;
            OrganizationMetadata organizationMetadataDeserialized;
            using (var stream = manager.GetStream())
            {
                var serializer = new DataContractSerializer(typeof(OrganizationMetadata));

                using (var writer = new StreamWriter(stream, Encoding.UTF8, 1024, true))
                using (var xmlwriter = new XmlTextWriter(writer))
                {
                    writer.WriteLine("Noise");
                    serializer.WriteObject(xmlwriter, organizationMetadata);
                    writer.WriteLine();
                    writer.WriteLine(Constants.CONSOLE_ENDSTREAM);
                    writer.WriteLine("other noise");
                }

                using (var xmlStream = manager.GetStream())
                {
                    stream.Position = 0;
                    using (var reader = new StreamReader(stream, Encoding.UTF8, true, 1024, true))
                    {
                        noise = reader.ReadLine();
                        using (var writer = new StreamWriter(xmlStream, Encoding.UTF8, 1024, true))
                        {
                            string line = reader.ReadLine();
                            while (line != Constants.CONSOLE_ENDSTREAM && line != null)
                            {
                                writer.WriteLine(line);
                                line = reader.ReadLine();
                            }
                        }
                        xmlStream.Position = 0;

                        using (var xmlreader = new XmlTextReader(xmlStream))
                        {
                            organizationMetadataDeserialized = (OrganizationMetadata)serializer.ReadObject(xmlreader, false);
                        }
                    }
                }

            }

            Assert.Equal("Noise", noise);
            Assert.NotNull(organizationMetadataDeserialized);
        }
    }
}
