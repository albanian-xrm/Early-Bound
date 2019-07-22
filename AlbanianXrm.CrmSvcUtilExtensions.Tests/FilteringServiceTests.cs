using AlbanianXrm;
using AlbanianXrm.CrmSvcUtilExtensions;
using FakeItEasy;
using Microsoft.Crm.Services.Utility;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Reflection;
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

            A.CallTo(() => fakeFilterService.FakedObject.GenerateEntity(A<EntityMetadata>._, A<IServiceProvider>._)).MustHaveHappenedOnceExactly();
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
            typeof(AttributeMetadata).GetField("_entityLogicalName", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(metadata, "contact");

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
            typeof(AttributeMetadata).GetField("_entityLogicalName", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(metadata, "contact");

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
            typeof(AttributeMetadata).GetField("_entityLogicalName", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(metadata, "contact");

            var filteringService = new FilteringService(fakeFilterService.FakedObject);
            var shouldGenerateLastName = (filteringService as ICodeWriterFilterService).GenerateAttribute(metadata, fakeServiceProvider.FakedObject);

            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES, null);

            A.CallTo(() => fakeFilterService.FakedObject.GenerateAttribute(A<AttributeMetadata>._, A<IServiceProvider>._)).MustNotHaveHappened();
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
            typeof(AttributeMetadata).GetField("_entityLogicalName", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(metadata, "contact");

            var filteringService = new FilteringService(fakeFilterService.FakedObject);
            var shouldGenerateLastName = (filteringService as ICodeWriterFilterService).GenerateAttribute(metadata, fakeServiceProvider.FakedObject);

            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES, null);
            Environment.SetEnvironmentVariable(Constants.ENVIRONMENT_ALL_ATTRIBUTES, null);

            A.CallTo(() => fakeFilterService.FakedObject.GenerateAttribute(A<AttributeMetadata>._, A<IServiceProvider>._)).MustHaveHappenedOnceExactly();
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

            A.CallTo(() => fakeFilterService.FakedObject.GenerateRelationship(A<RelationshipMetadataBase>._, A<EntityMetadata>._, A<IServiceProvider>._)).MustHaveHappenedOnceExactly();
            Assert.True(shouldGenerateRelationship);
        }
    }
}
