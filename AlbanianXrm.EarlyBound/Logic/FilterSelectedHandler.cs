using Microsoft.Xrm.Sdk.Metadata;
using Syncfusion.Windows.Forms.Tools;
using System;
using System.Windows.Forms;

namespace AlbanianXrm.EarlyBound.Logic
{
    internal class FilterSelectedHandler
    {
        public const string DUMMY = "Dummy";
        private readonly PluginViewModel pluginViewModel;
        private readonly TreeViewAdv metadataTree;
        private readonly CheckBox chkOnlySelected;

        public FilterSelectedHandler(PluginViewModel pluginViewModel, TreeViewAdv metadataTree, CheckBox chkOnlySelected)
        {
            this.pluginViewModel = pluginViewModel;
            this.metadataTree = metadataTree;
            this.chkOnlySelected = chkOnlySelected;
            WireEvents();
        }

        internal void WireEvents()
        {
            chkOnlySelected.CheckedChanged += ChkOnlySelected_CheckedChanged;
            metadataTree.BeforeCheck += MetadataTree_BeforeCheck;
        }

        private void MetadataTree_BeforeCheck(object sender, TreeNodeAdvBeforeCheckEventArgs e)
        {
            if (chkOnlySelected.CheckState == CheckState.Checked)
            {
                chkOnlySelected.CheckState = CheckState.Indeterminate;
            }
        }

        internal void ChkOnlySelected_CheckedChanged(object sender, EventArgs e)
        {
            metadataTree.SelectedNode = null;
            if (chkOnlySelected.Checked)
            {
                RemoveUncheckedFromTree();
            }
            else
            {
                RestoreUncheckedInTree();
            }
        }

        private void RestoreUncheckedInTree()
        {
            for (int i = 0; i < pluginViewModel.AllEntities.Length; i++)
            {
                if (metadataTree.Nodes.Count <= i || metadataTree.Nodes[i] != pluginViewModel.AllEntities[i])
                {
                    metadataTree.Nodes.Insert(i, pluginViewModel.AllEntities[i]);
                }
                var entity = pluginViewModel.AllEntities[i].Tag as EntityMetadata;
                if (pluginViewModel.AllAttributes.TryGetValue(entity.LogicalName, out TreeNodeAdv[] attributes))
                {
                    RestoreUncheckedChildren(pluginViewModel.AllEntities[i].Nodes[0], attributes);
                }
                if (pluginViewModel.AllRelationships.TryGetValue(entity.LogicalName, out TreeNodeAdv[] relationships))
                {
                    RestoreUncheckedChildren(pluginViewModel.AllEntities[i].Nodes[1], relationships);
                }
            }
        }

        private void RestoreUncheckedChildren(TreeNodeAdv treeNodeAdv, TreeNodeAdv[] children)
        {
            for(int i=0; i < treeNodeAdv.Nodes.Count; i++)
            {
                if(treeNodeAdv.Nodes[i].Tag as string == DUMMY)
                {
                    treeNodeAdv.Nodes.RemoveAt(i);
                    break;
                }
            }
            for (int i = 0; i < children.Length; i++)
            {
                if (treeNodeAdv.Nodes.Count <= i || treeNodeAdv.Nodes[i] != children[i])
                {
                    treeNodeAdv.Nodes.Insert(i, children[i]);
                }
            }
        }

        private void RemoveUncheckedFromTree()
        {
            int entitiesCount = metadataTree.Nodes.Count;
            int removedEntities = 0;
            for (int i = 0; i < entitiesCount; i++)
            {
                if (metadataTree.Nodes[i - removedEntities].CheckState == CheckState.Unchecked)
                {
                    metadataTree.Nodes.RemoveAt(i - removedEntities);
                    removedEntities++;
                    continue;
                }
                RemoveChildren(metadataTree.Nodes[i - removedEntities].Nodes[0]);
                RemoveChildren(metadataTree.Nodes[i - removedEntities].Nodes[1]);
            }
        }

        private void RemoveChildren(TreeNodeAdv attributes)
        {
            int childrenCount = attributes.Nodes.Count;
            int removedChildren = 0;
            for (int j = 0; j < childrenCount; j++)
            {
                if (attributes.Nodes[j - removedChildren].CheckState == CheckState.Unchecked)
                {
                    attributes.Nodes.RemoveAt(j - removedChildren);
                    removedChildren++;
                }
            }
            if (removedChildren > 0)
            {
                attributes.Nodes.Add(new TreeNodeAdv("Filtered...")
                {
                    Tag = DUMMY,
                    Checked = false,
                    Enabled = false,
                    ExpandedOnce = true
                });
            }
        }
    }
}
