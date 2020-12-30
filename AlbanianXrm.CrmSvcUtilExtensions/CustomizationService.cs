using Microsoft.Crm.Services.Utility;
using System;
using System.CodeDom;

namespace AlbanianXrm.CrmSvcUtilExtensions
{
    public sealed class CustomizationService : ICustomizeCodeDomService
    {

        public CustomizationService()
        {
#if DEBUG
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_ATTACHDEBUGGER)))
            {
                System.Diagnostics.Debugger.Launch();
            }
#endif         
        }

        public void CustomizeCodeDom(CodeCompileUnit codeUnit, IServiceProvider services)
        {
            if (codeUnit == null) throw new ArgumentNullException(nameof(codeUnit));
            if (services == null) throw new ArgumentNullException(nameof(services));

            var removePropertyChanged = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_REMOVEPROPERTYCHANGED));
            var generateXmlDocumentation = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_GENERATEXML));

            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_ATTRIBUTECONSTANTS)))
            {
                var attributeConstantsHandler = new AttributeConstantsHandler(codeUnit, generateXmlDocumentation);
                attributeConstantsHandler.GenerateAttributeConstants();
            }

            var optionSetEnumHandler = new OptionSetEnumHandler(codeUnit, services, removePropertyChanged, generateXmlDocumentation);
            optionSetEnumHandler.FixStateCode();
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_OPTIONSETENUMS)))
            {
                optionSetEnumHandler.GenerateOptionSets();
            }

            if (removePropertyChanged)
            {
                for (var i = 0; i < codeUnit.Namespaces.Count; i++)
                {
                    var types = codeUnit.Namespaces[i].Types;
                    for (var j = 0; j < types.Count; j++)
                    {
                        var thisType = types[j];
                        if (!thisType.IsClass) continue;
                        for (var k = 0; k < thisType.BaseTypes.Count; k++)
                        {
                            var baseType = thisType.BaseTypes[k];
                            if (baseType.BaseType == "System.ComponentModel.INotifyPropertyChanging" ||
                               baseType.BaseType == "System.ComponentModel.INotifyPropertyChanged")
                            {
                                thisType.BaseTypes.RemoveAt(k);
                                k -= 1;
                            }
                        }

                        for (var k = 0; k < thisType.Members.Count; k++)
                        {
                            var member = thisType.Members[k];
                            if (member.Name == "PropertyChanging" ||
                                member.Name == "PropertyChanged" ||
                                member.Name == "OnPropertyChanging" ||
                                member.Name == "OnPropertyChanged")
                            {
                                thisType.Members.RemoveAt(k);
                                k -= 1;
                                continue;
                            }

                            if (member is CodeMemberProperty property)
                            {
                                for (var l = 0; l < property.SetStatements.Count; l++)
                                {
                                    var statement = property.SetStatements[l];
                                    if (statement is CodeExpressionStatement codeStatement &&
                                        codeStatement.Expression is CodeMethodInvokeExpression methodInvoke &&
                                        (methodInvoke.Method.MethodName == "OnPropertyChanging" || methodInvoke.Method.MethodName == "OnPropertyChanged"))
                                    {
                                        property.SetStatements.RemoveAt(l);
                                        l -= 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (!generateXmlDocumentation)
            {
                RemoveXmlDocumentation(codeUnit.Namespaces);
            }
        }

        private static void RemoveXmlDocumentation(CodeNamespaceCollection namespaces)
        {
            foreach (CodeNamespace @namespace in namespaces)
            {
                RemoveXmlDocumentation(@namespace.Comments);
                foreach (CodeTypeDeclaration typeDeclaration in @namespace.Types)
                {
                    RemoveXmlDocumentation(typeDeclaration.Comments);
                    foreach (CodeTypeMember typeMember in typeDeclaration.Members)
                    {
                        RemoveXmlDocumentation(typeMember.Comments);
                    }
                }
            }
        }

        private static void RemoveXmlDocumentation(CodeCommentStatementCollection comments)
        {
            for (int i = 0; i < comments.Count; i++)
            {
                if (comments[i].Comment.DocComment)
                {
                    comments.RemoveAt(i);
                    i -= 1;
                }
            }
        }
    }
}
