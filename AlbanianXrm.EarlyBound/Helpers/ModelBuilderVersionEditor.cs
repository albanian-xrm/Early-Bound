using Helpers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.IO;

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
            try
            {
                process.Start();
                using (StreamReader sr = process.StandardOutput)
                {
                    sr.ReadLine(); //Microsoft PowerPlatform CLI
                    string line = sr.ReadLine();
                    if (line != null)
                    {
                        return line.Substring("Version:".Length);
                    }
                }
            }
            catch { }
            process.WaitForExit();

            return value;
        }
    }
}
