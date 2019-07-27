using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Syncfusion.Windows.Forms.Tools;
using System.Linq;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace AlbanianXrm.EarlyBound.Logic
{
    internal class AttributeMetadataHandler
    {
        MyPluginControl myPlugin;

        public AttributeMetadataHandler(MyPluginControl myPlugin)
        {
            this.myPlugin = myPlugin;
        }

        public void GetAttributes(string entityName, TreeNodeAdv attributesNode)
        {
            myPlugin.WorkAsync(new WorkAsyncInfo
            {
                Message = $"Getting attributes for entity {entityName}",
                Work = (worker, args) =>
                {
                    args.Result = myPlugin.Service.Execute(new RetrieveEntityRequest()
                    {
                        EntityFilters = EntityFilters.Attributes,
                        LogicalName = entityName
                    });
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    if (args.Result is RetrieveEntityResponse result)
                    {
                        foreach (var item in result.EntityMetadata.Attributes.OrderBy(x => x.LogicalName))
                        {
                            var name = item.DisplayName.LocalizedLabels.Count == 0 ? item.LogicalName : item.DisplayName.LocalizedLabels[0].Label;

                            TreeNodeAdv node = new TreeNodeAdv($"{item.LogicalName}: {name}")
                            {
                                ExpandedOnce = true,
                                ShowCheckBox = true,
                                Tag = item
                            };

                            attributesNode.Nodes.Add(node);
                        }
                    }
                }
            });
        }
    }
}
