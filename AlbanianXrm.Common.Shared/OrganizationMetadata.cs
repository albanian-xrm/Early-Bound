#if CRMSVCUTILEXTENSIONS
using Microsoft.Crm.Services.Utility;
#endif
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AlbanianXrm.Common.Shared
{
    [DataContract]
    class OrganizationMetadata
#if CRMSVCUTILEXTENSIONS
        : IOrganizationMetadata
#endif
    {
        public OrganizationMetadata(EntityMetadata[] entities,
                                    OptionSetMetadataBase[] optionSets
#if CRMSVCUTILEXTENSIONS
                                    , SdkMessages messages)
        {
            Messages = messages;
#else
            )
        {
#endif
            Entities = entities;
            OptionSets = optionSets;
        }

        [DataMember]
        public EntityMetadata[] Entities { get { return _Entities ?? Array.Empty<EntityMetadata>(); } internal set { _Entities = value; } }
        private EntityMetadata[] _Entities;


        [DataMember]
        public OptionSetMetadataBase[] OptionSets { get { return _OptionSets ?? Array.Empty<OptionSetMetadataBase>(); } internal set { _OptionSets = value; } }
        private OptionSetMetadataBase[] _OptionSets;

#if CRMSVCUTILEXTENSIONS
        [DataMember]
        public SdkMessages Messages { get { return _Messages ?? new SdkMessages(new Dictionary<Guid, SdkMessage>()); } internal set { _Messages = value; } }
        private SdkMessages _Messages;
#endif
    }
}
