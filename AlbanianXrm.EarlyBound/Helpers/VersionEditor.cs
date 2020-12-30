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
            string dir = Path.GetDirectoryName(typeof(MyPluginControl).Assembly.Location).ToUpperInvariant();
            string folder = Path.GetFileNameWithoutExtension(typeof(MyPluginControl).Assembly.Location);
            dir = Path.Combine(dir, folder);
            FileInfo targetFile = new FileInfo(Path.Combine(dir, targetFileName));
            if (targetFile.Exists)
            {
                return new Version(FileVersionInfo.GetVersionInfo(targetFile.FullName).FileVersion);
            }
            return value;
        }
    }
}
