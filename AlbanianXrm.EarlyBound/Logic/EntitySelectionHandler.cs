using AlbanianXrm.EarlyBound.Properties;
using AlbanianXrm.XrmToolBox.Shared;
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
        BackgroundWorkHandler backgroundWorkHandler;
        private readonly TreeViewAdv metadataTree;
        private readonly AttributeMetadataHandler attributeMetadataHandler;
        private readonly RelationshipMetadataHandler relationshipMetadataHandler;

        public EntitySelectionHandler(MyPluginControl myPlugin, BackgroundWorkHandler backgroundWorkHandler, TreeViewAdv metadataTree, AttributeMetadataHandler attributeMetadataHandler, RelationshipMetadataHandler relationshipMetadataHandler)
        {
            this.myPlugin = myPlugin;
            this.backgroundWorkHandler = backgroundWorkHandler;
            this.metadataTree = metadataTree;
            this.attributeMetadataHandler = attributeMetadataHandler;
            this.relationshipMetadataHandler = relationshipMetadataHandler;
        }

        internal void SelectGenerated()
        {
            var options = this.myPlugin.options;
            backgroundWorkHandler.EnqueueWork(
                Resources.SELECTING_GENERATED, Deserialize, 
                (string.IsNullOrEmpty(options.CurrentOrganizationOptions.Output) ? "Test.cs" : Path.GetFullPath(options.CurrentOrganizationOptions.Output)) + ".alb",
                SelectEntities);
        }

        private Dictionary<string, EntitySelection> Deserialize(string path)
        {
            ForrestSerializer forrestSerializer = new ForrestSerializer(path);
            return forrestSerializer.Deserialize();
        }

        public void SelectEntities(BackgroundWorkResult<string, Dictionary<string, EntitySelection>> args)
        {
            try
            {
                if (args.Exception != null)
                {
                    MessageBox.Show(args.Exception.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (args.Value is Dictionary<string, EntitySelection> entitySelection && entitySelection.Any())
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
                                    else if (thisSelection.SelectedAttributes.Any() && !node.ExpandedOnce)
                                    {
                                        attributeMetadataHandler.GetAttributes(entity.LogicalName, node, false, thisSelection.SelectedAttributes);
                                    }
                                    else if (thisSelection.SelectedAttributes.Any() && node.ExpandedOnce)
                                    {
                                        foreach (TreeNodeAdv attributeNode in node.Nodes)
                                        {
                                            if (attributeNode.Tag is AttributeMetadata attribute)
                                            {
                                                attributeNode.Checked = thisSelection.SelectedAttributes.Contains(attribute.LogicalName);
                                            }
                                        }
                                    }
                                }
                                else if (node.Text == "Relationships")
                                {
                                    if (thisSelection.AllRelationships)
                                    {
                                        node.Checked = true;
                                    }
                                    else if (thisSelection.SelectedRelationships.Any() && !node.ExpandedOnce)
                                    {
                                        relationshipMetadataHandler.GetRelationships(entity.LogicalName, node, false, thisSelection.SelectedRelationships);
                                    }
                                    else if (thisSelection.SelectedRelationships.Any() && node.ExpandedOnce)
                                    {
                                        foreach (TreeNodeAdv relationshipNode in node.Nodes)
                                        {
                                            if (relationshipNode.Tag is RelationshipMetadataBase relationshipMetadata)
                                            {
                                                relationshipNode.Checked = thisSelection.SelectedRelationships.Contains(relationshipMetadata.SchemaName);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }         
        }
    }
}
