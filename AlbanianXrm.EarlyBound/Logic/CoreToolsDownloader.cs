using AlbanianXrm.BackgroundWorker;
using AlbanianXrm.EarlyBound.Helpers;
using AlbanianXrm.EarlyBound.Properties;
using AlbanianXrm.XrmToolBox.Shared.Extensions;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlbanianXrm.EarlyBound.Logic
{
    internal class CoreToolsDownloader
    {
        private readonly MyPluginControl myPlugin;
        private readonly AlBackgroundWorkHandler backgroundWorkHandler;

        public CoreToolsDownloader(MyPluginControl myPlugin, AlBackgroundWorkHandler backgroundWorkHandler)
        {
            this.myPlugin = myPlugin;
            this.backgroundWorkHandler = backgroundWorkHandler;
        }

        public void DownloadCoreTools(string specificVersion)
        {
            NuGetVersion version = string.IsNullOrWhiteSpace(specificVersion) ? null : new NuGetVersion(specificVersion);
            backgroundWorkHandler.EnqueueBackgroundWork(
                AlBackgroundWorkerFactory.NewAsyncWorker(DownloadCoreToolsAsync, version, DownloadCoreToolsEnded)
                                         .WithViewModel(myPlugin.pluginViewModel)
                                         .WithMessage(myPlugin, Resources.DOWNLOADING_CORE_TOOLS));
        }

        public void DownloadCoreToolsEnded(NuGetVersion version, string value, Exception exception)
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
            //    myPlugin.options.CrmSvcUtils = new CrmSvcUtilsEditor().GetVersion(myPlugin.options.CrmSvcUtils);
                myPlugin.options.RecycableMemoryStream = new MemoryStreamEditor().GetVersion(myPlugin.options.RecycableMemoryStream);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async Task<string> DownloadCoreToolsAsync(NuGetVersion version)
        {
            //ID of the package to be looked 
            string coreToolsId = "Microsoft.CrmSdk.CoreTools";

            string dir = Path.GetDirectoryName(typeof(MyPluginControl).Assembly.Location).ToUpperInvariant();
            string folder = Path.GetFileNameWithoutExtension(typeof(MyPluginControl).Assembly.Location);
            dir = Path.Combine(dir, folder);
            Directory.CreateDirectory(dir);

            //Connect to the official package repository IPackageRepository
            ILogger logger = NullLogger.Instance;
            CancellationToken cancellationToken = CancellationToken.None;
            SourceCacheContext cache = new SourceCacheContext();

            SourceRepository repository = Repository.Factory.GetCoreV3(""/*myPlugin.options.NuGetFeed*/);
            PackageSearchResource packageSearch = await repository.GetResourceAsync<PackageSearchResource>();
            FindPackageByIdResource findPackageById = await repository.GetResourceAsync<FindPackageByIdResource>();
            if (version == null)
            {
                var metadata = (await packageSearch.SearchAsync(coreToolsId, new SearchFilter(false, SearchFilterType.IsLatestVersion), 0, 1, logger, cancellationToken)).FirstOrDefault();
                version = (await metadata.GetVersionsAsync()).Max(v => v.Version);
            }

#if DEBUG
            System.Diagnostics.Debug.WriteLine($"Version {version}");
#endif

            using (MemoryStream packageStream = new MemoryStream())
            {
                if (!await findPackageById.CopyNupkgToStreamAsync(
                     coreToolsId,
                     version,
                     packageStream,
                     cache,
                     logger,
                     cancellationToken))
                {
                    return string.Format(Resources.Culture, Resources.CORE_TOOLS_NOT_FOUND, coreToolsId, ""/*myPlugin.options.NuGetFeed*/);
                }

                using (PackageArchiveReader packageReader = new PackageArchiveReader(packageStream))
                {
                    foreach (var packageFile in await packageReader.GetFilesAsync(cancellationToken))
                    {
                        System.Diagnostics.Debug.WriteLine($"{packageFile} {Path.GetDirectoryName(packageFile)} {Path.GetFileName(Path.GetDirectoryName(packageFile))}");

                        if (Path.GetFileName(Path.GetDirectoryName(packageFile)) == "coretools")
                        {
                            using (var fileStream = File.OpenWrite(Path.Combine(dir, Path.GetFileName(packageFile))))
                            using (var stream = await packageReader.GetStreamAsync(packageFile, cancellationToken))
                            {
                                await stream.CopyToAsync(fileStream);
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
