using AlbanianXrm.EarlyBound.Properties;
using Microsoft.Xrm.Sdk.Metadata;
using Syncfusion.Windows.Forms.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace AlbanianXrm.EarlyBound.Logic
{
    internal class EntitySelectionHandler
    {
        private readonly MyPluginControl myPlugin;
        private readonly TreeViewAdv metadataTree;
        private readonly AttributeMetadataHandler attributeMetadataHandler;
        private readonly RelationshipMetadataHandler relationshipMetadataHandler;

        public EntitySelectionHandler(MyPluginControl myPlugin, TreeViewAdv metadataTree, AttributeMetadataHandler attributeMetadataHandler, RelationshipMetadataHandler relationshipMetadataHandler)
        {
            this.myPlugin = myPlugin;
            this.metadataTree = metadataTree;
            this.attributeMetadataHandler = attributeMetadataHandler;
            this.relationshipMetadataHandler = relationshipMetadataHandler;
        }

        internal void SelectGenerated()
        {
            var options = this.myPlugin.options;
            myPlugin.StartWorkAsync(new WorkAsyncInfo
            {
                Message = Resources.SELECTING_GENERATED,
                AsyncArgument = (string.IsNullOrEmpty(options.CurrentOrganizationOptions.Output) ? "Test.cs" : Path.GetFullPath(options.CurrentOrganizationOptions.Output)) + ".alb",
                Work = (worker, args) =>
                {
                    ForrestSerializer forrestSerializer = new ForrestSerializer(args.Argument as string);
                    args.Result = forrestSerializer.Deserialize();
                },
                PostWorkCallBack = (args) =>
                {
                    try
                    {
                        if (args.Error != null)
                        {
                            MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (args.Result is Dictionary<string, EntitySelection> entitySelection)
                        {
                            if (entitySelection.Any())
                            {
                                foreach (TreeNodeAdv entityNode in metadataTree.Nodes)
                                {
                                    EntityMetadata entity = entityNode.Tag as EntityMetadata;
                                    if (entitySelection.TryGetValue(entity.LogicalName, out EntitySelection thisSelection))
                                    {
                                        foreach (TreeNodeAdv node in entityNode.Nodes)
                                        {
                                            if (node.Text == "Attributes")
                                            {
                                                if (thisSelection.AllAttributes)
                                                {
                                                    node.Checked = true;
                                                }
                                                else if (thisSelection.SelectedAttributes.Any())
                                                {
                                                    attributeMetadataHandler.GetAttributes(entity.LogicalName, node, false, thisSelection.SelectedAttributes);
                                                }
                                            }
                                            else if (node.Text == "Relationships")
                                            {
                                                if (thisSelection.AllRelationships)
                                                {
                                                    node.Checked = true;
                                                }
                                                else if (thisSelection.SelectedRelationships.Any())
                                                {
                                                    relationshipMetadataHandler.GetRelationships(entity.LogicalName, node, false, thisSelection.SelectedRelationships);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
#pragma warning disable CA1031 // We don't want our plugin to crash because of unhandled exceptions
                    catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                    {
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        myPlugin.WorkAsyncEnded();
                    }
                }
            });
        }
    }
}
