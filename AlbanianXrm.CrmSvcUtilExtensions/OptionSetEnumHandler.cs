﻿using AlbanianXrm.CrmSvcUtilExtensions.Extensions;
using AlbanianXrm.Extensions;
using Microsoft.PowerPlatform.Dataverse.ModelBuilderLib;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace AlbanianXrm.CrmSvcUtilExtensions
{
    internal class OptionSetEnumHandler
    {
        private readonly CodeCompileUnit codeUnit;
        private readonly HashSet<string> allAttributes;
        private readonly Dictionary<string, HashSet<string>> entityAttributes;
        private readonly IOrganizationMetadata organizationMetadata;
        private readonly bool TwoOptionsEnum;
        private readonly bool TwoOptionsConstants;
        private readonly bool OptionSetEnumProperties;
        private readonly string CrmSvcUtilsVersion;
        private readonly string CrmSvcUtilsName;
        private readonly bool removePropertyChanged;
        private readonly bool generateXmlDocumentation;

        public OptionSetEnumHandler(CodeCompileUnit codeUnit, IServiceProvider services, bool removePropertyChanged, bool generateXmlDocumentation)
        {
            this.codeUnit = codeUnit;
            var metadataProvider = (IMetadataProviderService)services.GetService(typeof(IMetadataProviderService));
            this.organizationMetadata = metadataProvider.LoadMetadata(services);
            var entities = new HashSet<string>((Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_ENTITIES) ?? "").Split(","));
            allAttributes = new HashSet<string>((Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_ALL_ATTRIBUTES) ?? "").Split(","));
            entityAttributes = new Dictionary<string, HashSet<string>>();
            foreach (var entity in entities.Except(allAttributes))
            {
                entityAttributes.Add(entity, new HashSet<string>((Environment.GetEnvironmentVariable(string.Format(CultureInfo.InvariantCulture, Constants.ENVIRONMENT_ENTITY_ATTRIBUTES, entity)) ?? "").Split(",")));
            }
            TwoOptionsEnum = (Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_TWOOPTIONS) ?? "") == "1";
            TwoOptionsConstants = (Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_TWOOPTIONS) ?? "") == "2";
            OptionSetEnumProperties = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_OPTIONSETENUMPROPERTIES));
            OptionSetEnumProperties = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(Constants.ENVIRONMENT_OPTIONSETENUMPROPERTIES));
            CrmSvcUtilsVersion = FileVersionInfo.GetVersionInfo(typeof(SdkMessage).Assembly.Location).FileVersion;
            CrmSvcUtilsName = typeof(SdkMessage).Assembly.GetName().Name;
            this.removePropertyChanged = removePropertyChanged;
            this.generateXmlDocumentation = generateXmlDocumentation;
        }

        public void FixStateCode()
        {
            foreach (CodeNamespace @namespace in codeUnit.Namespaces)
            {
                for (int i = 0; i < @namespace.Types.Count; i++)
                {
                    var thisType = @namespace.Types[i];
                    if (!thisType.IsClass)
                    {
                        if (thisType.Name.EndsWith("State", StringComparison.InvariantCulture))
                        {
                            @namespace.Types.RemoveAt(i);
                            i -= 1;
                        }
                        continue;
                    }
                    var entity = GetEntityLogicalName(thisType);
                    if (entity == null) continue;
                    for (int j = 0; j < thisType.Members.Count; j++)
                    {
                        if (thisType.Members[j] is CodeMemberProperty property)
                        {
                            var attribute = GetAttributeLogicalName(property);
                            if (attribute != "statecode") continue;
                            if(property.Type.TypeArguments.Count >= 1)
                            RemoveClass(property.Type.TypeArguments[0].BaseType);

                            if (allAttributes.Contains(entity) ||
                                entityAttributes.TryGetValue(entity, out HashSet<string> attributes) &&
                                attributes.Contains("statecode"))
                            {
                                if (!OptionSetEnumProperties)
                                {
                                    thisType.Members[j] = GenerateOptionSetProperty($"OptionSets.{property.Name}", property.Name, attribute, thisType.Members[j].Comments);
                                }
                                if (GetOrCreateOptionSets(thisType.Members.ToEnumerable<CodeTypeDeclaration>(), out CodeTypeDeclaration optionSets))
                                {
                                    thisType.Members.Add(optionSets);
                                }
                                GenerateOptionSetProperty(organizationMetadata.Entities.First(x => x.LogicalName == entity), attribute, thisType, j, @namespace.Name, property.Name,
                                                          new HashSet<string>(), new CodeTypeDeclaration(), optionSets);
                            }
                            else
                            {
                                thisType.Members.Remove(property);
                            }
                            break;
                        }

                    }
                }
            }
        }

        public void GenerateOptionSets()
        {
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
                    if (GetOrCreateOptionSets(type.Members.ToEnumerable<CodeTypeDeclaration>(), out CodeTypeDeclaration optionSets))
                    {
                        type.Members.Add(optionSets);
                    }
                    for (int i = 0; i < type.Members.Count; i++)
                    {
                        if (type.Members[i] is CodeMemberProperty property)
                        {
                            var attribute = GetAttributeLogicalName(property);
                            if (attribute == null || attribute == "statecode") continue;
                            GenerateOptionSetProperty(organizationMetadata.Entities.First(x => x.LogicalName == entity), attribute, type, i, @namespace.Name, property.Name,
                                                      globalOptionsetNames, globalOptionSets, optionSets);
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


        private void GenerateOptionSetProperty(EntityMetadata entityMetadata, string attribute, CodeTypeDeclaration type, int i, string @namespace, string property,
                                               HashSet<string> globalOptionsetNames, CodeTypeDeclaration globalOptionSets, CodeTypeDeclaration optionSets)
        {
            var attributeMetadata = entityMetadata.Attributes.FirstOrDefault(x => x.LogicalName == attribute);
            CodeTypeDeclaration generatedMember = null;
            bool isGlobal = false;
            bool isTwoOptions = false;
            if (attributeMetadata is BooleanAttributeMetadata booleanAttribute)
            {
                isGlobal = booleanAttribute.OptionSet.IsGlobal == true;
                isTwoOptions = true;
                if (TwoOptionsEnum)
                {
                    generatedMember = GenerateEnumTwoOptions(booleanAttribute);
                    if (OptionSetEnumProperties)
                    {
                        type.Members[i] = GenerateOptionSetProperty(/*(isGlobal ? "" : "") + */generatedMember.Name, property, attribute, type.Members[i].Comments, isTwoOptions: true);
                    }
                }
                if (TwoOptionsConstants)
                {
                    generatedMember = GenerateConstantTwoOptions(booleanAttribute);
                }
            }
            else if (attributeMetadata is StateAttributeMetadata stateAttribute)
            {
                generatedMember = GenerateEnumOptions(attributeMetadata.SchemaName, stateAttribute.OptionSet.Options);
                isGlobal = stateAttribute.OptionSet.IsGlobal == true;
                if (OptionSetEnumProperties)
                {
                    type.Members[i] = GenerateOptionSetProperty(/*(isGlobal ? "" : "") + */generatedMember.Name, property, attribute, type.Members[i].Comments, isTwoOptions: true);
                }
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
            if (generatedMember == null) return;
            if (isGlobal)
            {
                if (OptionSetEnumProperties)
                {
                    type.Members[i] = GenerateOptionSetProperty($"{@namespace}.OptionSets.{generatedMember.Name}", property, attribute, type.Members[i].Comments);
                }
                if (globalOptionsetNames.Contains(generatedMember.Name)) return;
                globalOptionsetNames.Add(generatedMember.Name);
                globalOptionSets.Members.Add(generatedMember);
            }
            else
            {
                if (OptionSetEnumProperties && (generatedMember.IsEnum || isTwoOptions))
                {
                    type.Members[i] = generatedMember.IsEnum ?
                                        GenerateOptionSetProperty($"OptionSets.{generatedMember.Name}", property, attribute, type.Members[i].Comments, isTwoOptions) :
                                        GenerateBoolProperty($"OptionSets.{generatedMember.Name}", property, attribute, type.Members[i].Comments);
                }
                optionSets.Members.Add(generatedMember);
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
            optionSets = new CodeTypeDeclaration("OptionSets")
            {
                TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed,
                IsClass = true
            };
            if (generateXmlDocumentation)
            {
                CustomizationService.AddMissingXmlDocumentation("OptionSets", optionSets.Comments);
            }
            return true;
        }

        private CodeTypeDeclaration GenerateEnumOptions(string schemaName, OptionMetadataCollection optionSet)
        {
            return GenerateEnumOptions(schemaName, EnumItem.ToUniqueValues(optionSet));
        }

        private CodeTypeDeclaration GenerateEnumOptions(string schemaName, IEnumerable<EnumItem> values)
        {
            CodeTypeDeclaration optionSetEnum = new CodeTypeDeclaration(schemaName)
            {
                IsEnum = true
            };
            optionSetEnum.CustomAttributes.Add(new CodeAttributeDeclaration("System.Runtime.Serialization.DataContractAttribute"));
            optionSetEnum.CustomAttributes.Add(new CodeAttributeDeclaration("System.CodeDom.Compiler.GeneratedCodeAttribute",
                                                    new CodeAttributeArgument(new CodePrimitiveExpression(CrmSvcUtilsName)),
                                                    new CodeAttributeArgument(new CodePrimitiveExpression(CrmSvcUtilsVersion))));
            if (generateXmlDocumentation)
            {
                CustomizationService.AddMissingXmlDocumentation(schemaName, optionSetEnum.Comments);
            }

            foreach (var value in values)
            {
                CodeMemberField codeMemberField = new CodeMemberField(string.Empty, value.Value)
                {
                    InitExpression = new CodePrimitiveExpression(value.Key)
                };

                if (generateXmlDocumentation)
                {
                    CustomizationService.AddMissingXmlDocumentation(value.Description, codeMemberField.Comments);
                }

                codeMemberField.CustomAttributes.Add(new CodeAttributeDeclaration("System.Runtime.Serialization.EnumMemberAttribute"));

                optionSetEnum.Members.Add(codeMemberField);
            }
            return optionSetEnum;
        }

        private CodeTypeDeclaration GenerateEnumTwoOptions(BooleanAttributeMetadata booleanAttribute)
        {
            return GenerateEnumOptions(booleanAttribute.SchemaName, EnumItem.ToUniqueValues(booleanAttribute.OptionSet));
        }

        private CodeTypeDeclaration GenerateConstantTwoOptions(BooleanAttributeMetadata booleanAttribute)
        {
            CodeTypeDeclaration booleanOptionSet = new CodeTypeDeclaration(booleanAttribute.SchemaName)
            {
                IsClass = true,
                Attributes = MemberAttributes.Static | MemberAttributes.Public
            };
            if (generateXmlDocumentation)
            {
                CustomizationService.AddMissingXmlDocumentation(booleanAttribute.SchemaName, booleanOptionSet.Comments);              
            }
            var values = EnumItem.ToUniqueValues(booleanAttribute.OptionSet);
            foreach (var value in values)
            {
                CodeMemberField codeMemberField = new CodeMemberField(typeof(bool), value.Value)
                {
                    InitExpression = new CodePrimitiveExpression(value.Key == 1),
                    Attributes = MemberAttributes.Const | MemberAttributes.Public
                };

                if (generateXmlDocumentation)
                {
                    CustomizationService.AddMissingXmlDocumentation(value.Description, codeMemberField.Comments);
                }

                booleanOptionSet.Members.Add(codeMemberField);
            }
            return booleanOptionSet;
        }

        internal CodeTypeMember GenerateOptionSetProperty(string type, string name, string logicalName, CodeCommentStatementCollection comments, bool isTwoOptions = false)
        {
            CodeMemberProperty property = new CodeMemberProperty
            {
                Type = new CodeTypeReference("System.Nullable", new CodeTypeReference(type)),
                Name = name,
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            property.CustomAttributes.Add(new CodeAttributeDeclaration("Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute",
                                                                      new CodeAttributeArgument(new CodePrimitiveExpression(logicalName))));
            if (generateXmlDocumentation)
            {
                property.Comments.AddRange(comments);
            }
            var typeReference = isTwoOptions ? new CodeTypeReference(typeof(Nullable)) { TypeArguments = { new CodeTypeReference(typeof(bool)) } } : new CodeTypeReference(typeof(Microsoft.Xrm.Sdk.OptionSetValue));
            property.GetStatements.Add(
                new CodeVariableDeclarationStatement(typeReference,
                                                     "attributeValue",
                                                      new CodeMethodInvokeExpression(
                                                          new CodeMethodReferenceExpression(new CodeThisReferenceExpression(),
                                                                                            "GetAttributeValue",
                                                                                            typeReference),
                                                          new CodeExpression[] { new CodePrimitiveExpression(logicalName) })));

            var getReturnStatements = new CodeStatement[isTwoOptions ? 3 : 1];
            if (isTwoOptions)
            {
                getReturnStatements[0] = new CodeVariableDeclarationStatement(
                                              new CodeTypeReference(typeof(int)),
                                              "value");
                getReturnStatements[1] = new CodeConditionStatement(
                                            new CodePropertyReferenceExpression(
                                                  new CodeVariableReferenceExpression("attributeValue"),
                                                  "Value"),
                                            new CodeStatement[]
                                            {
                                               new CodeAssignStatement(
                                                   new CodeVariableReferenceExpression("value"),
                                                   new CodePrimitiveExpression(1))
                                            },
                                            new CodeStatement[]
                                            {
                                               new CodeAssignStatement(
                                                   new CodeVariableReferenceExpression("value"),
                                                   new CodePrimitiveExpression(0))
                                            });

            }
            getReturnStatements[getReturnStatements.Length - 1] = new CodeMethodReturnStatement(
                              new CodeCastExpression(
                                  new CodeTypeReference(type),
                                  new CodeMethodInvokeExpression(
                                      new CodeMethodReferenceExpression(
                                          new CodeTypeReferenceExpression(typeof(Enum)),
                                          "ToObject"),
                                      new CodeTypeOfExpression(
                                          new CodeTypeReference(type)),
                                          isTwoOptions ?
                                            (CodeExpression)new CodeVariableReferenceExpression("value") :
                                             new CodePropertyReferenceExpression(
                                                  new CodeVariableReferenceExpression("attributeValue"),
                                                  "Value")
                                  )));

            property.GetStatements.Add(
                new CodeConditionStatement(
                      new CodeBinaryOperatorExpression(new CodeVariableReferenceExpression("attributeValue"), CodeBinaryOperatorType.IdentityInequality, new CodePrimitiveExpression(null)),
                      getReturnStatements,
                      new CodeStatement[]
                      {
                          new CodeMethodReturnStatement(new CodePrimitiveExpression(null))
                      }));


            if (!removePropertyChanged)
            {
                property.SetStatements.Add(
                    new CodeMethodInvokeExpression(
                        new CodeThisReferenceExpression(),
                        "OnPropertyChanging",
                        new CodeExpression[] { new CodePrimitiveExpression(name) }));
            }
            property.SetStatements.Add(
                new CodeConditionStatement(
                    new CodeBinaryOperatorExpression(new CodePropertySetValueReferenceExpression(), CodeBinaryOperatorType.ValueEquality, new CodePrimitiveExpression(null)),
                    new CodeStatement[] //true statements
                    {
                          new CodeExpressionStatement(  new CodeMethodInvokeExpression(
                                new CodeThisReferenceExpression(),
                                "SetAttributeValue",
                                new CodeExpression[] { new CodePrimitiveExpression(logicalName), new CodePropertySetValueReferenceExpression() }))
                    },
                    new CodeStatement[] //false statements
                    {
                           new CodeExpressionStatement(  new CodeMethodInvokeExpression(
                                new CodeThisReferenceExpression(),
                                "SetAttributeValue",
                                new CodeExpression[] {
                                    new CodePrimitiveExpression(logicalName),
                                       isTwoOptions ?
                                           new CodeBinaryOperatorExpression(new CodePrimitiveExpression(1), CodeBinaryOperatorType.ValueEquality, new CodeCastExpression(typeof(int), new CodePropertySetValueReferenceExpression())) :
                                           (CodeExpression) new CodeObjectCreateExpression(typeof(Microsoft.Xrm.Sdk.OptionSetValue), new CodeCastExpression(typeof(int), new CodePropertySetValueReferenceExpression())) }))
                    }
                ));
            if (!removePropertyChanged)
            {
                property.SetStatements.Add(
                    new CodeMethodInvokeExpression(
                        new CodeThisReferenceExpression(),
                        "OnPropertyChanged",
                        new CodeExpression[] { new CodePrimitiveExpression(name) }));
            }
            return property;
        }

        internal CodeTypeMember GenerateBoolProperty(string type, string name, string logicalName, CodeCommentStatementCollection comments)
        {
            CodeMemberProperty property = new CodeMemberProperty
            {
                Type = new CodeTypeReference("System.Nullable", new CodeTypeReference(typeof(bool))),
                Name = name,
                Attributes = MemberAttributes.Public | MemberAttributes.Final
            };
            property.CustomAttributes.Add(new CodeAttributeDeclaration("Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute",
                                                                      new CodeAttributeArgument(new CodePrimitiveExpression(logicalName))));
            if (generateXmlDocumentation)
            {
                property.Comments.AddRange(comments);
                property.Comments.Insert(property.Comments.Count - 1, new CodeCommentStatement($"Possible values can be found in <see cref=\"{type}\" />", docComment: true));
            }
            var typeReference = new CodeTypeReference(typeof(Nullable)) { TypeArguments = { new CodeTypeReference(typeof(bool)) } };
            property.GetStatements.Add(
                new CodeMethodReturnStatement(
                    new CodeMethodInvokeExpression(
                        new CodeMethodReferenceExpression(
                            new CodeThisReferenceExpression(),
                            "GetAttributeValue",
                            typeReference),
                        new CodeExpression[] { new CodePrimitiveExpression(logicalName) })));

            if (!removePropertyChanged)
            {
                property.SetStatements.Add(
                    new CodeMethodInvokeExpression(
                        new CodeThisReferenceExpression(),
                        "OnPropertyChanging",
                        new CodeExpression[] { new CodePrimitiveExpression(name) }));
            }
            property.SetStatements.Add(
                new CodeExpressionStatement(
                    new CodeMethodInvokeExpression(
                        new CodeThisReferenceExpression(),
                        "SetAttributeValue",
                        new CodeExpression[] { new CodePrimitiveExpression(logicalName), new CodePropertySetValueReferenceExpression() })));
            if (!removePropertyChanged)
            {
                property.SetStatements.Add(
                    new CodeMethodInvokeExpression(
                        new CodeThisReferenceExpression(),
                        "OnPropertyChanged",
                        new CodeExpression[] { new CodePrimitiveExpression(name) }));
            }
            return property;
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

        private static string GetEntityLogicalName(CodeTypeDeclaration type)
        {
            foreach (CodeAttributeDeclaration attribute in type.CustomAttributes)
            {
                if (attribute.Name == Constants.EntityLogicalNameAttributeType &&
                    attribute.Arguments[0].Value is CodePrimitiveExpression value)
                {
                    return (string)value.Value;
                }
            }
            return null;
        }

        private static string GetAttributeLogicalName(CodeTypeMember member)
        {
            if (member == null) throw new ArgumentNullException(nameof(member));

            foreach (CodeAttributeDeclaration attribute in member.CustomAttributes)
            {
                if (attribute.Name == Constants.AttributeLogicalNameAttributeType &&
                    attribute.Arguments[0].Value is CodePrimitiveExpression value)
                {
                    return (string)value.Value;
                }
            }
            return null;
        }
    }
}
