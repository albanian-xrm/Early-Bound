using AlbanianXrm;
using AlbanianXrm.CrmSvcUtilExtensions;
using AlbanianXrm.CrmSvcUtilExtensions.Tests;
using FakeItEasy;
using Microsoft.Crm.Services.Utility;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using Xunit;

namespace Tests
{
    public class FilteringServiceTests
    {
        [Fact]
        public void GenerateEntityUsesDefaultFilterServiceIfNoParametersAreSpecified()
        {
            var fakeFilterService = new Fake<ICodeWriterFilterService>();
            fakeFilterService.AnyCall().WithReturnType<bool>().Returns(true);

            var fakeServiceProvider = new Fake<IServiceProvider>();
            var metadata = new EntityMetadata()
            {
                LogicalName = "account"
            };

            var filteringService = new FilteringService(fakeFilterService.FakedObject);
            var shouldGenerateAccount = (filteringService as ICodeWriterFilterService).GenerateEntity(metadata, fakeServiceProvider.FakedObject);

            A.CallTo(() => fakeFilterService.FakedObject.GenerateEntity(A<EntityMetadata>._, A<IServiceProvider>._)).MustHaveHappenedOnceExactly();
            Assert.True(shouldGenerateAccount);
        }

        [Fact]
        public void GenerateEntitySkipsEntityIfItIsNotSpecifiedInTheParameters()
        {
            var fakeFilterService = new Fake<ICodeWriterFilterService>();
            fakeFilterService.AnyCall().WithReturnType<bool>().Returns(true);
            var fakeServiceProvider = new Fake<IServiceProvider>();
            var metadata = new EntityMetadata()
            {
                LogicalName = "account"
            };
            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES, "contact");

            var filteringService = new FilteringService(fakeFilterService.FakedObject);
            var shouldGenerateAccount = (filteringService as ICodeWriterFilterService).GenerateEntity(metadata, fakeServiceProvider.FakedObject);

            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES, null);

