﻿using AlbanianXrm.BackgroundWorker;
using AlbanianXrm.EarlyBound.Interfaces;
using AlbanianXrm.EarlyBound.Logic;
using Syncfusion.Windows.Forms.Tools;
using System.Windows.Forms;

namespace AlbanianXrm.EarlyBound.Factories
{
    internal class MyPluginFactory : IMyPluginFactory
    {
        private readonly MyPluginControl myPlugin;
        private readonly PluginViewModel pluginViewModel;
        private readonly AlBackgroundWorkHandler backgroundWorkHandler;

        private MyPluginFactory(MyPluginControl myPlugin)
        {
            this.myPlugin = myPlugin;
            this.pluginViewModel = new PluginViewModel();
            this.backgroundWorkHandler = new AlBackgroundWorkHandler();
        }

        public AlBackgroundWorkHandler NewBackgroundWorkHandler()
        {
            return backgroundWorkHandler;
        }

        public static IMyPluginFactory GetMyPluginFactory(MyPluginControl myPlugin)
        {
            return new MyPluginFactory(myPlugin);
        }

        public AttributeMetadataHandler NewAttributeMetadataHandler()
        {
            return new AttributeMetadataHandler(myPlugin, backgroundWorkHandler);
        }

        public CoreToolsDownloader NewCoreToolsDownloader()
        {
            return new CoreToolsDownloader(myPlugin, backgroundWorkHandler);
        }

        public EntitySelectionHandler NewEntitySelectionHandler(TreeViewAdv metadataTree, AttributeMetadataHandler attributeMetadataHandler, RelationshipMetadataHandler relationshipMetadataHandler)
        {
            return new EntitySelectionHandler(myPlugin, backgroundWorkHandler, metadataTree, attributeMetadataHandler, relationshipMetadataHandler);
        }

        public EntityMetadataHandler NewEntityMetadataHandler(TreeViewAdv metadataTree, EntitySelectionHandler entitySelectionHandler)
        {
            return new EntityMetadataHandler(myPlugin, backgroundWorkHandler, metadataTree, entitySelectionHandler);
        }

        public RelationshipMetadataHandler NewRelationshipMetadataHandler()
        {
            return new RelationshipMetadataHandler(myPlugin, backgroundWorkHandler);
        }

        public EntityGeneratorHandler NewEntityGeneratorHandler(TreeViewAdv metadataTree, RichTextBox txtOutput)
        {
            return new EntityGeneratorHandler(myPlugin, backgroundWorkHandler, metadataTree, txtOutput);
        }

        public PluginViewModel NewPluginViewModel()
        {
            return pluginViewModel;
        }
    }
}
