using AlbanianXrm.BackgroundWorker;
using AlbanianXrm.EarlyBound.Helpers;
using AlbanianXrm.EarlyBound.Models;
using AlbanianXrm.EarlyBound.Properties;
using AlbanianXrm.XrmToolBox.Shared.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlbanianXrm.EarlyBound.Logic
{
    internal class CoreToolsDownloader
    {
        private readonly MyPluginControl myPlugin;
        private readonly AlBackgroundWorkHandler backgroundWorkHandler;
        //private readonly HttpClient httpClient;
        private const string coreToolsId = "Microsoft.CrmSdk.CoreTools";
        private const string NuGetURI = "https://api.nuget.org/v3/index.json";

        public CoreToolsDownloader(MyPluginControl myPlugin, AlBackgroundWorkHandler backgroundWorkHandler)
        {
            this.myPlugin = myPlugin;
            this.backgroundWorkHandler = backgroundWorkHandler;
            //var defaultWebProxy = WebRequest.DefaultWebProxy;
            //defaultWebProxy.Credentials = CredentialCache.DefaultCredentials;
            //// httpClient = new HttpClient(new HttpClientHandler { Proxy = defaultWebProxy, AllowAutoRedirect = true });
            //httpClient = new HttpClient();
        }

        public void DownloadCoreTools(string specificVersion)
        {
            backgroundWorkHandler.EnqueueBackgroundWork(
                AlBackgroundWorkerFactory.NewAsyncWorker(DownloadCoreToolsAsync, specificVersion, DownloadCoreToolsEnded)
                                         .WithViewModel(myPlugin.pluginViewModel)
                                         .WithMessage(myPlugin, Resources.DOWNLOADING_CORE_TOOLS));

        }

        public void DownloadCoreToolsEnded(string version, string value, Exception exception)
        {
            try
            {
                if (exception != null)
                {
                    MessageBox.Show(exception.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (value != null)
                {
                    MessageBox.Show(value, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                myPlugin.options.CrmSvcUtils = new CrmSvcUtilsEditor().GetVersion(myPlugin.options.CrmSvcUtils);
                myPlugin.options.RecycableMemoryStream = new MemoryStreamEditor().GetVersion(myPlugin.options.RecycableMemoryStream);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async Task<string> DownloadCoreToolsAsync(string version)
        {
            //ID of the package to be looked 
            var nuGetFeed = new Uri(myPlugin.options.NuGetFeed ?? NuGetURI);
            NuGetSearch nuGetSearch = null;
            string dir = Path.GetDirectoryName(typeof(MyPluginControl).Assembly.Location).ToUpperInvariant();
            string folder = Path.GetFileNameWithoutExtension(typeof(MyPluginControl).Assembly.Location);
            dir = Path.Combine(dir, folder);
            Directory.CreateDirectory(dir);

            //Connect to the official package repository IPackageRepository

            if (version == null)
            {
                if (nuGetFeed.IsFile)
                {
                    var packages = Directory.GetFiles(myPlugin.options.NuGetFeed, coreToolsId + ".*.nupkg");
                    version = packages.Select(package => Path.GetFileName(package))
                                      .Select((package) => package.Length >= 33 ? package.Substring(27, package.Length - 33) : "")
                                      .Where(v => Version.TryParse(v, out _))
                                      .OrderByDescending((v) => new Version(v))
                                      .FirstOrDefault();
                }
                else
                {
                    nuGetSearch = await GetNuGetSearch(nuGetFeed);
                    version = nuGetSearch.Data?.Find(package => package.Id == coreToolsId)?.Version;
                }
            }

#if DEBUG
            System.Diagnostics.Debug.WriteLine($"Version {version}");
#endif

            string localPackagePath = null;
            if (nuGetFeed.IsFile)
            {
                var packages = Directory.GetFiles(myPlugin.options.NuGetFeed, coreToolsId + ".*.nupkg");
                localPackagePath = packages.Select(package => Path.GetFileName(package))
                                           .Where((package) => package.Length >= 33 ? package.Substring(27, package.Length - 33) == version : false)
                                           .Select(package => Path.Combine(myPlugin.options.NuGetFeed, package))
                                           .FirstOrDefault();
            }
            else
            {
                if (nuGetSearch == null)
                {
                    nuGetSearch = await GetNuGetSearch(nuGetFeed);
                }
                var onlinePackageInfoPath = nuGetSearch.Data.Find(package => package.Id == coreToolsId).Versions.Find(v => v.Version == version)?.Id;
                string packageContent = null;
                if (onlinePackageInfoPath == null ||
                    (packageContent = await GetPackageContent(onlinePackageInfoPath)) == null ||
                    (localPackagePath = await DownloadNuGetPackage(packageContent)) == null)
                {
                    return string.Format(Resources.Culture, Resources.CORE_TOOLS_NOT_FOUND, coreToolsId, myPlugin.options.NuGetFeed);
                }

            }

            var packageInfo = new FileInfo(localPackagePath);
            using (var package = Package.Open(packageInfo.OpenRead()))
            {
                foreach (var part in package.GetParts())
                {
                    var path = part.Uri.ToString();

                    if (!path.StartsWith("/content/bin/coretools", StringComparison.InvariantCultureIgnoreCase)) continue;

                    var fileName = Path.GetFileName(path);


                    using (var fileStream = File.OpenWrite(Path.Combine(dir, fileName)))
                    using (var stream = part.GetStream())
                    {
                        await stream.CopyToAsync(fileStream);
                    }
                }
            }
            return null;
        }

        private async Task<string> DownloadNuGetPackage(string packageContent)
        {
            var tempPath = new DirectoryInfo(Path.Combine(Path.GetTempPath(), "AlbanianEarlyBound"));
            tempPath.Create();
            var fileInfo = new FileInfo(Path.Combine(tempPath.FullName, Path.GetFileName(packageContent)));
            var   httpClient = new HttpClient { BaseAddress = new Uri(new Uri(packageContent).GetLeftPart(UriPartial.Authority)) };
            var stream = await httpClient.GetStreamAsync(packageContent);
            using(var fileStream = fileInfo.OpenWrite()) {
                await stream.CopyToAsync(fileStream);
            }
            return fileInfo.FullName;
        }

        public async Task<string> GetPackageContent(string package)
        {  
            var httpClient = new HttpClient { BaseAddress = new Uri(new Uri(package).GetLeftPart(UriPartial.Authority)) };
            var stream = await httpClient.GetStreamAsync(package);
            using (var reader = new JsonTextReader(new StreamReader(stream)))
            {
                return (await JObject.LoadAsync(reader)).ToObject<NuGetPackage>().PackageContent;
            }
        }

        public async Task<NuGetSearch> GetNuGetSearch(Uri nuGetFeed)
        {           
            var searchEndpoint = new Uri( await GetSearchEndpoint(nuGetFeed) + $"?q={coreToolsId}");
            var httpClient = new HttpClient { BaseAddress = new Uri(searchEndpoint.GetLeftPart(UriPartial.Authority)) };
            var stream = await httpClient.GetStreamAsync(searchEndpoint);
            using (var reader = new JsonTextReader(new StreamReader(stream)))
            {
                return (await JObject.LoadAsync(reader)).ToObject<NuGetSearch>();
            }
        }

        public async Task<string> GetSearchEndpoint(Uri nuGetFeed)
        {
            var httpClient = new HttpClient { BaseAddress = new Uri(nuGetFeed.GetLeftPart(UriPartial.Authority)) };
            var stream = await httpClient.GetStreamAsync(nuGetFeed);

            NuGetApi nuGetApi = null;
            using (var reader = new JsonTextReader(new StreamReader(stream)))
            {
                nuGetApi = (await JObject.LoadAsync(reader)).ToObject<NuGetApi>();
            }
            return nuGetApi.Resources?.Find(resource => resource.type == "SearchQueryService")?.id;
        }
    }
}
