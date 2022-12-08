using AlbanianXrm.BackgroundWorker;
using AlbanianXrm.EarlyBound.Helpers;
using AlbanianXrm.EarlyBound.Properties;
using AlbanianXrm.XrmToolBox.Shared.Extensions;
using System;
using System.IO;
using System.Windows.Forms;

namespace AlbanianXrm.EarlyBound.Logic
{
    class ExtensionsMoverHandler
    {
        private MyPluginControl myPlugin;
        private AlBackgroundWorkHandler backgroundWorkHandler;

        public ExtensionsMoverHandler(MyPluginControl myPlugin, AlBackgroundWorkHandler backgroundWorkHandler)
        {
            this.myPlugin = myPlugin;
            this.backgroundWorkHandler = backgroundWorkHandler;
        }

        internal void CopyCrmSvcUtilExtensionsToCLI()
        {
            backgroundWorkHandler.EnqueueBackgroundWork(
                AlBackgroundWorkerFactory.NewWorker(CopyCrmSvcUtilExtensionsToCLIInternal, CopyCrmSvcUtilExtensionsToCLIEnded)
                                   .WithViewModel(myPlugin.pluginViewModel)
                                   .WithMessage(myPlugin, Resources.DOWNLOADING_CORE_TOOLS));
        }

        public void CopyCrmSvcUtilExtensionsToCLIEnded(string value, Exception exception)
        {
            try
            {
                if (exception != null)
                {
                    MessageBox.Show(exception.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (value != null)
                {
                    MessageBox.Show(value, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                myPlugin.options.CrmSvcUtilExtensions = new CrmSvcUtilExtensionsEditor().GetVersion(myPlugin.options.CrmSvcUtilExtensions);
                myPlugin.options.RecycableMemoryStream = new MemoryStreamEditor().GetVersion(myPlugin.options.RecycableMemoryStream);
                MessageBox.Show("Assemblies copied", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string CopyCrmSvcUtilExtensionsToCLIInternal()
        {
            string dir = Path.GetDirectoryName(typeof(MyPluginControl).Assembly.Location).ToUpperInvariant();
            string folder = Path.GetFileNameWithoutExtension(typeof(MyPluginControl).Assembly.Location);
            dir = Path.Combine(dir, folder);
            Directory.CreateDirectory(dir);
            var pacDir = VersionEditor.GetPACDir();
            Directory.CreateDirectory(Path.Combine(pacDir, "AlbanianEarlyBound"));

            FileInfo extension = new FileInfo(Path.Combine(dir, "AlbanianXrm.CrmSvcUtilExtensions.dll"));
            if (!extension.Exists)
            {
                return "AlbanianXrm.CrmSvcUtilExtensions.dll does not exist";
            }

            foreach (var item in new[] { "Microsoft.IO.RecyclableMemoryStream.dll", "System.Buffers.dll", "System.Memory.dll", "System.Numerics.Vectors.dll", "System.Runtime.CompilerServices.Unsafe.dll" })
            {
                FileInfo memorystream = new FileInfo(Path.Combine(dir, item));
                if (!extension.Exists)
                {
                    return "Microsoft.IO.RecyclableMemoryStream.dll does not exist";
                }
                memorystream.CopyTo(Path.Combine(pacDir, "AlbanianEarlyBound", memorystream.Name), overwrite: true);
            }
            extension.CopyTo(Path.Combine(pacDir, extension.Name), overwrite: true);

            return null;
        }
    }
}
