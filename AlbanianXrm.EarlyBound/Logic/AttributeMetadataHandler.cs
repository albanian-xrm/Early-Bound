using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Syncfusion.Windows.Forms.Tools;
using System;
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
            myPlugin.StartWorkAsync(new WorkAsyncInfo
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
                    try
                    {
                        if (args.Error != null)
                        {
                            MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        if (args.Result is RetrieveEntityResponse result)
                        {
                            attributesNode.ExpandedOnce = true;
                            var entityMetadata = myPlugin.entityMetadatas.FirstOrDefault(x => x.LogicalName == entityName);
                            typeof(EntityMetadata).GetProperty(nameof(entityMetadata.Attributes)).SetValue(entityMetadata, result.EntityMetadata.Attributes);
                            foreach (var item in result.EntityMetadata.Attributes.OrderBy(x => x.LogicalName))
                            {
                                if (!item.DisplayName.LocalizedLabels.Any()) continue;
                                var name = item.DisplayName.LocalizedLabels.First().Label;

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
                    catch (Exception ex)
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
