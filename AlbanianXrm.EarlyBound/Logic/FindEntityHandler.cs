using Microsoft.Xrm.Sdk.Metadata;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.WinForms.ListView;
using System;

namespace AlbanianXrm.EarlyBound.Logic
{
    internal class FindEntityHandler
    {
        private readonly TreeViewAdv metadataTree;
        private readonly SfComboBox cmbFindEntity;
        private string lastSelectedValue = null;

        public FindEntityHandler(TreeViewAdv metadataTree, SfComboBox cmbFindEntity)
        {
            this.metadataTree = metadataTree;
            this.cmbFindEntity = cmbFindEntity;
            WireEvents();
        }

        internal void WireEvents()
        {
            cmbFindEntity.SelectedValueChanged += CmbFindEntity_SelectedValueChanged;
            cmbFindEntity.Enter += CmbFindEntity_Enter;
            metadataTree.SelectedNodes.CollectionChanged += SelectedNodes_CollectionChanged;
        }

        private void SelectedNodes_CollectionChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e)
        {
            var node = metadataTree.SelectedNode;
            if (node == null) return;
            var entity = node.Tag as EntityMetadata;
            if (entity == null) return;
            cmbFindEntity.SelectedValue = entity.LogicalName;
        }

        private void CmbFindEntity_Enter(object sender, EventArgs e)
        {
            cmbFindEntity.Text = "";
            cmbFindEntity.Refresh();
        }

        private void CmbFindEntity_SelectedValueChanged(object sender, EventArgs e)
        {
            var entityName = cmbFindEntity.SelectedValue as string;
            if (lastSelectedValue == entityName) return;
            lastSelectedValue = entityName;
            if (string.IsNullOrEmpty(entityName)) return;
            foreach (TreeNodeAdv node in metadataTree.Nodes)
            {
                var entity = node.Tag as EntityMetadata;
                if (entity.LogicalName == entityName)
                {
                    metadataTree.SelectedNode = node;
                    metadataTree.Select();
                    break;
                }
            }
        }
    }
}
