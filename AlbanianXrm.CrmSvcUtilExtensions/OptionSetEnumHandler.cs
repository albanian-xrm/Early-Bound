using AlbanianXrm.CrmSvcUtilExtensions.Extensions;
using AlbanianXrm.Extensions;
using Microsoft.Crm.Services.Utility;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace AlbanianXrm.CrmSvcUtilExtensions
{
    internal class OptionSetEnumHandler
    {
        CodeCompileUnit codeUnit;
        private HashSet<string> entities;
        private HashSet<string> allAttributes;
        private Dictionary<string, HashSet<string>> entityAttributes;
        private IServiceProvider services;
        private IOrganizationMetadata organizationMetadata;
        private readonly bool TwoOptionsEnum;
        private readonly bool TwoOptionsConstants;
        private readonly bool OptionSetEnumProperties;
        private string CrmSvcUtilsVersion;
        private string CrmSvcUtilsName;

        public OptionSetEnumHandler(CodeCompileUnit codeUnit, IServiceProvider services)
        {
            this.codeUnit = codeUnit;
            this.services = services;
            entities = new HashSet<string>((Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES) ?? "").Split(","));
            allAttributes = new HashSet<string>((Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_ALL_ATTRIBUTES) ?? "").Split(","));
            entityAttributes = new Dictionary<string, HashSet<string>>();
            foreach (var entity in entities.Except(allAttributes))
            {
                entityAttributes.Add(entity, new HashSet<string>((Environment.GetEnvironmentVariable(string.Format(Constants.ENVIRONMENT_ENTITY_ATTRIBUTES, entity)) ?? "").Split(",")));
            }
            TwoOptionsEnum = (Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_TWOOPTIONS) ?? "") == "1";
            TwoOptionsConstants = (Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_TWOOPTIONS) ?? "") == "2";
            OptionSetEnumProperties = (Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_OPTIONSETENUMPROPERTIES) ?? "") != "";
            CrmSvcUtilsVersion = FileVersionInfo.GetVersionInfo(typeof(SdkMessage).Assembly.Location).FileVersion;
            CrmSvcUtilsName = typeof(SdkMessage).Assembly.GetName().Name;
        }

        public void FixStateCode()
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
                                RemoveClass(property.Type.TypeArguments[0].BaseType);
                                thisType.Members.Remove(property);
                                break;
                            }
                        }
                    }
                }
            }
        }

        public void GenerateOptionSets()
        {
            var metadataProvider = (IMetadataProviderService)services.GetService(typeof(IMetadataProviderService));
            this.organizationMetadata = metadataProvider.LoadMetadata();
            GenerateOptionSetEnums();
        }

        private void GenerateOptionSetEnums()
        {
            foreach (CodeNamespace @namespace in codeUnit.Namespaces)
            {
                var globalOptionsetNames = new HashSet<string>();
                if (GetOrCreateOptionSets(@namespace.Types.ToEnumerable(), out CodeTypeDeclaration globalOptionSets))
                {
                    @namespace.Types.Add(globalOptionSets);
                }
                foreach (CodeTypeDeclaration type in @namespace.Types)
                {
                    if (!type.IsClass) continue;
                    var entity = GetEntityLogicalName(type);
                    if (entity == null) continue;
                    var entityMetadata = organizationMetadata.Entities.First(x => x.LogicalName == entity);
                    if (GetOrCreateOptionSets(type.Members.ToEnumerable<CodeTypeDeclaration>(), out CodeTypeDeclaration optionSets))
                    {
                        type.Members.Add(optionSets);
                    }
                    foreach (CodeTypeMember member in type.Members)
                    {
                        if (member is CodeMemberProperty property)
                        {
                            var attribute = GetAttributeLogicalName(member);
                            if (attribute == null) continue;
                            var attributeMetadata = entityMetadata.Attributes.FirstOrDefault(x => x.LogicalName == attribute);
                            CodeTypeMember generatedMember = null;
                            bool isGlobal = false;
                            if (attributeMetadata is BooleanAttributeMetadata booleanAttribute)
                            {
                                if (TwoOptionsEnum)
                                {
                                    generatedMember = GenerateEnumTwoOptions(booleanAttribute);
                                }
                                if (TwoOptionsConstants)
                                {
                                    generatedMember = GenerateConstantTwoOptions(booleanAttribute);
                                }
                                isGlobal = booleanAttribute.OptionSet.IsGlobal == true;
                            }
                            else if (attributeMetadata is StateAttributeMetadata stateAttribute)
                            {
                                generatedMember = GenerateEnumOptions(attributeMetadata.SchemaName, stateAttribute.OptionSet.Options);
                                isGlobal = stateAttribute.OptionSet.IsGlobal == true;
                            }
                            else if (attributeMetadata is StatusAttributeMetadata statusAttribute)
                            {
                                generatedMember = GenerateEnumOptions(attributeMetadata.SchemaName, statusAttribute.OptionSet.Options);
                                isGlobal = statusAttribute.OptionSet.IsGlobal == true;
                            }
                            else if (attributeMetadata is PicklistAttributeMetadata picklistAttribute)
                            {
                                generatedMember = GenerateEnumOptions(attributeMetadata.SchemaName, picklistAttribute.OptionSet.Options);
                                isGlobal = picklistAttribute.OptionSet.IsGlobal == true;
                            }
                            if (generatedMember == null) continue;
                            if (isGlobal)
                            {
                                if (globalOptionsetNames.Contains(generatedMember.Name)) continue;
                                globalOptionsetNames.Add(generatedMember.Name);
                                globalOptionSets.Members.Add(generatedMember);
                            }
                            else
                            {
                                optionSets.Members.Add(generatedMember);
                            }
                        }
                    }
                    if (optionSets.Members.Count == 0)
                    {
                        type.Members.Remove(optionSets);
                    }
                }
                if (globalOptionSets.Members.Count == 0)
                {
                    @namespace.Types.Remove(globalOptionSets);
                }
            }
        }

        private bool GetOrCreateOptionSets(IEnumerable<CodeTypeDeclaration> codeTypeDeclarations, out CodeTypeDeclaration optionSets)
        {
            foreach (CodeTypeDeclaration type in codeTypeDeclarations)
            {
                if (type.Name == "OptionSets")
                {
                    optionSets = type;
                    return false;
                }
            }
            optionSets = new CodeTypeDeclaration("OptionSets");
            optionSets.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;
            optionSets.IsClass = true;
            return true;
        }

        private CodeTypeMember GenerateEnumOptions(string schemaName, OptionMetadataCollection optionSet)
        {
            return GenerateEnumOptions(schemaName, EnumItem.ToUniqueValues(optionSet));
        }

        private CodeTypeMember GenerateEnumOptions(string schemaName, IEnumerable<EnumItem> values)
        {
            CodeTypeDeclaration optionSetEnum = new CodeTypeDeclaration(schemaName);
            optionSetEnum.IsEnum = true;
            optionSetEnum.CustomAttributes.Add(new CodeAttributeDeclaration("System.Runtime.Serialization.DataContractAttribute"));
            optionSetEnum.CustomAttributes.Add(new CodeAttributeDeclaration("System.CodeDom.Compiler.GeneratedCodeAttribute",
                                                       new CodeAttributeArgument(new CodePrimitiveExpression(CrmSvcUtilsName)),
                                                       new CodeAttributeArgument(new CodePrimitiveExpression(CrmSvcUtilsVersion))));
            foreach (var value in values)
            {
                CodeMemberField codeMemberField = new CodeMemberField(string.Empty, value.Value);
                codeMemberField.InitExpression = new CodePrimitiveExpression(value.Key);

                codeMemberField.Comments.Add(new CodeCommentStatement("<summary>", docComment: true));
                codeMemberField.Comments.Add(new CodeCommentStatement(System.Security.SecurityElement.Escape(value.Description), docComment: true));
                codeMemberField.Comments.Add(new CodeCommentStatement("</summary>", docComment: true));

                optionSetEnum.Members.Add(codeMemberField);
            }
            return optionSetEnum;
        }

        private CodeTypeMember GenerateEnumTwoOptions(BooleanAttributeMetadata booleanAttribute)
        {
            return GenerateEnumOptions(booleanAttribute.SchemaName, EnumItem.ToUniqueValues(booleanAttribute.OptionSet));
        }

        private CodeTypeMember GenerateConstantTwoOptions(BooleanAttributeMetadata booleanAttribute)
        {
            CodeTypeDeclaration booleanOptionSet = new CodeTypeDeclaration(booleanAttribute.SchemaName);

            booleanOptionSet.IsClass = true;
            booleanOptionSet.Attributes = MemberAttributes.Static | MemberAttributes.Public;
            var values = EnumItem.ToUniqueValues(booleanAttribute.OptionSet);
            foreach (var value in values)
            {
                CodeMemberField codeMemberField = new CodeMemberField(typeof(bool), value.Value);
                codeMemberField.InitExpression = new CodePrimitiveExpression(value.Key == 1);
                codeMemberField.Attributes = MemberAttributes.Const | MemberAttributes.Public;
                codeMemberField.Comments.Add(new CodeCommentStatement("<summary>", docComment: true));
                codeMemberField.Comments.Add(new CodeCommentStatement(System.Security.SecurityElement.Escape(value.Description), docComment: true));
                codeMemberField.Comments.Add(new CodeCommentStatement("</summary>", docComment: true));
                booleanOptionSet.Members.Add(codeMemberField);
            }
            return booleanOptionSet;
        }

        private void RemoveClass(string baseType)
        {
            foreach (CodeNamespace @namespace in codeUnit.Namespaces)
            {
                foreach (CodeTypeDeclaration type in @namespace.Types)
                {
                    if ($"{@namespace.Name}.{type.Name}" == baseType)
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
                if (attribute.Name == Constants.EntityLogicalNameAttributeType)
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
                if (attribute.Name == Constants.AttributeLogicalNameAttributeType)
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
