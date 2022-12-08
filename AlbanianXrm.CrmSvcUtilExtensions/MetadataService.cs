using AlbanianXrm.Common.Shared;
using Microsoft.IO;
using Microsoft.PowerPlatform.Dataverse.ModelBuilderLib;
using Microsoft.Xrm.Sdk;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace AlbanianXrm.CrmSvcUtilExtensions
{
    public class MetadataService : IMetadataProviderService
    {
        private IOrganizationMetadata cachedMetadata;
        private readonly IMetadataProviderService defaultMetadataService;

        public IOrganizationService ServiceConnection { get; set; }

        public bool IsLiveConnectionRequired { get; set; } = true;

        public MetadataService(IMetadataProviderService defaultMetadataService)
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveEventHandler);
#if DEBUG
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_ATTACHDEBUGGER)))
            {
                System.Diagnostics.Debugger.Launch();
            }
#endif

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_CACHEMEATADATA)))
            {
                this.IsLiveConnectionRequired = false;
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

        private readonly string[] dependecies = new[] { "Microsoft.IO.RecyclableMemoryStream", "System.Buffers", "System.Memory", "System.Numerics.Vectors", "System.Runtime.CompilerServices.Unsafe" };

        /// <summary>
        /// Event fired by CLR when an assembly reference fails to load
        /// Assumes that related assemblies will be loaded from a subfolder named the same as the Plugin
        /// For example, a folder named Sample.XrmToolBox.MyPlugin 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Assembly loadAssembly = null;
            Assembly currAssembly = typeof(MetadataService).Assembly;

            // base name of the assembly that failed to resolve
            var argName = args.Name.Substring(0, args.Name.IndexOf(",", StringComparison.InvariantCulture));

            // if the current unresolved assembly is referenced by our plugin, attempt to load
            if (dependecies.Contains(argName))
            {
                // load from the path to this plugin assembly, not host executable
                string dir = Path.GetDirectoryName(currAssembly.Location).ToUpperInvariant();

                dir = Path.Combine(dir, "AlbanianEarlyBound");

                var assmbPath = Path.Combine(dir, $"{argName}.dll");

                if (File.Exists(assmbPath))
                {
                    loadAssembly = Assembly.LoadFrom(assmbPath);
                }
                else
                {
                    throw new FileNotFoundException($"Unable to locate dependency: {assmbPath}");
                }
            }

            return loadAssembly;
        }
    }
}
