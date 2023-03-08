using Microsoft.PowerPlatform.Dataverse.ModelBuilderLib;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;

namespace AlbanianXrm.CrmSvcUtilExtensions
{
    internal class MetadataQueryService : IMetadataProviderQueryService
    {
        public EntityMetadata[] RetrieveEntities(IOrganizationService service)
        {
            throw new NotImplementedException();
        }

        public OptionSetMetadataBase[] RetrieveOptionSets(IOrganizationService service)
        {
            throw new NotImplementedException();
        }

        public SdkMessages RetrieveSdkRequests(IOrganizationService service)
        {
            throw new NotImplementedException();
        }
    }
}
