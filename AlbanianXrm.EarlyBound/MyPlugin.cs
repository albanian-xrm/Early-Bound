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
        ExportMetadata("SmallImageBase64", "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAX3SURBVFhHvVdbUFVVGP4I5WaSgVy8oAgWoiQKmqSUpKUoD6ZNaWFTQ9rN6d7kS15yfAhrzLGZFB8sLSfDmQaaVEy6qY04CSEiauaAigSjUZjKJfD0fay9p30Oh+PhxW9mzdl7r/V/61//dZ0ALMEMAFs5RnLcOrhQhwA8KwXqEMHNQ60JPxHAkT2EPC5gX6P4+ohWjmbU3cYfs7kY+zCWjgY2TwYK7gWWJHpf43OEcADxUsBvDOwP9LckEm8HfvmToxkYPdB805zW9AV+KxBO4qJMYMsk8/7haSArhiMKWM9noYBzRdOoRD/z7g98KiBLxRhTobULuNwOTImkECca2+hCvjd3AE181rcMzl3iN60VJCsOX/CpwLK7gM8ygId40n9vALmHDeGiEWbeCX1TIOaWAZ18eDjWyIrDF3wqUPkXEMcAnRxh3kX81UVg4p3m3Ql9K+Jcl5UOk/guWXH4gpsC99OfTfOA03OBUQOAQ5eBp44Ay5OB6dHWIl/5Zs1lca1kFtMa4kgglzgbya09nHBT4EWm1qazwLG/gbX3MFMCgaOM8td+BXZNNSMuzFrsBZrbxSAs5LpXKVPO04tDXJXk3Exu7eGEmwJ7GoA3k4BHhgFPsi6emAPMHw5sqwWS9gANLB7y9aCgIEvif+ib5i5eB+7m2u2Ukaw4niDXfHKKezf3cEKV0AVO2uGqU+jxmVHA22OAAUyp75uA1yuBKp4ie2gIdma0If1b4LssIzPzR6BiFrCwLAQlDW0YP4hpOhGYQVdc7QTWnTKHkIcuUMFuMKhBZXoo4MRwBlF+qjnBDUpvoQlXVvMUD5jAtEVErKKUcxBYkwI8x8qotPziHGPhGFCvsusJbwq8QRMp5cpIdp2aF14AzlPjqYOBDTyRNlWOh9Kv+q5TCU/TWiNoOXtO1VExcJgBOJIB+FgcEMbv97FO7Kc1uwuXpYBbDChgAqlIdDDTiJsdzwZW80QKoIxS4KVys8E5bp62z1hDI43ukEKae+GoWSt3SbZqNpDOlIxmURK31jjRwwX6sTNtbDhPngYkhwcw/4MQHtjeHRtrTgCruLETMv2KccCntMqVrmA8OqwDNS2u7gyquWLWOLm9WkAIsBQRpLFeVVxc6rsWHEu8QmslIy5x2HBy23CzwDtjTRE5RY2jaLJsltMP6K/3GcUqxcrhj2gRmVtZoN4gDKbLlAXKoGV0UwGDNYhHe4tZpNTTfUE9IpkW/YExsLaGQr0F4UwG4RErCHeeNxGs6qUgTKMvr/G7UlPp9AnNrUPJLdrcnqtgAZLpD15iJvG76oOCUM2q1CMIfaahIjh/PPA4CTopoEq2mr7fO90Elm1embuckT/nAPAuY+F5pmE/WqCQB1hexaC9Zta5obcYUDrFc2MRnWQVW8jN99OEExj1r1QwlaJCkcQLSMI3hriOI3E3MIbmzaAvXuYarS2ljGTFoWwQp7g94abAYqv8qnGsZETX08zzDgGzfzI9X/4vntaKwvqg7jjo4CkUG1Lky/pgFGe2YyPXaO0sykhWLlxFLnFWkzvX4+rrpsDcoeamU8y2+jmrWEoJ8DWf8xJIkMPAZLDpe0sHbyEeaOloxw7OqYZoszzGhWRT9qL7uzg3kDuHezjhpsCm343/VMtXHDcnVPVbP4HNhKdZxAuJmk1v0JzWLPiZMgzadMqKQ1yp5FSJ/viMtdiCmwKK2phi+pPdTL7NZAneNgV476SZE7zlsg177gDX5lNmO2XFUUsuddNYcut+4ESPIHQilZGudNOdQNCtdwFbbIX17oS+qY3bmSEZWUQcvuAzDQX5XUVEm+/gHW/cHcavqou/0deC+r+6n3pHdYu5CSk4bVmv4LzXNPSETaAmEsF7iDqlNh/CVh3JDSI4Ylk11a7LaF6tCbFYe93cgZtawIkwVrl2tlwVnnW8JyTwz4lw9qrp+zK/Oqoq4k3hrwWcUHm2b71n/jEZoqFnQXN+be4A/5+iFpGIt/6r9Qn6cyqU/GF++wTrz2kAluJBmmMrVYk3M7cILh4cyPsPv6oTvtw+B9AAAAAASUVORK5CYII="),
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