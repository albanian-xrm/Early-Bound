using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Syncfusion.Windows.Forms.Tools;
using System.Linq;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace AlbanianXrm.EarlyBound.Logic
{
    internal class RelationshipMetadataHandler
    {
        MyPluginControl myPlugin;

        public RelationshipMetadataHandler(MyPluginControl myPlugin)
        {
            this.myPlugin = myPlugin;
        }

        public void GetRelationships(string entityName, TreeNodeAdv relationshipsNode)
        {
            myPlugin.WorkAsync(new WorkAsyncInfo
            {
                Message = $"Getting relationships for entity {entityName}",
                Work = (worker, args) =>
                {
                    args.Result = myPlugin.Service.Execute(new RetrieveEntityRequest()
                    {
                        EntityFilters = EntityFilters.Relationships,
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
                        foreach (var item in result.EntityMetadata.ManyToManyRelationships.Union<RelationshipMetadataBase>(
                                             result.EntityMetadata.OneToManyRelationships).Union(
                                             result.EntityMetadata.ManyToOneRelationships).OrderBy(x => x.SchemaName))
                        {
                            TreeNodeAdv node = new TreeNodeAdv($"{item.SchemaName}")
                            {
                                ExpandedOnce = true,
                                ShowCheckBox = true,
                                Tag = item
                            };

                            relationshipsNode.Nodes.Add(node);
                        }
                    }
                }
            });
        }
    }
}
