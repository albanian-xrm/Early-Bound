using AlbanianXrm.EarlyBound.Helpers;
using Microsoft.Xrm.Sdk.Metadata;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.WinForms.ListView;
using System;
using System.Collections.Generic;

namespace AlbanianXrm.EarlyBound.Logic
{
    internal class FindEntityHandler
    {
        private readonly TreeViewAdv metadataTree;
        private readonly SfComboBox cmbFindEntity;
        private readonly SfComboBox cmbFindChild;
        private string lastSelectedEntity = null;
        private string lastSelectedChild = null;

        public FindEntityHandler(TreeViewAdv metadataTree, SfComboBox cmbFindEntity, SfComboBox cmbFindChild)
        {
            this.metadataTree = metadataTree;
            this.cmbFindEntity = cmbFindEntity;
            this.cmbFindChild = cmbFindChild;
            WireEvents();
        }

        internal void WireEvents()
        {
            cmbFindEntity.SelectedValueChanged += CmbFindEntity_SelectedValueChanged;
            cmbFindEntity.Enter += CmbFindEntity_Enter;
            cmbFindChild.SelectedValueChanged += CmbFindChild_SelectedValueChanged;
            cmbFindChild.Enter += CmbFindChild_Enter;
            metadataTree.SelectedNodes.CollectionChanged += SelectedNodes_CollectionChanged;          
        }

        private void MetadataTree_AfterExpand(object sender, TreeViewAdvNodeEventArgs e)
        {
            if ((e.Node.Text == "Attributes" || e.Node.Text == "Relationships") && e.Node.Tag == null &&
                e.Node.Parent.Tag is EntityMetadata entityMetadata &&
                entityMetadata.LogicalName == lastSelectedEntity)
            {
                lastSelectedEntity = null;
                cmbFindEntity.SelectedValue = entityMetadata.LogicalName;
            }
        }

        private void SelectedNodes_CollectionChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e)
        {
            var node = metadataTree.SelectedNode;
            if (node == null) return;
            if (node.Tag is EntityMetadata entity)
            {
                cmbFindEntity.SelectedValue = entity.LogicalName;
                cmbFindChild.SelectedValue = null;
            }
            else if (node.Tag is AttributeMetadata attribute)
            {
                entity = node.Parent.Parent.Tag as EntityMetadata;
                if (entity != null) cmbFindEntity.SelectedValue = entity.LogicalName;
                cmbFindChild.SelectedValue = attribute.LogicalName;
            }
            else if (node.Tag is RelationshipMetadataBase relationship)
            {
                entity = node.Parent.Parent.Tag as EntityMetadata;
                if (entity != null) cmbFindEntity.SelectedValue = entity.LogicalName;
                cmbFindChild.SelectedValue = relationship.SchemaName;
            }
        }

        private void CmbFindEntity_Enter(object sender, EventArgs e)
        {
            cmbFindEntity.Text = "";
            cmbFindEntity.Refresh();
        }

        private void CmbFindEntity_SelectedValueChanged(object sender, EventArgs e)
        {
            var entityName = cmbFindEntity.SelectedValue as string;
            if (lastSelectedEntity == entityName) return;
            lastSelectedEntity = entityName;
            var dataSource = new List<ComboItem>();
            if (string.IsNullOrEmpty(entityName))
            {
                cmbFindChild.DataSource = dataSource;
                return;
            }
            foreach (TreeNodeAdv node in metadataTree.Nodes)
            {
                var entity = node.Tag as EntityMetadata;
                if (entity.LogicalName == entityName)
                {
                    foreach (TreeNodeAdv childNode in node.Nodes[0].Nodes)
                    {
                        if (!(childNode.Tag is AttributeMetadata attribute))
                        {
                            continue;
                        }

                        dataSource.Add(new ComboItem() { Key = attribute.LogicalName, Value = childNode.Text });
                    }
                    foreach (TreeNodeAdv childNode in node.Nodes[1].Nodes)
                    {
                        if (!(childNode.Tag is RelationshipMetadataBase relationship))
                        {
                            continue;
                        }

                        dataSource.Add(new ComboItem() { Key = relationship.SchemaName, Value = childNode.Text });
                    }
                    cmbFindChild.DataSource = dataSource;
                    metadataTree.SelectedNode = node;
                    metadataTree.Select();
                    break;
                }
            }
        }

        private void CmbFindChild_Enter(object sender, EventArgs e)
        {
            cmbFindChild.Text = "";
            cmbFindChild.Refresh();
        }

        private void CmbFindChild_SelectedValueChanged(object sender, EventArgs e)
        {
            var childName = cmbFindChild.SelectedValue as string;
            if (lastSelectedChild == childName) return;
            lastSelectedChild = childName;
            if (string.IsNullOrEmpty(childName)) return;
            var entityName = cmbFindEntity.SelectedValue as string;
            if (string.IsNullOrEmpty(entityName)) return;
            foreach (TreeNodeAdv node in metadataTree.Nodes)
            {
                var entity = node.Tag as EntityMetadata;
                if (entity.LogicalName == entityName)
                {
                    foreach (TreeNodeAdv childNode in node.Nodes[0].Nodes)
                    {
                        if (!(childNode.Tag is AttributeMetadata attribute))
                        {
                            continue;
                        }

                        if (attribute.LogicalName == childName)
                        {
                            metadataTree.SelectedNode = childNode;
                            metadataTree.Select();
                            return;
                        }
                    }
                    foreach (TreeNodeAdv childNode in node.Nodes[1].Nodes)
                    {
                        if (!(childNode.Tag is RelationshipMetadataBase relationship))
                        {
                            continue;
                        }

                        if (relationship.SchemaName == childName)
                        {
                            metadataTree.SelectedNode = childNode;
                            metadataTree.Select();
                            return;
                        }
                    }
                }
            }
        }
    }
}
