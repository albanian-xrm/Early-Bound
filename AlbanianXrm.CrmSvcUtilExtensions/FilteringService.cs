﻿using System;
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

            all1NRelationships = new HashSet<string>((Environment.GetEnvironmentVariable("AlbanianXrm.EarlyBound:All1NRelationships") ?? "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            entity1NRelationships = new Dictionary<string, HashSet<string>>();
            foreach (var entity in entities.Except(all1NRelationships))
            {
                entity1NRelationships.Add(entity, new HashSet<string>((Environment.GetEnvironmentVariable("AlbanianXrm.EarlyBound:1NRelationships:" + entity) ?? "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)));
            }

            allN1Relationships = new HashSet<string>((Environment.GetEnvironmentVariable("AlbanianXrm.EarlyBound:AllN1Relationships") ?? "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            entityN1Relationships = new Dictionary<string, HashSet<string>>();
            foreach (var entity in entities.Except(allN1Relationships))
            {
                entityN1Relationships.Add(entity, new HashSet<string>((Environment.GetEnvironmentVariable("AlbanianXrm.EarlyBound:N1Relationships:" + entity) ?? "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)));
            }

            allNNRelationships = new HashSet<string>((Environment.GetEnvironmentVariable("AlbanianXrm.EarlyBound:AllNNRelationships") ?? "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            entityNNRelationships = new Dictionary<string, HashSet<string>>();
            foreach (var entity in entities.Except(allNNRelationships))
            {
                entityNNRelationships.Add(entity, new HashSet<string>((Environment.GetEnvironmentVariable("AlbanianXrm.EarlyBound:NNRelationships:" + entity) ?? "").Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)));
            }
        }

        private ICodeWriterFilterService DefaultService { get; set; }

        private HashSet<string> entities;
        private HashSet<string> allAttributes;
        private HashSet<string> all1NRelationships;
        private HashSet<string> allN1Relationships;
        private HashSet<string> allNNRelationships;
        private Dictionary<string, HashSet<string>> entityAttributes;
        private Dictionary<string, HashSet<string>> entity1NRelationships;
        private Dictionary<string, HashSet<string>> entityN1Relationships;
        private Dictionary<string, HashSet<string>> entityNNRelationships;

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
            if (relationshipMetadata is OneToManyRelationshipMetadata)
            {
                var oneToManyMetadata = (OneToManyRelationshipMetadata)relationshipMetadata;
                if (!all1NRelationships.Contains(oneToManyMetadata.ReferencingEntity) &&
                    entity1NRelationships.TryGetValue(oneToManyMetadata.ReferencingEntity, out relationships) &&
                    !relationships.Contains(oneToManyMetadata.SchemaName))
                {
                    return false;
                }
                if (!allN1Relationships.Contains(oneToManyMetadata.ReferencedEntity) &&
                    entityN1Relationships.TryGetValue(oneToManyMetadata.ReferencedEntity, out relationships) &&
                    !relationships.Contains(oneToManyMetadata.SchemaName))
                {
                    return false;
                }
            }
            else if (relationshipMetadata is ManyToManyRelationshipMetadata)
            {
                var manyToManyMetadata = (ManyToManyRelationshipMetadata)relationshipMetadata;
                if (!allNNRelationships.Contains(manyToManyMetadata.Entity1LogicalName) &&
                    entityNNRelationships.TryGetValue(manyToManyMetadata.Entity1LogicalName, out relationships) &&
                    !relationships.Contains(manyToManyMetadata.SchemaName))
                {
                    return false;
                }
                if (!allNNRelationships.Contains(manyToManyMetadata.Entity2LogicalName) &&
                    entityNNRelationships.TryGetValue(manyToManyMetadata.Entity2LogicalName, out relationships) &&
                    !relationships.Contains(manyToManyMetadata.SchemaName))
                {
                    return false;
                }
            }
            return this.DefaultService.GenerateRelationship(relationshipMetadata, otherEntityMetadata, services);
        }

        bool ICodeWriterFilterService.GenerateServiceContext(IServiceProvider services)
        {
            return this.DefaultService.GenerateServiceContext(services);
        }
    }
}
