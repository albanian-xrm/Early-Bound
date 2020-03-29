
using AlbanianXrm.EarlyBound.Logic;
using Syncfusion.Windows.Forms.Tools;
using System.Windows.Forms;

namespace AlbanianXrm.EarlyBound.Interfaces
{
    interface IMyPluginFactory
    {
        AttributeMetadataHandler NewAttributeMetadataHandler(MyPluginControl myPlugin);
        CoreToolsDownloader NewCoreToolsDownloader(MyPluginControl myPlugin);
        EntityMetadataHandler NewEntityMetadataHandler(MyPluginControl myPlugin, TreeViewAdv metadataTree, EntitySelectionHandler entitySelectionHandler);
        EntitySelectionHandler NewEntitySelectionHandler(MyPluginControl myPlugin, TreeViewAdv metadataTree, AttributeMetadataHandler attributeMetadataHandler, RelationshipMetadataHandler relationshipMetadataHandler);
        RelationshipMetadataHandler NewRelationshipMetadataHandler(MyPluginControl myPlugin);
        EntityGeneratorHandler NewEntityGeneratorHandler(MyPluginControl myPlugin, TreeViewAdv metadataTree, RichTextBox txtOutput);
        PluginViewModel NewPluginViewModel();
    }
}
