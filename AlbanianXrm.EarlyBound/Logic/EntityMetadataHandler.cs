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
                    if (args.Result is RetrieveAllEntitiesResponse result)
                    {
                        metadataTree.Enabled = true;
                        metadataTree.Nodes.Clear();
                        foreach (var item in result.EntityMetadata.OrderBy(x => x.LogicalName))
                        {
                            if (item.DisplayName.LocalizedLabels.Count == 0) continue;

                            TreeNodeAdv attributes = new TreeNodeAdv("Attributes")
                            {
                                ShowCheckBox = true,
                                InteractiveCheckBox = true
                            };

                            TreeNodeAdv relationships = new TreeNodeAdv("Relationships")
                            {
                                ShowCheckBox = true,
                                InteractiveCheckBox = true
                            };

                            TreeNodeAdv node = new TreeNodeAdv($"{item.LogicalName}: {item.DisplayName.LocalizedLabels[0].Label}",
                                new TreeNodeAdv[] { attributes, relationships })
                            {
                                ExpandedOnce = true,
                                ShowCheckBox = true,
                                InteractiveCheckBox = true,
                                Tag = item
                            };

                            metadataTree.Nodes.Add(node);
                        }
                    }
                }
            });
        }
    }
}
