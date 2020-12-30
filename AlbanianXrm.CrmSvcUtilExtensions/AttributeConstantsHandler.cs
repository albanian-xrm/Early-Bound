using AlbanianXrm.CrmSvcUtilExtensions.Extensions;
using System.CodeDom;
using System.Linq;
using System.Reflection;

namespace AlbanianXrm.CrmSvcUtilExtensions
{
    internal class AttributeConstantsHandler
    {
        private readonly CodeCompileUnit codeUnit;
        private readonly bool generateXmlDocumentation;

        public AttributeConstantsHandler(CodeCompileUnit codeUnit, bool generateXmlDocumentation)
        {
            this.codeUnit = codeUnit;
            this.generateXmlDocumentation = generateXmlDocumentation;
        }

        internal void GenerateAttributeConstants()
        {
            foreach (CodeNamespace @namespace in codeUnit.Namespaces)
            {
                foreach (CodeTypeDeclaration type in @namespace.Types.ToEnumerable().Where(type => type.IsClass && type.GetEntityLogicalName() != null))
                {
                    GenerateAttributeConstants(type);
                }
            }
        }

        private void GenerateAttributeConstants(CodeTypeDeclaration type)
        {
            var fields = GetOrCreateClass("Fields", type.Members);
            foreach (CodeMemberProperty property in type.Members.ToEnumerable<CodeMemberProperty>())
            {
                var attributeLogicalName = property.GetAttributeLogicalName();
                if (attributeLogicalName == null) continue;
                fields.Members.Add(NewAttributeConstant(property, attributeLogicalName));
            }
        }

        private CodeMemberField NewAttributeConstant(CodeMemberProperty property, string attributeLogicalName)
        {
            CodeMemberField attributeConstant = new CodeMemberField(typeof(string), property.Name)
            {
                Attributes = MemberAttributes.Public | MemberAttributes.Const,
                InitExpression = new CodePrimitiveExpression(attributeLogicalName)
            };
            if (generateXmlDocumentation)
            {
                foreach (var documentation in property.Comments.ToEnumerable().Where(documentation => documentation.Comment.DocComment))
                {
                    attributeConstant.Comments.Add(documentation);
                }
            }
            return attributeConstant;
        }

        private static CodeTypeDeclaration GetOrCreateClass(string className, CodeTypeMemberCollection members)
        {
            var @class = members.ToEnumerable<CodeTypeDeclaration>()
                                .FirstOrDefault(_class => _class.Name == className);
            if (@class == null)
            {
                @class = new CodeTypeDeclaration(className)
                {
                    TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed,
                    IsClass = true
                };
                members.Add(@class);
            }
            return @class;
        }
    }
}
