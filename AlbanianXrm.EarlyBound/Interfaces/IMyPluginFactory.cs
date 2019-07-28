
using AlbanianXrm.EarlyBound.Logic;
using Syncfusion.Windows.Forms.Tools;

namespace AlbanianXrm.EarlyBound.Interfaces
{
    interface IMyPluginFactory
    {
        AttributeMetadataHandler NewAttributeMetadataHandler(MyPluginControl myPlugin);
        CoreToolsDownloader NewCoreToolsDownloader(MyPluginControl myPlugin);
        EntityMetadataHandler NewEntityMetadataHandler(MyPluginControl myPlugin, TreeViewAdv metadataTree);
        RelationshipMetadataHandler NewRelationshipMetadataHandler(MyPluginControl myPlugin);
        EntityGeneratorHandler NewEntityGeneratorHandler(MyPluginControl myPlugin, TreeViewAdv metadataTree, TextBoxExt txtOutput);
        PluginViewModel NewPluginViewModel();
    }
}
