using Helpers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.IO;

namespace AlbanianXrm.EarlyBound.Helpers
{
    internal class VersionEditor : UITypeEditor
    {
        private readonly string targetFileName;
        public VersionEditor(string targetFileName)
        {
            this.targetFileName = targetFileName;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            return GetVersion(value as Version);
        }

        public Version GetVersion(Version value)
        {
            var dir = GetPACDir();
            if (string.IsNullOrEmpty(dir))
            {
                return value;
            }

            FileInfo targetFile = new FileInfo(Path.Combine(dir, targetFileName));
            if (targetFile.Exists)
            {
                return new Version(FileVersionInfo.GetVersionInfo(targetFile.FullName).FileVersion);
            }
            return value;
        }

        public static string GetPACDir()
        {
            var pacVersion = new ModelBuilderVersionEditor().GetVersion("");

            if (string.IsNullOrEmpty(pacVersion))
            {
                return "";
            }

            Process process = ProcessHelper.getProcess("where.exe");
            process.StartInfo.Arguments = "pac.launcher.exe";
            process.Start();
            string pacLauncherPath = process.StandardOutput.ReadLine();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                return "";
            }
            string dir = Path.GetDirectoryName(pacLauncherPath).ToUpperInvariant();
            return Path.Combine(dir, "Microsoft.PowerApps.CLI." + pacVersion, "tools");
        }
    }
}
