using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xrm.Sdk.Metadata;
using Syncfusion.Windows.Forms.Tools;
using System.IO;
using System.Diagnostics;
using AlbanianXrm.EarlyBound.Extensions;
using AlbanianXrm.EarlyBound.Helpers;
using System;
using AlbanianXrm.Common.Shared;
using System.Runtime.Serialization;
using System.Xml;
using System.Drawing;
using AlbanianXrm.EarlyBound.Properties;
using System.Globalization;
using AlbanianXrm.BackgroundWorker;
using AlbanianXrm.XrmToolBox.Shared.Extensions;

namespace AlbanianXrm.EarlyBound.Logic
{
    internal class EntityGeneratorHandler
    {
        readonly MyPluginControl myPlugin;
        readonly AlBackgroundWorkHandler backgroundWorkHandler;
        readonly TreeViewAdv metadataTree;
        readonly RichTextBox output;

        public EntityGeneratorHandler(MyPluginControl myPlugin, AlBackgroundWorkHandler backgroundWorkHandler, TreeViewAdv metadataTree, RichTextBox output)
        {
            this.myPlugin = myPlugin;
            this.backgroundWorkHandler = backgroundWorkHandler;
            this.metadataTree = metadataTree;
            this.output = output;
        }

        public void GenerateEntities(Options options)
        {
            output.ResetText();
            backgroundWorkHandler.EnqueueBackgroundWork(
                AlBackgroundWorkerFactory.NewWorker<Options, string, string>(
                GenerateEntitiesInner,
                options,
                Progress,
                GenerateEntitiesEnd
                ).WithMessage(myPlugin, Resources.GENERATING_ENTITIES));
        }

