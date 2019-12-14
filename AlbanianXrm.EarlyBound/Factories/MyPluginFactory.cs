using AlbanianXrm.EarlyBound.Interfaces;
using AlbanianXrm.EarlyBound.Logic;
using Syncfusion.Windows.Forms.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlbanianXrm.EarlyBound.Factories
{
    internal class MyPluginFactory : IMyPluginFactory
    {
        public AttributeMetadataHandler NewAttributeMetadataHandler(MyPluginControl myPlugin)
        {
            return new AttributeMetadataHandler(myPlugin);
        }

        public CoreToolsDownloader NewCoreToolsDownloader(MyPluginControl myPlugin)
        {
            return new CoreToolsDownloader(myPlugin);
        }

        public EntityMetadataHandler NewEntityMetadataHandler(MyPluginControl myPlugin, TreeViewAdv metadataTree)
        {
            return new EntityMetadataHandler(myPlugin, metadataTree);
        }

        public RelationshipMetadataHandler NewRelationshipMetadataHandler(MyPluginControl myPlugin)
        {
            return new RelationshipMetadataHandler(myPlugin);
        }

        public EntityGeneratorHandler NewEntityGeneratorHandler(MyPluginControl myPlugin, TreeViewAdv metadataTree, RichTextBox txtOutput)
        {
            return new EntityGeneratorHandler(myPlugin, metadataTree, txtOutput);
        }

        public PluginViewModel NewPluginViewModel()
        {
            return new PluginViewModel();
        }
    }
}
