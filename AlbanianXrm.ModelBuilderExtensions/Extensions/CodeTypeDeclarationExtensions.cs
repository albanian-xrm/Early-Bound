using System;
using System.CodeDom;
using System.Linq;

namespace AlbanianXrm.CrmSvcUtilExtensions.Extensions
{
    public static class CodeTypeDeclarationExtensions
    {
        public static string GetEntityLogicalName(this CodeTypeDeclaration type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.CustomAttributes.ToEnumerable()
                                  .Where(attribute => attribute.Name == Constants.EntityLogicalNameAttributeType && attribute.Arguments.Count > 0)
                                  .Select(attribute => attribute.Arguments[0].Value as CodePrimitiveExpression)
                                  .Select(attributeArgument => attributeArgument.Value as string)
                                  .FirstOrDefault();
        }
    }
}
