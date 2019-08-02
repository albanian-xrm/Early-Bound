using Microsoft.Crm.Services.Utility;
using System;
using System.CodeDom;
using System.Linq;

namespace AlbanianXrm.CrmSvcUtilExtensions
{
    public sealed class CustomizationService : ICustomizeCodeDomService
    {
        public void CustomizeCodeDom(CodeCompileUnit codeUnit, IServiceProvider services)
        {
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
    }
}