        private string GenerateEntitiesInner(Options options, Reporter<string> reporter)
        {
            string dir = Path.GetDirectoryName(typeof(MyPluginControl).Assembly.Location).ToUpperInvariant();
            string folder = Path.GetFileNameWithoutExtension(typeof(MyPluginControl).Assembly.Location);
            dir = Path.Combine(dir, folder);

            if (!File.Exists(Path.Combine(dir, "CrmSvcUtil.exe")))
            {
                return Resources.CRMSVCUTIL_MISSING;
            }
            if (!File.Exists(Path.Combine(dir, "Microsoft.IO.RecyclableMemoryStream.dll"))) // specific version included with the plugin
            {
                return Resources.MEMORYSTREAM_MISSING;
            }
            Process process = new Process();
            var connectionString = myPlugin.ConnectionDetail.GetConnectionStringWithPassword();
            var argumentsWithoutConnectionString = (string.IsNullOrEmpty(options.CurrentOrganizationOptions.Namespace) ? "" : " /namespace:" + options.CurrentOrganizationOptions.Namespace) +
                                          " /codewriterfilter:AlbanianXrm.CrmSvcUtilExtensions.FilteringService,AlbanianXrm.CrmSvcUtilExtensions" +
                                          " /codecustomization:AlbanianXrm.CrmSvcUtilExtensions.CustomizationService,AlbanianXrm.CrmSvcUtilExtensions" +
                                          " /metadataproviderservice:AlbanianXrm.CrmSvcUtilExtensions.MetadataService,AlbanianXrm.CrmSvcUtilExtensions" +
                                          " /namingservice:AlbanianXrm.CrmSvcUtilExtensions.NamingService,AlbanianXrm.CrmSvcUtilExtensions" +
                                          " /out:" + (string.IsNullOrEmpty(options.CurrentOrganizationOptions.Output) ? "Test.cs" : "\"" + Path.GetFullPath(options.CurrentOrganizationOptions.Output) + "\"") +
                                          (options.CurrentOrganizationOptions.Language == Language.VB ? " /language:VB" : "") +
                                          (string.IsNullOrEmpty(options.CurrentOrganizationOptions.ServiceContextName) ? "" : " /serviceContextName:" + options.CurrentOrganizationOptions.ServiceContextName);

            process.StartInfo.Arguments = "/connectionstring:" + connectionString + argumentsWithoutConnectionString;

            process.StartInfo.WorkingDirectory = dir;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            HashSet<string> entities = new HashSet<string>();
            HashSet<string> relationshipEntities = new HashSet<string>();
            List<string> allAttributes = new List<string>();
            List<string> allRelationships = new List<string>();
            Dictionary<string, EntitySelection> entitySelections = new Dictionary<string, EntitySelection>();

            foreach (TreeNodeAdv entity in metadataTree.Nodes)
            {
                if (entity.CheckState != CheckState.Unchecked)
                {
                    EntityMetadata metadata = (EntityMetadata)entity.Tag;
                    entities.Add(metadata.LogicalName);
                    EntitySelection entitySelection = new EntitySelection(metadata.LogicalName);
                    entitySelections.Add(entitySelection.LogicalName, entitySelection);
                    foreach (TreeNodeAdv item in entity.Nodes)
                    {
                        if (item.Text == "Attributes")
                        {
                            if (item.CheckState == CheckState.Checked)
                            {
                                allAttributes.Add(metadata.LogicalName);
                                entitySelection.AllAttributes = true;
                            }
                            else if (item.CheckState == CheckState.Indeterminate)
                            {
                                List<string> attributes = new List<string>();
                                foreach (TreeNodeAdv attribute in item.Nodes)
                                {
                                    if (attribute.Checked)
                                    {
                                        var attributeMetadata = (AttributeMetadata)attribute.Tag;
                                        attributes.Add(attributeMetadata.LogicalName);
                                        entitySelection.SelectedAttributes.Add(attributeMetadata.LogicalName);
                                    }
                                }
                                process.StartInfo.EnvironmentVariables.Add(string.Format(CultureInfo.InvariantCulture, Constants.ENVIRONMENT_ENTITY_ATTRIBUTES, metadata.LogicalName), string.Join(",", attributes));
                            }
                        }
                        else if (item.Text == "Relationships")
                        {
                            if (item.CheckState == CheckState.Checked)
                            {
                                allRelationships.Add(metadata.LogicalName);
                                entitySelection.AllRelationships = true;
                                foreach (TreeNodeAdv relationship in item.Nodes)
                                {
                                    if (relationship.Tag is OneToManyRelationshipMetadata relationshipMetadataO)
                                    {
                                        if (!relationshipEntities.Contains(relationshipMetadataO.ReferencedEntity)) relationshipEntities.Add(relationshipMetadataO.ReferencedEntity);
                                        if (!relationshipEntities.Contains(relationshipMetadataO.ReferencingEntity)) relationshipEntities.Add(relationshipMetadataO.ReferencingEntity);
                                    }
                                    else if (relationship.Tag is ManyToManyRelationshipMetadata relationshipMetadataM)
                                    {
                                        if (!relationshipEntities.Contains(relationshipMetadataM.Entity1LogicalName)) relationshipEntities.Add(relationshipMetadataM.Entity1LogicalName);
                                        if (!relationshipEntities.Contains(relationshipMetadataM.Entity2LogicalName)) relationshipEntities.Add(relationshipMetadataM.Entity2LogicalName);
                                    }
                                }
                            }
                            else if (item.CheckState == CheckState.Indeterminate)
                            {
                                List<string> relationships1N = new List<string>();
                                List<string> relationshipsN1 = new List<string>();
                                List<string> relationshipsNN = new List<string>();
                                foreach (TreeNodeAdv relationship in item.Nodes)
                                {
                                    if (relationship.Checked)
                                    {
                                        if (relationship.Tag is OneToManyRelationshipMetadata relationshipMetadata)
                                        {
                                            entitySelection.SelectedRelationships.Add(relationshipMetadata.SchemaName);
                                            if (relationshipMetadata.ReferencingEntity == metadata.LogicalName)
                                            {
                                                relationshipsN1.Add(relationshipMetadata.SchemaName);
                                            }
                                            else
                                            {
                                                relationships1N.Add(relationshipMetadata.SchemaName);
                                            }
                                            if (!relationshipEntities.Contains(relationshipMetadata.ReferencedEntity)) relationshipEntities.Add(relationshipMetadata.ReferencedEntity);
                                            if (!relationshipEntities.Contains(relationshipMetadata.ReferencingEntity)) relationshipEntities.Add(relationshipMetadata.ReferencingEntity);
                                        }
                                        else if (relationship.Tag is ManyToManyRelationshipMetadata relationshipMetadataM)
                                        {
                                            entitySelection.SelectedRelationships.Add(relationshipMetadataM.SchemaName);
                                            relationshipsNN.Add(relationshipMetadataM.SchemaName);
                                            if (!relationshipEntities.Contains(relationshipMetadataM.Entity1LogicalName)) relationshipEntities.Add(relationshipMetadataM.Entity1LogicalName);
                                            if (!relationshipEntities.Contains(relationshipMetadataM.Entity2LogicalName)) relationshipEntities.Add(relationshipMetadataM.Entity2LogicalName);

                                        }
                                    }
                                }
                                if (relationships1N.Any()) process.StartInfo.EnvironmentVariables.Add(string.Format(CultureInfo.InvariantCulture, Constants.ENVIRONMENT_RELATIONSHIPS1N, metadata.LogicalName), string.Join(",", relationships1N.Distinct()));
                                if (relationshipsN1.Any()) process.StartInfo.EnvironmentVariables.Add(string.Format(CultureInfo.InvariantCulture, Constants.ENVIRONMENT_RELATIONSHIPSN1, metadata.LogicalName), string.Join(",", relationshipsN1.Distinct()));
                                if (relationshipsNN.Any()) process.StartInfo.EnvironmentVariables.Add(string.Format(CultureInfo.InvariantCulture, Constants.ENVIRONMENT_RELATIONSHIPSNN, metadata.LogicalName), string.Join(",", relationshipsNN.Distinct()));
                            }
                        }
                    }
                }
            }

            if (entities.Any()) process.StartInfo.EnvironmentVariables.Add(Constants.ENVIRONMENT_ENTITIES, string.Join(",", entities));
            if (allAttributes.Any()) process.StartInfo.EnvironmentVariables.Add(Constants.ENVIRONMENT_ALL_ATTRIBUTES, string.Join(",", allAttributes));
            if (allRelationships.Any()) process.StartInfo.EnvironmentVariables.Add(Constants.ENVIRONMENT_ALL_RELATIONSHIPS, string.Join(",", allRelationships));
            if (options.CurrentOrganizationOptions.RemovePropertyChanged) process.StartInfo.EnvironmentVariables.Add(Constants.ENVIRONMENT_REMOVEPROPERTYCHANGED, "YES");
            if (options.CurrentOrganizationOptions.RemoveProxyTypesAssembly) process.StartInfo.EnvironmentVariables.Add(Constants.ENVIRONMENT_REMOVEPROXYTYPESASSEMBLY, "YES");
            if (options.CurrentOrganizationOptions.RemovePublisherPrefix) process.StartInfo.EnvironmentVariables.Add(Constants.ENVIRONMENT_REMOVEPUBLISHER, "YES");
            if (options.CurrentOrganizationOptions.OptionSetEnums) process.StartInfo.EnvironmentVariables.Add(Constants.ENVIRONMENT_OPTIONSETENUMS, "YES");
            if (options.CurrentOrganizationOptions.OptionSetEnumProperties) process.StartInfo.EnvironmentVariables.Add(Constants.ENVIRONMENT_OPTIONSETENUMPROPERTIES, "YES");
            if (options.CurrentOrganizationOptions.GenerateXmlDocumentation) process.StartInfo.EnvironmentVariables.Add(Constants.ENVIRONMENT_GENERATEXML, "YES");
            if (options.CurrentOrganizationOptions.GenerateAttributeConstants) process.StartInfo.EnvironmentVariables.Add(Constants.ENVIRONMENT_ATTRIBUTECONSTANTS, "YES");
            process.StartInfo.EnvironmentVariables.Add(Constants.ENVIRONMENT_TWOOPTIONS, ((int)options.CurrentOrganizationOptions.TwoOptions).ToString(CultureInfo.InvariantCulture));
#if DEBUG
            if (options.LaunchDebugger) process.StartInfo.EnvironmentVariables.Add(Constants.ENVIRONMENT_ATTACHDEBUGGER, "YES");
#endif
            myPlugin.pluginViewModel.LaunchCommand = $"{string.Join("\r\n", process.StartInfo.EnvironmentVariables.ToEnumerable().Where(x => x.Key.StartsWith(Constants.ENVIRONMENT_VARIABLE_PREFIX)).Select(x => $"SET {x.Key}={x.Value}"))}\r\n{Path.Combine(dir, "CrmSvcUtil.exe")} /connectionstring:{myPlugin.ConnectionDetail.GetConnectionStringWithoutPassword()}{argumentsWithoutConnectionString}";
            if (options.CacheMetadata) process.StartInfo.EnvironmentVariables.Add(Constants.ENVIRONMENT_CACHEMEATADATA, "YES");

            process.EnableRaisingEvents = true;
            process.StartInfo.FileName = Path.Combine(dir, "CrmSvcUtil.exe");
            ForrestSerializer serializer = new ForrestSerializer((string.IsNullOrEmpty(options.CurrentOrganizationOptions.Output) ? "Test.cs" : Path.GetFullPath(options.CurrentOrganizationOptions.Output)) + ".alb");
            serializer.Serialize(entitySelections);
            process.Start();

            while (!process.StandardOutput.EndOfStream)
            {
                var line = process.StandardOutput.ReadLine();
                if (line.EndsWith(Constants.CONSOLE_METADATA, StringComparison.InvariantCulture))
                {
                    line = TrimEnd(line, Constants.CONSOLE_METADATA);
                    SerializeMetadata(process.StandardInput, entities, relationshipEntities);
                }
                if (line != null)
                {
                    reporter.ReportProgress(0, line);
                }
            }
            while (!process.StandardError.EndOfStream)
            {
                reporter.ReportProgress(50, process.StandardError.ReadLine());
            }
            process.WaitForExit();
            reporter.ReportProgress(100, "Ended");
            return null;
        }