            A.CallTo(() => fakeFilterService.FakedObject.GenerateEntity(A<EntityMetadata>._, A<IServiceProvider>._)).MustNotHaveHappened();
            Assert.False(shouldGenerateAccount);
        }

        [Fact]
        public void GenerateEntityGeneratesEntityIfItIsSpecifiedInTheParameters()
        {
            var fakeFilterService = new Fake<ICodeWriterFilterService>();
            fakeFilterService.AnyCall().WithReturnType<bool>().Returns(true);
            var fakeServiceProvider = new Fake<IServiceProvider>();
            var metadata = new EntityMetadata()
            {
                LogicalName = "account"
            };
            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES, "account");

            var filteringService = new FilteringService(fakeFilterService.FakedObject);
            var shouldGenerateAccount = (filteringService as ICodeWriterFilterService).GenerateEntity(metadata, fakeServiceProvider.FakedObject);

            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES, null);

            Assert.True(shouldGenerateAccount);
        }

        [Fact]
        public void GenerateAtributeUsesDefaultFilterServiceIfNoParametersAreSpecified()
        {
            var fakeFilterService = new Fake<ICodeWriterFilterService>();
            fakeFilterService.AnyCall().WithReturnType<bool>().Returns(true);

            var fakeServiceProvider = new Fake<IServiceProvider>();
            var metadata = new AttributeMetadata()
            {
                LogicalName = "lastname"
            };
            metadata.SetEntityLogicalName("contact");

            var filteringService = new FilteringService(fakeFilterService.FakedObject);
            var shouldGenerateLastName = (filteringService as ICodeWriterFilterService).GenerateAttribute(metadata, fakeServiceProvider.FakedObject);

            A.CallTo(() => fakeFilterService.FakedObject.GenerateAttribute(A<AttributeMetadata>._, A<IServiceProvider>._)).MustHaveHappenedOnceExactly();
            Assert.True(shouldGenerateLastName);
        }

        [Fact]
        public void GenerateAttributeSkipsEntityIfItIsNotSpecifiedInTheParameters()
        {
            var fakeFilterService = new Fake<ICodeWriterFilterService>();
            fakeFilterService.AnyCall().WithReturnType<bool>().Returns(true);

            var fakeServiceProvider = new Fake<IServiceProvider>();
            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES, "contact");
            Environment.SetEnvironmentVariable(string.Format(Constants.ENVIRONMENT_ENTITY_ATTRIBUTES, "contact"), "contactid");

            var metadata = new AttributeMetadata()
            {
                LogicalName = "lastname"
            };
            metadata.SetEntityLogicalName("contact");

            var filteringService = new FilteringService(fakeFilterService.FakedObject);
            var shouldGenerateLastName = (filteringService as ICodeWriterFilterService).GenerateAttribute(metadata, fakeServiceProvider.FakedObject);

            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES, null);
            Environment.SetEnvironmentVariable(string.Format(Constants.ENVIRONMENT_ENTITY_ATTRIBUTES, "contact"), null);

            A.CallTo(() => fakeFilterService.FakedObject.GenerateAttribute(A<AttributeMetadata>._, A<IServiceProvider>._)).MustNotHaveHappened();
            Assert.False(shouldGenerateLastName);
        }

        [Fact]
        public void GenerateAttributeSkipsAttributeIfAllAttributesParameterIsNotSpecified()
        {
            var fakeFilterService = new Fake<ICodeWriterFilterService>();
            fakeFilterService.AnyCall().WithReturnType<bool>().Returns(true);

            var fakeServiceProvider = new Fake<IServiceProvider>();
            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES, "contact");

            var metadata = new AttributeMetadata()
            {
                LogicalName = "lastname"
            };
            metadata.SetEntityLogicalName("contact");

            var filteringService = new FilteringService(fakeFilterService.FakedObject);
            var shouldGenerateLastName = (filteringService as ICodeWriterFilterService).GenerateAttribute(metadata, fakeServiceProvider.FakedObject);

            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES, null);

            A.CallTo(() => fakeFilterService.FakedObject
                .GenerateAttribute(A<AttributeMetadata>._, A<IServiceProvider>._)).MustNotHaveHappened();
            Assert.False(shouldGenerateLastName);
        }

        [Fact]
        public void GenerateAttributeGeneratesAttributeIfAllAttributesParameterIsSpecified()
        {
            var fakeFilterService = new Fake<ICodeWriterFilterService>();
            fakeFilterService.AnyCall().WithReturnType<bool>().Returns(true);

            var fakeServiceProvider = new Fake<IServiceProvider>();
            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES, "contact");
            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ALL_ATTRIBUTES, "contact");

            var metadata = new AttributeMetadata()
            {
                LogicalName = "lastname"
            };
            metadata.SetEntityLogicalName("contact");

            var filteringService = new FilteringService(fakeFilterService.FakedObject);
            var shouldGenerateLastName = (filteringService as ICodeWriterFilterService).GenerateAttribute(metadata, fakeServiceProvider.FakedObject);

            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES, null);
            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ALL_ATTRIBUTES, null);

            Assert.True(shouldGenerateLastName);
        }

        [Fact]
        public void Generate1NRelationshipIfParameterIsSpecified()
        {
            var fakeFilterService = new Fake<ICodeWriterFilterService>();
            fakeFilterService.AnyCall().WithReturnType<bool>().Returns(true);

            var fakeServiceProvider = new Fake<IServiceProvider>();
            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES, "contact");
            Environment.SetEnvironmentVariable(string.Format(Constants.ENVIRONMENT_RELATIONSHIPS1N, "contact"), "account_primary_contact");

            var metadata = new OneToManyRelationshipMetadata()
            {
                MetadataId = Guid.NewGuid(),
                SchemaName = "account_primary_contact",
                ReferencingEntity = "account",
                ReferencingAttribute = "primarycontactid",
                ReferencedEntity = "contact",
                ReferencedAttribute = "contactid"
            };

            var otherEntity = new EntityMetadata()
            {
                LogicalName = "account"
            };

            var filteringService = new FilteringService(fakeFilterService.FakedObject);
            var shouldGenerateRelationship = (filteringService as ICodeWriterFilterService).GenerateRelationship(metadata, otherEntity, fakeServiceProvider.FakedObject);

            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES, null);
            Environment.SetEnvironmentVariable(string.Format(Constants.ENVIRONMENT_RELATIONSHIPS1N, "contact"), null);

            Assert.True(shouldGenerateRelationship);
        }

        [Fact]
        public void Generate1NRelationshipIfAllRelationshipsParameterIsSpecified()
        {
            var fakeFilterService = new Fake<ICodeWriterFilterService>();
            fakeFilterService.AnyCall().WithReturnType<bool>().Returns(true);

            var fakeServiceProvider = new Fake<IServiceProvider>();
            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES, "contact");
            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ALL_RELATIONSHIPS, "contact");

            var metadata = new OneToManyRelationshipMetadata()
            {
                MetadataId = Guid.NewGuid(),
                SchemaName = "account_primary_contact",
                ReferencingEntity = "account",
                ReferencingAttribute = "primarycontactid",
                ReferencedEntity = "contact",
                ReferencedAttribute = "contactid"
            };

            var otherEntity = new EntityMetadata()
            {
                LogicalName = "account"
            };

            var filteringService = new FilteringService(fakeFilterService.FakedObject);
            var shouldGenerateRelationship = (filteringService as ICodeWriterFilterService).GenerateRelationship(metadata, otherEntity, fakeServiceProvider.FakedObject);

            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES, null);
            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ALL_RELATIONSHIPS, null);

            A.CallTo(() => fakeFilterService.FakedObject.GenerateRelationship(A<RelationshipMetadataBase>.Ignored, A<EntityMetadata>.Ignored, A<IServiceProvider>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.True(shouldGenerateRelationship);
        }

        [Fact]
        public void Skip1NRelationshipIfParameterDoesNotContainRelationship()
        {
            var fakeFilterService = new Fake<ICodeWriterFilterService>();
            fakeFilterService.AnyCall().WithReturnType<bool>().Returns(true);

            var fakeServiceProvider = new Fake<IServiceProvider>();
            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES, "contact");
            Environment.SetEnvironmentVariable(string.Format(Constants.ENVIRONMENT_RELATIONSHIPS1N, "contact"), "Contact_Annotation");


            var metadata = new OneToManyRelationshipMetadata()
            {
                MetadataId = Guid.NewGuid(),
                SchemaName = "account_primary_contact",
                ReferencingEntity = "account",
                ReferencingAttribute = "primarycontactid",
                ReferencedEntity = "contact",
                ReferencedAttribute = "contactid"
            };

            var otherEntity = new EntityMetadata()
            {
                LogicalName = "account"
            };

            var filteringService = new FilteringService(fakeFilterService.FakedObject);
            var shouldGenerateRelationship = (filteringService as ICodeWriterFilterService).GenerateRelationship(metadata, otherEntity, fakeServiceProvider.FakedObject);

            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES, null);
            Environment.SetEnvironmentVariable(string.Format(Constants.ENVIRONMENT_RELATIONSHIPS1N, "contact"), null);

            A.CallTo(() => fakeFilterService.FakedObject.GenerateRelationship(A<RelationshipMetadataBase>.Ignored, A<EntityMetadata>.Ignored, A<IServiceProvider>.Ignored)).MustNotHaveHappened();
            Assert.False(shouldGenerateRelationship);
        }

        [Fact]
        public void GenerateN1RelationshipIfParameterIsSpecified()
        {
            var fakeFilterService = new Fake<ICodeWriterFilterService>();
            fakeFilterService.AnyCall().WithReturnType<bool>().Returns(true);

            var fakeServiceProvider = new Fake<IServiceProvider>();
            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES, "account");
            Environment.SetEnvironmentVariable(string.Format(Constants.ENVIRONMENT_RELATIONSHIPSN1, "account"), "account_primary_contact");

            var metadata = new OneToManyRelationshipMetadata()
            {
                MetadataId = Guid.NewGuid(),
                SchemaName = "account_primary_contact",
                ReferencingEntity = "account",
                ReferencingAttribute = "primarycontactid",
                ReferencedEntity = "contact",
                ReferencedAttribute = "contactid"
            };

            var otherEntity = new EntityMetadata()
            {
                LogicalName = "contact"
            };

            var filteringService = new FilteringService(fakeFilterService.FakedObject);
            var shouldGenerateRelationship = (filteringService as ICodeWriterFilterService).GenerateRelationship(metadata, otherEntity, fakeServiceProvider.FakedObject);

            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES, null);
            Environment.SetEnvironmentVariable(string.Format(Constants.ENVIRONMENT_RELATIONSHIPSN1, "account"), null);

            Assert.True(shouldGenerateRelationship);
        }

        [Fact]
        public void GenerateN1RelationshipIfAllRelationshipsParameterIsSpecified()
        {
            var fakeFilterService = new Fake<ICodeWriterFilterService>();
            fakeFilterService.AnyCall().WithReturnType<bool>().Returns(true);

            var fakeServiceProvider = new Fake<IServiceProvider>();
            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES, "account");
            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ALL_RELATIONSHIPS, "account");

            var metadata = new OneToManyRelationshipMetadata()
            {
                MetadataId = Guid.NewGuid(),
                SchemaName = "account_primary_contact",
                ReferencingEntity = "account",
                ReferencingAttribute = "primarycontactid",
                ReferencedEntity = "contact",
                ReferencedAttribute = "contactid"
            };

            var otherEntity = new EntityMetadata()
            {
                LogicalName = "contact"
            };

            var filteringService = new FilteringService(fakeFilterService.FakedObject);
            var shouldGenerateRelationship = (filteringService as ICodeWriterFilterService).GenerateRelationship(metadata, otherEntity, fakeServiceProvider.FakedObject);

            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES, null);
            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ALL_RELATIONSHIPS, null);

            A.CallTo(() => fakeFilterService.FakedObject.GenerateRelationship(A<RelationshipMetadataBase>.Ignored, A<EntityMetadata>.Ignored, A<IServiceProvider>.Ignored)).MustHaveHappenedOnceExactly();
            Assert.True(shouldGenerateRelationship);
        }

        [Fact]
        public void SkipN1RelationshipIfParameterDoesNotContainRelationship()
        {
            var fakeFilterService = new Fake<ICodeWriterFilterService>();
            fakeFilterService.AnyCall().WithReturnType<bool>().Returns(true);

            var fakeServiceProvider = new Fake<IServiceProvider>();
            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES, "account");
            Environment.SetEnvironmentVariable(string.Format(Constants.ENVIRONMENT_RELATIONSHIPS1N, "account"), "Account_Annotation");


            var metadata = new OneToManyRelationshipMetadata()
            {
                MetadataId = Guid.NewGuid(),
                SchemaName = "account_primary_contact",
                ReferencingEntity = "account",
                ReferencingAttribute = "primarycontactid",
                ReferencedEntity = "contact",
                ReferencedAttribute = "contactid"
            };

            var otherEntity = new EntityMetadata()
            {
                LogicalName = "contact"
            };

            var filteringService = new FilteringService(fakeFilterService.FakedObject);
            var shouldGenerateRelationship = (filteringService as ICodeWriterFilterService).GenerateRelationship(metadata, otherEntity, fakeServiceProvider.FakedObject);

            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES, null);
            Environment.SetEnvironmentVariable(string.Format(Constants.ENVIRONMENT_RELATIONSHIPS1N, "account"), null);

            A.CallTo(() => fakeFilterService.FakedObject.GenerateRelationship(A<RelationshipMetadataBase>.Ignored, A<EntityMetadata>.Ignored, A<IServiceProvider>.Ignored)).MustNotHaveHappened();
            Assert.False(shouldGenerateRelationship);
        }
    }
}
