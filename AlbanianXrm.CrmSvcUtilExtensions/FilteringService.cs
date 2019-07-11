using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Crm.Services.Utility;
using Microsoft.Xrm.Sdk.Metadata;

namespace AlbanianXrm.CrmSvcUtilExtensions
{
    public sealed class FilteringService : ICodeWriterFilterService
    {
        public FilteringService(ICodeWriterFilterService defaultService)
        {
            this.DefaultService = defaultService;
            entities = new HashSet<string>((Environment.GetEnvironmentVariable("AlbanianXrm.EarlyBound:Entities") ?? "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            allAttributes = new HashSet<string>((Environment.GetEnvironmentVariable("AlbanianXrm.EarlyBound:AllAttributes") ?? "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            entityAttributes = new Dictionary<string, HashSet<string>>();
            foreach (var entity in entities.Except(allAttributes))
            {
                entityAttributes.Add(entity, new HashSet<string>((Environment.GetEnvironmentVariable("AlbanianXrm.EarlyBound:Attributes:" + entity) ?? "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)));
            }
            allRelationships = new HashSet<string>((Environment.GetEnvironmentVariable("AlbanianXrm.EarlyBound:AllRelationships") ?? "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            entityRelationships = new Dictionary<string, HashSet<string>>();
            foreach (var entity in entities.Except(allRelationships))
            {
                entityRelationships.Add(entity, new HashSet<string>((Environment.GetEnvironmentVariable("AlbanianXrm.EarlyBound:Relationships:" + entity) ?? "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)));
            }
        }

        private ICodeWriterFilterService DefaultService { get; set; }

        private HashSet<string> entities;
        private HashSet<string> allAttributes;
        private HashSet<string> allRelationships;
        private Dictionary<string, HashSet<string>> entityAttributes;
        private Dictionary<string, HashSet<string>> entityRelationships;

        bool ICodeWriterFilterService.GenerateAttribute(AttributeMetadata attributeMetadata, IServiceProvider services)
        {
            HashSet<string> attributes;
            if (!allAttributes.Contains(attributeMetadata.EntityLogicalName) &&
                entityAttributes.TryGetValue(attributeMetadata.EntityLogicalName, out attributes) &&
                !attributes.Contains(attributeMetadata.LogicalName))
            {
                return false;
            }
            return this.DefaultService.GenerateAttribute(attributeMetadata, services);
        }

        bool ICodeWriterFilterService.GenerateEntity(EntityMetadata entityMetadata, IServiceProvider services)
        {
            if (entities.Any() && !entities.Contains(entityMetadata.LogicalName))
            {
                return false;
            }
            return this.DefaultService.GenerateEntity(entityMetadata, services);
        }

        bool ICodeWriterFilterService.GenerateOption(OptionMetadata optionMetadata, IServiceProvider services)
        {
            return this.DefaultService.GenerateOption(optionMetadata, services);
        }

        bool ICodeWriterFilterService.GenerateOptionSet(OptionSetMetadataBase optionSetMetadata, IServiceProvider services)
        {
            return this.DefaultService.GenerateOptionSet(optionSetMetadata, services);
        }

        bool ICodeWriterFilterService.GenerateRelationship(RelationshipMetadataBase relationshipMetadata, EntityMetadata otherEntityMetadata,
        IServiceProvider services)
        {
            HashSet<string> relationships;
            if (!allRelationships.Contains(relationshipMetadata.SchemaName) &&
                entityRelationships.TryGetValue(relationshipMetadata.SchemaName, out relationships) &&
                !relationships.Contains(relationshipMetadata.SchemaName))
            {
                return false;
            }
            return this.DefaultService.GenerateRelationship(relationshipMetadata, otherEntityMetadata, services);
        }

        bool ICodeWriterFilterService.GenerateServiceContext(IServiceProvider services)
        {
            return this.DefaultService.GenerateServiceContext(services);
        }
    }
}