        private void Progress(int percentage, string userState)
        {
            if (percentage == 0)
            {
                output.AppendText(userState + Environment.NewLine);
            }
            else if (percentage == 50)
            {
                output.AppendText(userState + Environment.NewLine, Color.Red);
            }
            else if (percentage == 100)
            {
                Debug.WriteLine(userState);
            }
        }

        private void GenerateEntitiesEnd(Options input, string value, Exception exception)
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SerializeMetadata(StreamWriter standardInput, HashSet<string> entities, HashSet<string> relationshipEntities)
        {
            EntityMetadata[] entityMetadatas = myPlugin.entityMetadatas;
            if (entities.Any())
            {
                entityMetadatas = entityMetadatas.Where(x => entities.Contains(x.LogicalName) || relationshipEntities.Contains(x.LogicalName)).ToArray();
            }
            var metadata = new OrganizationMetadata(entityMetadatas, myPlugin.optionSetMetadatas);
            var serializer = new DataContractSerializer(typeof(OrganizationMetadata));
            using (XmlTextWriter writer = new XmlTextWriter(new StreamWriter(standardInput.BaseStream, standardInput.Encoding, 128, true)))
                serializer.WriteObject(writer, metadata);
            standardInput.WriteLine();
            standardInput.WriteLine(Constants.CONSOLE_ENDSTREAM);
        }

        private static string TrimEnd(string line, string keyword)
        {
            return line.Length == keyword.Length ?
                        null :
                        line.Substring(0, line.Length - Constants.CONSOLE_METADATA.Length);
        }
    }
}
