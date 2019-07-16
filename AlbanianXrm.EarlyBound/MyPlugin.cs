using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace AlbanianXrm.EarlyBound
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "Albanian Early Bound"),
        ExportMetadata("Company", "Betim Beja"),
        ExportMetadata("Description", "This plugin generates early bound entities using svcutil.exe giving the possibility to choose the wanted attributes"),
        // Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAP+SURBVFhH7VdZbE1RFF2GlhhLDUGooTSNGisNakjoRz/aP0GIGmpIKDVESBAqPoikKBKJsREkPqUiYkppQ5SaKkGNlYiYitKgNaz1zr3e4J2+iw8/VnPePfecvffZZ++193vFf/xrNHKefmQjk6sjOX7d+xt85x9Qij0oMgsGwYfo8FgcQwvnPQJGdTDPklfmGREfOap5RoATjZ2ngW7u8XAhq5cZnmFsp/o+HYQ6EDHsLZsCYzqa+a23Zgijuaa9iAg5I9gBD4iixgzeummAGa3N5FrUH7DGkwODY/xkefsF2FEJzO7jLBDZPLzgHvfqzLtkpeMFVgfaRQNNnFOnxgEJbcxcuMGwxwVwpTvnWnORSFnpCLIhWzaEdUDhPToS2D7UGFh3G1jcD4j2EK9mlFlI2bXUkW4BbRwZ4Y9gKMKarGfF7n1IJWpdSgOGtaczDPu2IUBGV7uxTO5tpYxSlEId6cqJ/Y/UBsIj2NYcbEQ3rHDefBjQFthCo9XM/fIbJrfpXYAa5ruq1sgoBa1ZASefA9eZis2DgBiGfck1oOKdkfFBXjzDJuzGSrPQAAdUVgv6AsNjSbgy4HAVUDQaSObNVt0EOjVnWfNQlV5nzlffMnuSkexc6oyg7vx4Y8sGqwMvPgHnXwDnOLJ6GmKNPQvU1rPzjQdieUOVo4bmWvvIPclIdhp1pHvhJfDys2M0DCKmwEWvlsDSBDYehlQkXUSiJZ0wPKlIZ+5Zhl8ZYqUs/y7wSG03FL+TglDoIJdImrtPZ/rz+Y3D3fcCqwMJrYFBJFw/Ptf0B6b0APIqTLhzmNf7NYwGb36To/IDS498ac89yUhWOtKVDdmywepARxJL33ZjSaCDj4Hb74HicYZ4qWeA16wKleo+jjfMsdZEyPOUkax0pJtKG7JlQxPnaZCMNLTBKE1VYmVvTE/YmQzEt2K/v+yTwjy2YZWlnNC+al2lWULC5ZMLy8iViYzCPtZ/0TN/ufpQw98E5TjtvNkjMJkGdg2jkRRgPbtaHseKROALk5xz1Rzsoo5rC5w1yahzbuCQrmzIlg1hHRDLZ/U2rE45BZRXmxznlgPHeaOAs4Og20pGHLlCHenW07npLEkbL8M6oJtMKOWhNPaN87wkU2a6fSRIpoCtWDrSXcRuOOmi3WlrCt6z1cqAIELdIbFciNlPAvL6lHOtuZDsoSdmLhuyZYPVgUCov7tQj89hOvY8cBaIvSRbLhtTTJR5l9/XmAIv8ORAIES4QkYklIQH6ESdLc4NINgB89O5QajfF7PHCwMZdg2hmCWovYgIOSM0AqW+n84S8TAKeWuNcHthR63vk/T249fqmI0MfqZy5zc6ugdY/jH5j38M4Ae+d0k90kdVIwAAAABJRU5ErkJggg=="),
        // Please specify the base64 content of a 80x80 pixels image
        ExportMetadata("BigImageBase64", "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"),
        ExportMetadata("BackgroundColor", "Lavender"),
        ExportMetadata("PrimaryFontColor", "Black"),
        ExportMetadata("SecondaryFontColor", "Gray")]
    public class MyPlugin : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new MyPluginControl();
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        public MyPlugin()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(SecretKeeper.Syncfusion);
            
            // If you have external assemblies that you need to load, uncomment the following to 
            // hook into the event that will fire when an Assembly fails to resolve
            // AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveEventHandler);
        }

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
            Assembly currAssembly = Assembly.GetExecutingAssembly();

            // base name of the assembly that failed to resolve
            var argName = args.Name.Substring(0, args.Name.IndexOf(","));

            // check to see if the failing assembly is one that we reference.
            List<AssemblyName> refAssemblies = currAssembly.GetReferencedAssemblies().ToList();
            var refAssembly = refAssemblies.Where(a => a.Name == argName).FirstOrDefault();

            // if the current unresolved assembly is referenced by our plugin, attempt to load
            if (refAssembly != null)
            {
                // load from the path to this plugin assembly, not host executable
                string dir = Path.GetDirectoryName(currAssembly.Location).ToLower();
                string folder = Path.GetFileNameWithoutExtension(currAssembly.Location);
                dir = Path.Combine(dir, folder);

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