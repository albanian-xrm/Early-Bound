using AlbanianXrm.EarlyBound.Extensions;
using AlbanianXrm.EarlyBound.Properties;
using AlbanianXrm.XrmToolBox.Shared;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Syncfusion.Windows.Forms.Tools;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace AlbanianXrm.EarlyBound.Logic
{
    internal class RelationshipMetadataHandler
    {
        private readonly MyPluginControl myPlugin;
        private readonly BackgroundWorkHandler backgroundWorkHandler;

        public RelationshipMetadataHandler(MyPluginControl myPlugin, BackgroundWorkHandler backgroundWorkHandler)
        {
            this.myPlugin = myPlugin;
            this.backgroundWorkHandler = backgroundWorkHandler;
        }

        public void GetRelationships(string entityName, TreeNodeAdv relationshipsNode, bool checkedState = false, HashSet<string> checkedRelationships = default(HashSet<string>))
        {
            backgroundWorkHandler.EnqueueWork(
                string.Format(CultureInfo.CurrentCulture, Resources.GETTING_RELATIONSHIPS, entityName),
                GetRelationships,
               new Tuple<string, TreeNodeAdv, bool, HashSet<string>>(entityName, relationshipsNode, checkedState, checkedRelationships),
               GetRelationships);
        }

        private RetrieveEntityResponse GetRelationships(Tuple<string, TreeNodeAdv, bool, HashSet<string>> args)
        {
            return myPlugin.Service.Execute(new RetrieveEntityRequest()
            {
                EntityFilters = EntityFilters.Relationships,
                LogicalName = args.Item1 //entityName
            }) as RetrieveEntityResponse;
        }

        private void GetRelationships(BackgroundWorkResult<Tuple<string, TreeNodeAdv, bool, HashSet<string>>, RetrieveEntityResponse> args)
        {
            try
            {
                if (args.Exception != null)
                {
                    MessageBox.Show(args.Exception.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (args.Value is RetrieveEntityResponse result)
                {
                    var checkedRelationships = args.Argument.Item4;
                    var entityName = args.Argument.Item1;
                    var checkedState = args.Argument.Item3;
                    var relationshipsNode = args.Argument.Item2;
                    if (checkedRelationships == null) checkedRelationships = new HashSet<string>();

                    var entityMetadata = myPlugin.entityMetadatas.FirstOrDefault(x => x.LogicalName == entityName);
                    entityMetadata.SetPrivateValue(x => x.ManyToManyRelationships, result.EntityMetadata.ManyToManyRelationships);
                    entityMetadata.SetPrivateValue(x => x.OneToManyRelationships, result.EntityMetadata.OneToManyRelationships);
                    entityMetadata.SetPrivateValue(x => x.ManyToOneRelationships, result.EntityMetadata.ManyToOneRelationships);

                    CreateRelationshipNodes(relationshipsNode, result.EntityMetadata, checkedState, checkedRelationships);
                }
            }
#pragma warning disable CA1031 // We don't want our plugin to crash because of unhandled exceptions
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void CreateRelationshipNodes(TreeNodeAdv relationshipsNode, EntityMetadata entityMetadata, bool checkedState = false, HashSet<string> checkedRelationships = default(HashSet<string>))
        {
            relationshipsNode.ExpandedOnce = true;
            foreach (var item in entityMetadata.ManyToManyRelationships.Union<RelationshipMetadataBase>(
                                 entityMetadata.OneToManyRelationships).Union(
                                 entityMetadata.ManyToOneRelationships).OrderBy(x => x.SchemaName))
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
            if (entityMetadata.ManyToManyRelationships.Length == 0 &&
                entityMetadata.OneToManyRelationships.Length == 0 &&
                entityMetadata.ManyToOneRelationships.Length == 0)
            {
                relationshipsNode.Checked = checkedState;
            }
        }
    }
}
