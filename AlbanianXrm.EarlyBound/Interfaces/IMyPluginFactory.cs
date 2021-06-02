
using AlbanianXrm.BackgroundWorker;
using AlbanianXrm.EarlyBound.Logic;
using Syncfusion.Windows.Forms.Tools;
using System.Windows.Forms;

namespace AlbanianXrm.EarlyBound.Interfaces
{
    interface IMyPluginFactory
    {
        AlBackgroundWorkHandler NewBackgroundWorkHandler();
        AttributeMetadataHandler NewAttributeMetadataHandler();
        CoreToolsDownloader NewCoreToolsDownloader();
        EntityMetadataHandler NewEntityMetadataHandler(TreeViewAdv metadataTree, EntitySelectionHandler entitySelectionHandler);
        EntitySelectionHandler NewEntitySelectionHandler(TreeViewAdv metadataTree, AttributeMetadataHandler attributeMetadataHandler, RelationshipMetadataHandler relationshipMetadataHandler);
        RelationshipMetadataHandler NewRelationshipMetadataHandler();
        EntityGeneratorHandler NewEntityGeneratorHandler(TreeViewAdv metadataTree, RichTextBox txtOutput);
        PluginViewModel NewPluginViewModel();
    }
}
