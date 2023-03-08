﻿using Microsoft.PowerPlatform.Dataverse.ModelBuilderLib;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Linq;

namespace AlbanianXrm.CrmSvcUtilExtensions
{
    public class NamingService : INamingService
    {
        private readonly INamingService namingService;
        private readonly bool removePublisherPrefix;

        public NamingService(INamingService namingService)
        {
            this.namingService = namingService ?? throw new ArgumentNullException(nameof(namingService));
            removePublisherPrefix = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_REMOVEPUBLISHER));
        }

        public string GetNameForAttribute(EntityMetadata entityMetadata, AttributeMetadata attributeMetadata, IServiceProvider services)
        {
            if (entityMetadata == null) throw new ArgumentNullException(nameof(entityMetadata));
            if (attributeMetadata == null) throw new ArgumentNullException(nameof(attributeMetadata));

            if (removePublisherPrefix)
            {
                var noPrefixName = attributeMetadata.IsCustomAttribute == true ? attributeMetadata.SchemaName.Substring(attributeMetadata.SchemaName.IndexOf("_", StringComparison.InvariantCulture) + 1) : attributeMetadata.SchemaName;
                var result = noPrefixName;
                var equal = entityMetadata.Attributes.Where(x => noPrefixName == (x.IsCustomAttribute == true ? x.SchemaName.Substring(x.SchemaName.IndexOf("_", StringComparison.InvariantCulture) + 1) : x.SchemaName)).Select(x => new string[] { x.SchemaName, x.IsCustomAttribute == true ? x.SchemaName.Substring(x.SchemaName.IndexOf("_", StringComparison.InvariantCulture) + 1) : x.SchemaName }).ToArray();
                if (equal.Length > 1)
                {
                    for (int i = 0; i < equal.Length; i++)
                    {
                        if (equal[i][0] == attributeMetadata.SchemaName)
                        {
                            result = equal[i][1] + (i + 1);
                            break;
                        }
                    }
                }
                if (GetNameForEntity(entityMetadata, services) == noPrefixName)
                {
                    result += "_";
                }
                return result;
            }
            else
            {
                return namingService.GetNameForAttribute(entityMetadata, attributeMetadata, services);
            }
        }

        public string GetNameForEntity(EntityMetadata entityMetadata, IServiceProvider services)
        {
            if (entityMetadata == null) throw new ArgumentNullException(nameof(entityMetadata));
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (removePublisherPrefix)
            {
                var metadataService = (IMetadataProviderService)services.GetService(typeof(IMetadataProviderService));

                var noPrefixName = entityMetadata.IsCustomEntity == true ? entityMetadata.SchemaName.Substring(entityMetadata.SchemaName.IndexOf("_", StringComparison.InvariantCulture) + 1) : entityMetadata.SchemaName;
                var result = noPrefixName;
                var equal = metadataService.LoadMetadata(services).Entities.Where(x => noPrefixName == (x.IsCustomEntity == true ? x.SchemaName.Substring(x.SchemaName.IndexOf("_", StringComparison.InvariantCulture) + 1) : x.SchemaName)).Select(x => new string[] { x.SchemaName, x.IsCustomEntity == true ? x.SchemaName.Substring(x.SchemaName.IndexOf("_", StringComparison.InvariantCulture) + 1) : x.SchemaName }).ToArray();
                if (equal.Length > 1)
                {
                    for (int i = 0; i < equal.Length; i++)
                    {
                        if (equal[i][0] == entityMetadata.SchemaName)
                        {
                            result = equal[i][1] + (i + 1);
                            break;
                        }
                    }
                }
                return result;
            }
            else
            {
                return namingService.GetNameForEntity(entityMetadata, services);
            }
        }

        public string GetNameForEntitySet(EntityMetadata entityMetadata, IServiceProvider services)
        {
            if (entityMetadata == null) throw new ArgumentNullException(nameof(entityMetadata));
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (removePublisherPrefix)
            {
                var metadataService = (IMetadataProviderService)services.GetService(typeof(IMetadataProviderService));

                var noPrefixName = entityMetadata.IsCustomEntity == true ? entityMetadata.EntitySetName.Substring(entityMetadata.EntitySetName.IndexOf("_", StringComparison.InvariantCulture) + 1) : entityMetadata.EntitySetName;
                var result = noPrefixName;
                var equal = metadataService.LoadMetadata(services).Entities.Where(x => noPrefixName == (x.IsCustomEntity == true ? x.EntitySetName.Substring(x.EntitySetName.IndexOf("_", StringComparison.InvariantCulture) + 1) : x.EntitySetName)).Select(x => new string[] { x.EntitySetName, x.IsCustomEntity == true ? x.EntitySetName.Substring(x.EntitySetName.IndexOf("_", StringComparison.InvariantCulture) + 1) : x.EntitySetName }).ToArray();
                if (equal.Length > 1)
                {
                    for (int i = 0; i < equal.Length; i++)
                    {
                        if (equal[i][0] == entityMetadata.EntitySetName)
                        {
                            result = equal[i][1] + (i + 1);
                            break;
                        }
                    }
                }
                return result;
            }
            else
            {
                return namingService.GetNameForEntitySet(entityMetadata, services);
            }
        }

        public string GetNameForMessagePair(SdkMessagePair messagePair, IServiceProvider services)
        {
            return namingService.GetNameForMessagePair(messagePair, services);
        }

        public string GetNameForOption(OptionSetMetadataBase optionSetMetadata, OptionMetadata optionMetadata, IServiceProvider services)
        {
            return namingService.GetNameForOption(optionSetMetadata, optionMetadata, services);
        }

        public string GetNameForOptionSet(EntityMetadata entityMetadata, OptionSetMetadataBase optionSetMetadata, IServiceProvider services)
        {
            if (entityMetadata == null) throw new ArgumentNullException(nameof(entityMetadata));
            if (optionSetMetadata == null) throw new ArgumentNullException(nameof(optionSetMetadata));
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (removePublisherPrefix)
            {
                var noPrefixName = optionSetMetadata.IsCustomOptionSet == true ? optionSetMetadata.Name.Substring(optionSetMetadata.Name.IndexOf("_", StringComparison.InvariantCulture) + 1) : optionSetMetadata.Name;
                var result = noPrefixName;
                var equal = entityMetadata.Attributes.Where(x => x is PicklistAttributeMetadata test && noPrefixName == (test.OptionSet.IsCustomOptionSet == true ? test.OptionSet.Name.Substring(test.OptionSet.Name.IndexOf("_", StringComparison.InvariantCulture) + 1) : test.OptionSet.Name)).Cast<PicklistAttributeMetadata>().Select(x => new string[] { x.OptionSet.Name, x.OptionSet.IsCustomOptionSet == true ? x.OptionSet.Name.Substring(x.OptionSet.Name.IndexOf("_", StringComparison.InvariantCulture) + 1) : x.OptionSet.Name }).ToArray();
                if (equal.Length > 1)
                {
                    for (int i = 0; i < equal.Length; i++)
                    {
                        if (equal[i][0] == optionSetMetadata.Name)
                        {
                            result = equal[i][1] + (i + 1);
                            break;
                        }
                    }
                }
                if (GetNameForEntity(entityMetadata, services) == noPrefixName)
                {
                    result += "_";
                }
                return result;
            }
            else
            {
                return namingService.GetNameForOptionSet(entityMetadata, optionSetMetadata, services);
            }
        }

        public string GetNameForRelationship(EntityMetadata entityMetadata, RelationshipMetadataBase relationshipMetadata, EntityRole? reflexiveRole, IServiceProvider services)
        {
            return namingService.GetNameForRelationship(entityMetadata, relationshipMetadata, reflexiveRole, services);
        }

        public string GetNameForRequestField(SdkMessageRequest request, SdkMessageRequestField requestField, IServiceProvider services)
        {
            return namingService.GetNameForRequestField(request, requestField, services);
        }

        public string GetNameForResponseField(SdkMessageResponse response, SdkMessageResponseField responseField, IServiceProvider services)
        {
            return namingService.GetNameForResponseField(response, responseField, services);
        }

        public string GetNameForServiceContext(IServiceProvider services)
        {
            return namingService.GetNameForServiceContext(services);
        }
    }
}
