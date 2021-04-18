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
using XrmToolBox.Extensibility;

namespace AlbanianXrm.EarlyBound.Logic
{
    internal class AttributeMetadataHandler
    {
        private readonly MyPluginControl myPlugin;
        private readonly BackgroundWorkHandler backgroundWorkHandler;

        public AttributeMetadataHandler(MyPluginControl myPlugin, BackgroundWorkHandler backgroundWorkHandler)
        {
            this.myPlugin = myPlugin;
            this.backgroundWorkHandler = backgroundWorkHandler;
        }

        public void GetAttributes(string entityName, TreeNodeAdv attributesNode, bool checkedState = false, HashSet<string> checkedAttributes = default(HashSet<string>))
        {
            backgroundWorkHandler.EnqueueWork(
                string.Format(CultureInfo.CurrentCulture, Resources.GETTING_ATTRIBUTES, entityName),
                () => myPlugin.Service.Execute(new RetrieveEntityRequest()
                {
                    EntityFilters = EntityFilters.Attributes,
                    LogicalName = entityName
                })
                 ,
                 (args) =>
                 {
                     try
                     {
                         if (args.Exception != null)
                         {
                             MessageBox.Show(args.Exception.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                         }
                         if (args.Value is RetrieveEntityResponse result)
                         {
                             if (checkedAttributes == null) checkedAttributes = new HashSet<string>();

                             var entityMetadata = myPlugin.entityMetadatas.FirstOrDefault(x => x.LogicalName == entityName);
                             typeof(EntityMetadata).GetProperty(nameof(entityMetadata.Attributes)).SetValue(entityMetadata, result.EntityMetadata.Attributes);
                             CreateAttributeNodes(attributesNode, result.EntityMetadata, checkedState, checkedAttributes);
                         }
                     }
#pragma warning disable CA1031 // We don't want our plugin to crash because of unhandled exceptions
                     catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
                     {
                         MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     }
                 }
             );
        }

        public static void CreateAttributeNodes(TreeNodeAdv attributesNode, EntityMetadata entityMetadata, bool checkedState = false, HashSet<string> checkedAttributes = default(HashSet<string>))
        {
            attributesNode.ExpandedOnce = true;
            foreach (var item in entityMetadata.Attributes.OrderBy(x => x.LogicalName))
            {
                if (!item.DisplayName.LocalizedLabels.Any()) continue;
                var name = item.DisplayName.LocalizedLabels.First().Label;

                TreeNodeAdv node = new TreeNodeAdv($"{item.LogicalName}: {name}")
                {
                    ExpandedOnce = true,
                    ShowCheckBox = true,
                    Tag = item,
                    Checked = checkedState || checkedAttributes.Contains(item.LogicalName)
                };

                attributesNode.Nodes.Add(node);
            }
            if (entityMetadata.Attributes.Length == 0) attributesNode.Checked = checkedState;
        }
    }
}
