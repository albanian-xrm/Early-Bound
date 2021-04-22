using AlbanianXrm.EarlyBound.Helpers;
using AlbanianXrm.EarlyBound.Properties;
using AlbanianXrm.XrmToolBox.Shared;
using NuGet.Common;
using NuGet.Packaging;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
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
        private readonly BackgroundWorkHandler backgroundWorkHandler;

        public CoreToolsDownloader(MyPluginControl myPlugin, BackgroundWorkHandler backgroundWorkHandler)
        {
            this.myPlugin = myPlugin;
            this.backgroundWorkHandler = backgroundWorkHandler;
        }

        public void DownloadCoreTools()
        {
            backgroundWorkHandler.EnqueueAsyncWork(Resources.DOWNLOADING_CORE_TOOLS, DownloadCoreToolsAsync, DownloadCoreToolsEnded);
        }

        public void DownloadCoreToolsEnded(BackgroundWorkResult<string> args)
        {
            try
            {
                if (args.Exception != null)
                {
                    MessageBox.Show(args.Exception.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (args.Value != null)
                {
                    MessageBox.Show(args.Value, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                myPlugin.options.CrmSvcUtils = new CrmSvcUtilsEditor().GetVersion(myPlugin.options.CrmSvcUtils);
                myPlugin.options.RecycableMemoryStream = new MemoryStreamEditor().GetVersion(myPlugin.options.RecycableMemoryStream);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public async Task<string> DownloadCoreToolsAsync()
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

            SourceRepository repository = Repository.Factory.GetCoreV3(myPlugin.options.NuGetFeed);
            PackageSearchResource packageSearch = await repository.GetResourceAsync<PackageSearchResource>();
            FindPackageByIdResource findPackageById = await repository.GetResourceAsync<FindPackageByIdResource>();

            var metadata = (await packageSearch.SearchAsync(coreToolsId, new SearchFilter(false, SearchFilterType.IsLatestVersion), 0, 1, logger, cancellationToken)).FirstOrDefault();
            var version = (await metadata.GetVersionsAsync()).Max(v => v.Version);
            System.Diagnostics.Debug.WriteLine($"Version {version}");
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
                    return string.Format(Resources.Culture, Resources.CORE_TOOLS_NOT_FOUND, coreToolsId, myPlugin.options.NuGetFeed);
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
