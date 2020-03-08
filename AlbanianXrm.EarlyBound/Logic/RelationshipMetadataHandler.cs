using AlbanianXrm.EarlyBound.Extensions;
using AlbanianXrm.EarlyBound.Properties;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Syncfusion.Windows.Forms.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace AlbanianXrm.EarlyBound.Logic
{
    internal class RelationshipMetadataHandler
    {
        private readonly MyPluginControl myPlugin;

        public RelationshipMetadataHandler(MyPluginControl myPlugin)
        {
            this.myPlugin = myPlugin;
        }

        public void GetRelationships(string entityName, TreeNodeAdv relationshipsNode, bool checkedState = false, HashSet<string> checkedRelationships = default(HashSet<string>))
        {
            myPlugin.StartWorkAsync(new WorkAsyncInfo
            {
                Message = string.Format(CultureInfo.CurrentCulture, Resources.GETTING_RELATIONSHIPS, entityName),
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
                    try
                    {
                        if (args.Error != null)
                        {
                            MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        if (args.Result is RetrieveEntityResponse result)
                        {
                            if (checkedRelationships == null) checkedRelationships = new HashSet<string>();
                            relationshipsNode.ExpandedOnce = true;
                            var entityMetadata = myPlugin.entityMetadatas.FirstOrDefault(x => x.LogicalName == entityName);
                            entityMetadata.SetPrivateValue(x => x.ManyToManyRelationships, result.EntityMetadata.ManyToManyRelationships);
                            entityMetadata.SetPrivateValue(x => x.OneToManyRelationships, result.EntityMetadata.OneToManyRelationships);
                            entityMetadata.SetPrivateValue(x => x.ManyToOneRelationships, result.EntityMetadata.ManyToOneRelationships);

                            foreach (var item in result.EntityMetadata.ManyToManyRelationships.Union<RelationshipMetadataBase>(
                                                 result.EntityMetadata.OneToManyRelationships).Union(
                                                 result.EntityMetadata.ManyToOneRelationships).OrderBy(x => x.SchemaName))
                            {
                                TreeNodeAdv node = new TreeNodeAdv($"{item.SchemaName}")
                                {
                                    ExpandedOnce = true,
                                    ShowCheckBox = true,
                                    Tag = item,
                                    Checked = checkedState || checkedRelationships.Contains(item.SchemaName)
                                };

                                relationshipsNode.Nodes.Add(node);
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
