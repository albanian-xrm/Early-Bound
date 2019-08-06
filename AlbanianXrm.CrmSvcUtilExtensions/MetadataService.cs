using Microsoft.Crm.Services.Utility;

namespace AlbanianXrm.CrmSvcUtilExtensions
{
    public class MetadataService : IMetadataProviderService
    {
        IOrganizationMetadata cachedMetadata;
        IMetadataProviderService defaultMetadataService;
        public MetadataService(IMetadataProviderService defaultMetadataService)
        {
#if DEBUG
            if ((System.Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_ATTACHDEBUGGER) ?? "") != "")
            {
                System.Diagnostics.Debugger.Launch();
            }
#endif
            this.defaultMetadataService = defaultMetadataService;
        }

        public IOrganizationMetadata LoadMetadata()
        {
            if (cachedMetadata == null)
            {
                cachedMetadata = defaultMetadataService.LoadMetadata();
            }
            return cachedMetadata;
        }
    }
}
