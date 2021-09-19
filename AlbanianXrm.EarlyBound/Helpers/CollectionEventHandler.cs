using Microsoft.Xrm.Sdk.Metadata;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.WinForms.ListView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlbanianXrm.EarlyBound.Helpers
{
    class CollectionEventHandler
    {
        private readonly TreeNodeAdv attributes;
        private readonly string logicalName;
        private readonly SfComboBox cmbFindEntity;

        public CollectionEventHandler(TreeNodeAdv attributes, string logicalName, SfComboBox cmbFindEntity)
        {
            this.attributes = attributes;
            this.logicalName = logicalName;
            this.cmbFindEntity = cmbFindEntity;
        }

        internal void Nodes_CollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            attributes.Nodes.CollectionChanged -= Nodes_CollectionChanged;
            if(cmbFindEntity.SelectedValue as string == logicalName)
            {
                cmbFindEntity.SelectedValue = null;
                cmbFindEntity.SelectedValue = logicalName;
            }
        }
    }
}
