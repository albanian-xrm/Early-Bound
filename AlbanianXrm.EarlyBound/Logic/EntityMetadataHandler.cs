using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Syncfusion.Windows.Forms.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace AlbanianXrm.EarlyBound.Logic
{
    internal class EntityMetadataHandler
    {
        MyPluginControl myPlugin;
        TreeViewAdv metadataTree;

        public EntityMetadataHandler(MyPluginControl myPlugin, TreeViewAdv metadataTree)
        {
            this.myPlugin = myPlugin;
            this.metadataTree = metadataTree;
        }

        public void GetEntityList()
        {
            myPlugin.WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting entity list",
                Work = (worker, args) =>
                {
                    args.Result = myPlugin.Service.Execute(new RetrieveAllEntitiesRequest()
                    {
                        EntityFilters = EntityFilters.Entity
                    });
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    var result = args.Result as RetrieveAllEntitiesResponse;
                    if (result != null)
                    {
                        metadataTree.Enabled = true;
                        metadataTree.Nodes.Clear();
                        foreach (var item in result.EntityMetadata.OrderBy(x => x.LogicalName))
                        {
                            if (item.DisplayName.LocalizedLabels.Count == 0) continue;

                            TreeNodeAdv attributes = new TreeNodeAdv("Attributes");
                            attributes.ShowCheckBox = true;
                            attributes.InteractiveCheckBox = true;

                            TreeNodeAdv relationships = new TreeNodeAdv("Relationships");
                            relationships.ShowCheckBox = true;
                            relationships.InteractiveCheckBox = true;

                            TreeNodeAdv node = new TreeNodeAdv($"{item.LogicalName}: {item.DisplayName.LocalizedLabels[0].Label}",
                                new TreeNodeAdv[] { attributes, relationships });
                            node.ExpandedOnce = true;
                            node.ShowCheckBox = true;
                            node.InteractiveCheckBox = true;
                            node.Tag = item;

                            metadataTree.Nodes.Add(node);
                        }
                    }
                }
            });
        }
    }
}
