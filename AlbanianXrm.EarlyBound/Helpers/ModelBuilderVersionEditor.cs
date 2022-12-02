using Helpers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.IO;
using System.Linq;

namespace AlbanianXrm.EarlyBound.Helpers
{
    internal class ModelBuilderVersionEditor : UITypeEditor
    {
        public ModelBuilderVersionEditor()
        {
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            return GetVersion(value as string);
        }

        public string GetVersion(string value)
        {
            Process process = ProcessHelper.getProcess("pac.launcher.exe");
            process.StartInfo.Arguments = "use";
            Version version = new Version(0, 0, 0);
            try
            {
                process.Start();
                using (StreamReader sr = process.StandardOutput)
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var lineParts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        var thisVersion = new Version(lineParts[0]);
                        if (thisVersion > version)
                        {
                            version = thisVersion;
                        }
                        if (lineParts.Contains("(In"))
                        {
                            return thisVersion.ToString();
                        }
                    }
                    if (version > new Version(0, 0, 0))
                    {
                        return version.ToString();
                    }
                }
                process.WaitForExit();
            }
            catch { }
            return value;
        }
    }
}
