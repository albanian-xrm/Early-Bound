namespace AlbanianXrm
{
    internal static class Constants
    {
        public const string CONSOLE_METADATA = ENVIRONMENT_VARIABLE_PREFIX + "[AlbanianXrm.EarlyBound:Metadata]";
        public const string CONSOLE_ENDSTREAM = ENVIRONMENT_VARIABLE_PREFIX + "[AlbanianXrm.EarlyBound:EndStream]";

        public const string ENVIRONMENT_VARIABLE_PREFIX = "AlbanianXrm.EarlyBound:";
        public const string ENVIRONMENT_ENTITIES = ENVIRONMENT_VARIABLE_PREFIX + "Entities";
        public const string ENVIRONMENT_ALL_ATTRIBUTES = ENVIRONMENT_VARIABLE_PREFIX + "AllAttributes";
        public const string ENVIRONMENT_ALL_RELATIONSHIPS = ENVIRONMENT_VARIABLE_PREFIX + "AllRelationships";
        public const string ENVIRONMENT_ENTITY_ATTRIBUTES = ENVIRONMENT_VARIABLE_PREFIX + "Attributes:{0}";
        public const string ENVIRONMENT_RELATIONSHIPS1N = ENVIRONMENT_VARIABLE_PREFIX + "Relationships1N:{0}";
        public const string ENVIRONMENT_RELATIONSHIPSN1 = ENVIRONMENT_VARIABLE_PREFIX + "RelationshipsN1:{0}";
        public const string ENVIRONMENT_RELATIONSHIPSNN = ENVIRONMENT_VARIABLE_PREFIX + "RelationshipsNN:{0}";
        public const string ENVIRONMENT_REMOVEPROPERTYCHANGED = ENVIRONMENT_VARIABLE_PREFIX + "RemovePropertyChanged";
        public const string ENVIRONMENT_REMOVEPROXYTYPESASSEMBLY = ENVIRONMENT_VARIABLE_PREFIX + "RemoveProxyTypesAssembly";
        public const string ENVIRONMENT_ATTACHDEBUGGER = ENVIRONMENT_VARIABLE_PREFIX + "AttachDebugger";
        public const string ENVIRONMENT_CACHEMEATADATA = ENVIRONMENT_VARIABLE_PREFIX + "CacheMetadata";
        public const string ENVIRONMENT_OPTIONSETENUMS = ENVIRONMENT_VARIABLE_PREFIX + "OptionSetEnums";
        public const string ENVIRONMENT_TWOOPTIONS = ENVIRONMENT_VARIABLE_PREFIX + "TwoOptions";
        public const string ENVIRONMENT_OPTIONSETENUMPROPERTIES = ENVIRONMENT_VARIABLE_PREFIX + "OptionSetEnumProperties";
        public const string ENVIRONMENT_REMOVEPUBLISHER = ENVIRONMENT_VARIABLE_PREFIX + "RemovePublisher";
        public const string ENVIRONMENT_GENERATEXML = ENVIRONMENT_VARIABLE_PREFIX + "GenerateXML";
        public const string ENVIRONMENT_FIXXML = ENVIRONMENT_VARIABLE_PREFIX + "FixXML";
        public const string ENVIRONMENT_ATTRIBUTECONSTANTS = ENVIRONMENT_VARIABLE_PREFIX + "GenerateAttributeConstants";

        public const string EntityLogicalNameAttributeType = "Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute";
        public const string AttributeLogicalNameAttributeType = "Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute";
    }
}
