using AlbanianXrm.BackgroundWorker;
using AlbanianXrm.EarlyBound.Extensions;
using AlbanianXrm.EarlyBound.Properties;
using AlbanianXrm.XrmToolBox.Shared.Extensions;
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
        private readonly AlBackgroundWorkHandler backgroundWorkHandler;

        public RelationshipMetadataHandler(MyPluginControl myPlugin, AlBackgroundWorkHandler backgroundWorkHandler)
        {
            this.myPlugin = myPlugin;
            this.backgroundWorkHandler = backgroundWorkHandler;
        }

        public void GetRelationships(string entityName, TreeNodeAdv relationshipsNode, bool checkedState = false, HashSet<string> checkedRelationships = default)
        {
            backgroundWorkHandler.EnqueueBackgroundWork(
                AlBackgroundWorkerFactory.NewWorker(
                GetRelationships,
               new Tuple<string, TreeNodeAdv, bool, HashSet<string>>(entityName, relationshipsNode, checkedState, checkedRelationships),
               GetRelationships).WithViewModel(myPlugin.pluginViewModel)
                                .WithMessage(myPlugin, string.Format(CultureInfo.CurrentCulture, Resources.GETTING_RELATIONSHIPS, entityName)));
        }

        private RetrieveEntityResponse GetRelationships(Tuple<string, TreeNodeAdv, bool, HashSet<string>> args)
        {
            return myPlugin.Service.Execute(new RetrieveEntityRequest()
            {
                EntityFilters = EntityFilters.Relationships,
                LogicalName = args.Item1 //entityName
            }) as RetrieveEntityResponse;
        }

        private void GetRelationships(Tuple<string, TreeNodeAdv, bool, HashSet<string>> input, RetrieveEntityResponse value, Exception exception)
        {
            try
            {
                if (exception != null)
                {
                    MessageBox.Show(exception.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (value is RetrieveEntityResponse result)
                {
                    var checkedRelationships = input.Item4;
                    var entityName = input.Item1;
                    var checkedState = input.Item3;
                    var relationshipsNode = input.Item2;
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

        public void CreateRelationshipNodes(TreeNodeAdv relationshipsNode, EntityMetadata entityMetadata, bool checkedState = false, HashSet<string> checkedRelationships = default)
        {
            var relationshipNodeList = new List<TreeNodeAdv>();
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
                relationshipNodeList.Add(node);
                relationshipsNode.Nodes.Add(node);
            }
            myPlugin.pluginViewModel.AllRelationships[entityMetadata.LogicalName] = relationshipNodeList.ToArray();
            if (entityMetadata.ManyToManyRelationships.Length == 0 &&
                entityMetadata.OneToManyRelationships.Length == 0 &&
                entityMetadata.ManyToOneRelationships.Length == 0)
            {
                relationshipsNode.Checked = checkedState;
            }
        }
    }
}
