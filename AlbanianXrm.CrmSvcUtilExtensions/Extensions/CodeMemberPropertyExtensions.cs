using System;
using System.CodeDom;
using System.Linq;

namespace AlbanianXrm.CrmSvcUtilExtensions.Extensions
{
    public static class CodeMemberPropertyExtensions
    {
        public static string GetAttributeLogicalName(this CodeMemberProperty member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));

            return member.CustomAttributes.ToEnumerable()
                                     .Where(attribute => attribute.Name == Constants.AttributeLogicalNameAttributeType && attribute.Arguments.Count > 0)
                                     .Select(attribute => attribute.Arguments[0].Value as CodePrimitiveExpression)
                                     .Select(attributeArgument => attributeArgument.Value as string)
                                     .FirstOrDefault();
        }
    }
}
