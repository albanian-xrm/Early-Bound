using AlbanianXrm.Common.Shared;
using Microsoft.Crm.Services.Utility;
using Microsoft.IO;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace AlbanianXrm.CrmSvcUtilExtensions
{
    public class MetadataService : IMetadataProviderService
    {
        IOrganizationMetadata cachedMetadata;
        IMetadataProviderService defaultMetadataService;

        public MetadataService(IMetadataProviderService defaultMetadataService)
        {
#if DEBUG
            if ((Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_ATTACHDEBUGGER) ?? "") != "")
            {
                System.Diagnostics.Debugger.Launch();
            }
#endif

            if ((Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_CACHEMEATADATA) ?? "") != "")
            {
                Console.WriteLine(Constants.CONSOLE_METADATA);

                var manager = new RecyclableMemoryStreamManager();
                using (var stream = manager.GetStream())
                {
                    using (var writer = new StreamWriter(stream, Encoding.UTF8, 1024, true))
                    {
                        string line = Console.ReadLine();
                        while (line != Constants.CONSOLE_ENDSTREAM && line != null)
                        {
                            writer.WriteLine(line);
                            line = Console.ReadLine();
                        }
                    }
                    stream.Position = 0;
                    var serializer = new DataContractSerializer(typeof(OrganizationMetadata));
                    using (var xmlreader = new XmlTextReader(stream))
                    {
                        cachedMetadata = (OrganizationMetadata)serializer.ReadObject(xmlreader);
                    }
                }
            }
            this.defaultMetadataService = defaultMetadataService;
        }

        public IOrganizationMetadata LoadMetadata()
        {
            if (cachedMetadata == null)
            {
                cachedMetadata = defaultMetadataService.LoadMetadata();
            }
            return cachedMetadata;
        }
    }
}
