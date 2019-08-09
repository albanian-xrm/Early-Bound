using Microsoft.Xrm.Sdk.Metadata;
using System.Reflection;

namespace AlbanianXrm.CrmSvcUtilExtensions.Tests
{
    static class Extensions
    {
        public static void SetEntityLogicalName(this AttributeMetadata attributeMetadata, string value)
        {
            typeof(AttributeMetadata)
                .GetField("_entityLogicalName", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(attributeMetadata, value);
        }
    }
}
