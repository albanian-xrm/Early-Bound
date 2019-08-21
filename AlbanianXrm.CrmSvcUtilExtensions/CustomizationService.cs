using AlbanianXrm.Extensions;
using Microsoft.Crm.Services.Utility;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace AlbanianXrm.CrmSvcUtilExtensions
{
    public sealed class CustomizationService : ICustomizeCodeDomService
    {

        public CustomizationService()
        {
#if DEBUG
            if ((Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_ATTACHDEBUGGER) ?? "") != "")
            {
                System.Diagnostics.Debugger.Launch();
            }
#endif
            entities = new HashSet<string>((Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES) ?? "").Split(","));
            allAttributes = new HashSet<string>((Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_ALL_ATTRIBUTES) ?? "").Split(","));
            entityAttributes = new Dictionary<string, HashSet<string>>();
            foreach (var entity in entities.Except(allAttributes))
            {
                entityAttributes.Add(entity, new HashSet<string>((Environment.GetEnvironmentVariable(string.Format(Constants.ENVIRONMENT_ENTITY_ATTRIBUTES, entity)) ?? "").Split(",")));
            }
        }

        private HashSet<string> entities;
        private HashSet<string> allAttributes;
        private Dictionary<string, HashSet<string>> entityAttributes;

        public void CustomizeCodeDom(CodeCompileUnit codeUnit, IServiceProvider services)
        {
            FixStateCode(codeUnit);
            var removePropertyChanged = (Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_REMOVEPROPERTYCHANGED) ?? "") != "";
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
                                    if (statement is CodeExpressionStatement codeStatement)
                                    {
                                        if (codeStatement.Expression is CodeMethodInvokeExpression methodInvoke)
                                        {
                                            if (methodInvoke.Method.MethodName == "OnPropertyChanging" ||
                                                methodInvoke.Method.MethodName == "OnPropertyChanged")
                                            {
                                                property.SetStatements.RemoveAt(l);
                                                l -= 1;
                                                continue;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void FixStateCode(CodeCompileUnit codeUnit)
        {
            foreach (CodeNamespace @namespace in codeUnit.Namespaces)
            {
                for (int i = 0; i < @namespace.Types.Count; i++)
                {
                    var thisType = @namespace.Types[i];
                    if (!thisType.IsClass) continue;
                    var entity = GetEntityLogicalName(thisType);
                    if (entity == null || allAttributes.Contains(entity)) continue;
                    if (entityAttributes.TryGetValue(entity, out HashSet<string> attributes) &&
                        !attributes.Contains("statecode"))
                    {
                        for (int j = 0; j < thisType.Members.Count; j++)
                        {
                            if (thisType.Members[j] is CodeMemberProperty property)
                            {
                                var attribute = GetAttributeLogicalName(property);
                                if (attribute != "statecode") continue;                               
                                RemoveClass(codeUnit, property.Type.TypeArguments[0].BaseType); 
                                thisType.Members.Remove(property);
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void RemoveClass(CodeCompileUnit codeUnit, string baseType)
        {
            foreach (CodeNamespace @namespace in codeUnit.Namespaces)
            {
                foreach (CodeTypeDeclaration type in @namespace.Types)
                {
                    if ($"{@namespace.Name}.{type.Name}"==baseType)
                    {
                        @namespace.Types.Remove(type);
                        return;
                    }
                }
            }
        }

        private string GetEntityLogicalName(CodeTypeDeclaration type)
        {
            foreach (CodeAttributeDeclaration attribute in type.CustomAttributes)
            {
                if (attribute.Name == "Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute")
                {
                    if (attribute.Arguments[0].Value is CodePrimitiveExpression value)
                    {
                        return (string)value.Value;
                    }
                }
            }
            return null;
        }

        private string GetAttributeLogicalName(CodeTypeMember member)
        {
            foreach (CodeAttributeDeclaration attribute in member.CustomAttributes)
            {
                if (attribute.Name == "Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute")
                {
                    if (attribute.Arguments[0].Value is CodePrimitiveExpression value)
                    {
                        return (string)value.Value;
                    }
                }
            }
            return null;
        }
    }
}
