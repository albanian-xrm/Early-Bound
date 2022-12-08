
using AlbanianXrm.BackgroundWorker;
using AlbanianXrm.EarlyBound.Logic;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.WinForms.ListView;
using System.Windows.Forms;

namespace AlbanianXrm.EarlyBound.Interfaces
{
    interface IMyPluginFactory
    {
        AlBackgroundWorkHandler NewBackgroundWorkHandler();
        AttributeMetadataHandler NewAttributeMetadataHandler();
        EntityMetadataHandler NewEntityMetadataHandler(TreeViewAdv metadataTree, EntitySelectionHandler entitySelectionHandler, AttributeMetadataHandler attributeMetadataHandler, RelationshipMetadataHandler relationshipMetadataHandler, SfComboBox cmbFindEntity);
        EntitySelectionHandler NewEntitySelectionHandler(TreeViewAdv metadataTree, AttributeMetadataHandler attributeMetadataHandler, RelationshipMetadataHandler relationshipMetadataHandler);
        ExtensionsMoverHandler NewExtensionsMoverHandler();
        FilterSelectedHandler NewFilterSelectedHandler(TreeViewAdv metadataTree, CheckBox chkOnlySelected);
        FindEntityHandler NewFindEntityHandler(TreeViewAdv metadataTree, SfComboBox cmbFindEntity, SfComboBox cmbFindChild);
        RelationshipMetadataHandler NewRelationshipMetadataHandler();
        EntityGeneratorHandler NewEntityGeneratorHandler(TreeViewAdv metadataTree, RichTextBox txtOutput);
        PluginViewModel NewPluginViewModel();
    }
}
