using AlbanianXrm.Common.Shared;
using Microsoft.IO;
using Microsoft.PowerPlatform.Dataverse.ModelBuilderLib;
using Microsoft.Xrm.Sdk;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace AlbanianXrm.CrmSvcUtilExtensions
{
    public class MetadataService : IMetadataProviderService
    {
        private IOrganizationMetadata cachedMetadata;
        private readonly IMetadataProviderService defaultMetadataService;

        public IOrganizationService ServiceConnection { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool IsLiveConnectionRequired => throw new NotImplementedException();

        public MetadataService(IMetadataProviderService defaultMetadataService)
        {
#if DEBUG
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_ATTACHDEBUGGER)))
            {
                System.Diagnostics.Debugger.Launch();
            }
#endif

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_CACHEMEATADATA)))
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
                    using (var xmlreader = XmlReader.Create(stream, new XmlReaderSettings()))
                    {
                        cachedMetadata = (OrganizationMetadata)serializer.ReadObject(xmlreader);
                    }
                }
            }
            this.defaultMetadataService = defaultMetadataService;
        }

        public IOrganizationMetadata LoadMetadata(IServiceProvider service)
        {
            if (cachedMetadata == null)
            {
                cachedMetadata = defaultMetadataService.LoadMetadata(service);
            }
            return cachedMetadata;
        }
    }
}
