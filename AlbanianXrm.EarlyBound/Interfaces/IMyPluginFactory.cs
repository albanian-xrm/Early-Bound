
using AlbanianXrm.EarlyBound.Logic;
using AlbanianXrm.XrmToolBox.Shared;
using Syncfusion.Windows.Forms.Tools;
using System.Windows.Forms;

namespace AlbanianXrm.EarlyBound.Interfaces
{
    interface IMyPluginFactory
    {
        BackgroundWorkHandler NewBackgroundWorkHandler();
        AttributeMetadataHandler NewAttributeMetadataHandler();
        CoreToolsDownloader NewCoreToolsDownloader();
        EntityMetadataHandler NewEntityMetadataHandler(TreeViewAdv metadataTree, EntitySelectionHandler entitySelectionHandler);
        EntitySelectionHandler NewEntitySelectionHandler(TreeViewAdv metadataTree, AttributeMetadataHandler attributeMetadataHandler, RelationshipMetadataHandler relationshipMetadataHandler);
        RelationshipMetadataHandler NewRelationshipMetadataHandler();
        EntityGeneratorHandler NewEntityGeneratorHandler(TreeViewAdv metadataTree, RichTextBox txtOutput);
        PluginViewModel NewPluginViewModel();
    }
}
