using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using Microsoft.Xrm.Sdk.Metadata;
using Syncfusion.Windows.Forms.Tools;
using System.IO;
using System.Diagnostics;
using AlbanianXrm.EarlyBound.Extensions;
using AlbanianXrm.EarlyBound.Helpers;

namespace AlbanianXrm.EarlyBound.Logic
{
    internal class EntityGeneratorHandler
    {
        MyPluginControl myPlugin;
        TreeViewAdv metadataTree;
        TextBoxExt output;

        public EntityGeneratorHandler(MyPluginControl myPlugin, TreeViewAdv metadataTree, TextBoxExt output)
        {
            this.myPlugin = myPlugin;
            this.metadataTree = metadataTree;
            this.output = output;
        }

        public void GenerateEntities(Options options)
        {
            myPlugin.pluginViewModel.AllowRequests = false;
            myPlugin.WorkAsync(new WorkAsyncInfo()
            {
                Message = $"Generating Early-Bound Classes",
                Work = (worker, args) =>
                {
                    string dir = Path.GetDirectoryName(typeof(MyPluginControl).Assembly.Location).ToLower();
                    string folder = Path.GetFileNameWithoutExtension(typeof(MyPluginControl).Assembly.Location);
                    dir = Path.Combine(dir, folder);
                    Process process = new Process();
                    var connectionString = myPlugin.ConnectionDetail.GetConnectionStringWithPassword();
                    process.StartInfo.Arguments = "/connectionstring:" + connectionString +
                                                  (string.IsNullOrEmpty(options.CurrentOrganizationOptions.Namespace) ? "" : " /namespace:" + options.CurrentOrganizationOptions.Namespace) +
                                                  " /codewriterfilter:AlbanianXrm.CrmSvcUtilExtensions.FilteringService,AlbanianXrm.CrmSvcUtilExtensions" +
                                                  " /out:" + (string.IsNullOrEmpty(options.CurrentOrganizationOptions.Output) ? "Test.cs" : "\"" + Path.GetFullPath(options.CurrentOrganizationOptions.Output) + "\"") + 
                                                  (options.CurrentOrganizationOptions.Language == LanguageEnum.VB ? " /language:VB" : "") +
                                                  (string.IsNullOrEmpty(options.CurrentOrganizationOptions.ServiceContextName) ? "" : " /serviceContextName:" + options.CurrentOrganizationOptions.ServiceContextName);
                    process.StartInfo.WorkingDirectory = dir;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;

                    List<string> entities = new List<string>();
                    List<string> allAttributes = new List<string>();
                    List<string> allRelationships = new List<string>();

                    foreach (TreeNodeAdv entity in metadataTree.Nodes)
                    {
                        if (entity.CheckState != CheckState.Unchecked)
                        {
                            EntityMetadata metadata = (EntityMetadata)entity.Tag;
                            entities.Add(metadata.LogicalName);
                            foreach (TreeNodeAdv item in entity.Nodes)
                            {
                                if (item.Text == "Attributes")
                                {
                                    if (item.CheckState == CheckState.Checked)
                                    {
                                        allAttributes.Add(metadata.LogicalName);
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
                                            }
                                        }
                                        process.StartInfo.EnvironmentVariables.Add(string.Format(Constants.ENVIRONMENT_ENTITY_ATTRIBUTES, metadata.LogicalName), string.Join(",", attributes));
                                    }
                                }
                                else if (item.Text == "Relationships")
                                {
                                    if (item.CheckState == CheckState.Checked)
                                    {
                                        allRelationships.Add(metadata.LogicalName);
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
                                                if (relationship.Tag is OneToManyRelationshipMetadata)
                                                {
                                                    var relationshipMetadata = (OneToManyRelationshipMetadata)relationship.Tag;
                                                    if (relationshipMetadata.ReferencingEntity == metadata.LogicalName)
                                                    {
                                                        relationshipsN1.Add(relationshipMetadata.SchemaName);
                                                    }
                                                    else
                                                    {
                                                        relationships1N.Add(relationshipMetadata.SchemaName);
                                                    }
                                                }
                                                else
                                                {
                                                    var relationshipMetadata = (RelationshipMetadataBase)relationship.Tag;
                                                    relationshipsNN.Add(relationshipMetadata.SchemaName);
                                                }
                                            }
                                        }
                                        if (relationships1N.Any()) process.StartInfo.EnvironmentVariables.Add(string.Format(Constants.ENVIRONMENT_RELATIONSHIPS1N, metadata.LogicalName), string.Join(",", relationships1N.Distinct()));
                                        if (relationshipsN1.Any()) process.StartInfo.EnvironmentVariables.Add(string.Format(Constants.ENVIRONMENT_RELATIONSHIPSN1, metadata.LogicalName), string.Join(",", relationshipsN1.Distinct()));
                                        if (relationshipsNN.Any()) process.StartInfo.EnvironmentVariables.Add(string.Format(Constants.ENVIRONMENT_RELATIONSHIPSNN, metadata.LogicalName), string.Join(",", relationshipsNN.Distinct()));
                                    }
                                }
                            }
                        }
                    }

                    if (entities.Any()) process.StartInfo.EnvironmentVariables.Add(Constants.ENVIRONMENT_ENTITIES, string.Join(",", entities));
                    if (allAttributes.Any()) process.StartInfo.EnvironmentVariables.Add(Constants.ENVIRONMENT_ALL_ATTRIBUTES, string.Join(",", allAttributes));
                    if (allRelationships.Any()) process.StartInfo.EnvironmentVariables.Add(Constants.ENVIRONMENT_ALL_RELATIONSHIPS, string.Join(",", allRelationships));
                    process.EnableRaisingEvents = true;
                    process.StartInfo.FileName = Path.Combine(dir, "CrmSvcUtil.exe");
                    process.Start();
                    process.WaitForExit();
                    args.Result = process.StandardOutput.ReadToEnd();
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    output.Text = (string)args.Result;
                    myPlugin.pluginViewModel.AllowRequests = true;
                }
            });

        }
    }
}
