using FakeItEasy;
using Microsoft.Crm.Services.Utility;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using Xunit;

namespace AlbanianXrm.CrmSvcUtilExtensions.Tests
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
            Assert.Null(filteringService.Test);
            var shouldGenerateAccount = (filteringService as ICodeWriterFilterService).GenerateEntity(metadata, fakeServiceProvider.FakedObject);

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
            Environment.SetEnvironmentVariable("AlbanianXrm.EarlyBound:Entities", "contact");

            var filteringService = new FilteringService(fakeFilterService.FakedObject);
            var shouldGenerateAccount = (filteringService as ICodeWriterFilterService).GenerateEntity(metadata, fakeServiceProvider.FakedObject);

            Environment.SetEnvironmentVariable("AlbanianXrm.EarlyBound:Entities", null);

            Assert.False(shouldGenerateAccount);
        }
    }
}
