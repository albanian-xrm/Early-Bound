using System;
using System.CodeDom;
using System.Collections.Generic;

namespace AlbanianXrm.CrmSvcUtilExtensions.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<CodeTypeDeclaration> ToEnumerable(this CodeTypeDeclarationCollection collection)
        {
            return SafeToEnumerable(collection ?? throw new ArgumentNullException(nameof(collection)));
        }

        private static IEnumerable<CodeTypeDeclaration> SafeToEnumerable(this CodeTypeDeclarationCollection collection)
        {
            foreach (CodeTypeDeclaration codeTypeDeclaration in collection)
            {
                yield return codeTypeDeclaration;
            }
        }

        public static IEnumerable<CodeTypeMember> ToEnumerable(this CodeTypeMemberCollection collection)
        {
            return SafeToEnumerable(collection ?? throw new ArgumentNullException(nameof(collection)));
        }

        private static IEnumerable<CodeTypeMember> SafeToEnumerable(CodeTypeMemberCollection collection)
        {
            foreach (CodeTypeMember codeTypeMember in collection)
            {
                yield return codeTypeMember;
            }
        }

        public static IEnumerable<T> ToEnumerable<T>(this CodeTypeMemberCollection collection) where T : CodeTypeMember
        {
            return SafeToEnumerable<T>(collection ?? throw new ArgumentNullException(nameof(collection)));
        }

        private static IEnumerable<T> SafeToEnumerable<T>(CodeTypeMemberCollection collection) where T : CodeTypeMember
        {
            foreach (CodeTypeMember codeTypeMember in collection)
            {
                if (codeTypeMember is T tMember)
                {
                    yield return tMember;
                }
            }
        }

        public static IEnumerable<CodeNamespace> ToEnumerable(this CodeNamespaceCollection collection)
        {
            return SafeToEnumerable(collection ?? throw new ArgumentNullException(nameof(collection)));
        }

        private static IEnumerable<CodeNamespace> SafeToEnumerable(CodeNamespaceCollection collection)
        {
            foreach (CodeNamespace @namespace in collection)
            {
                yield return @namespace;
            }
        }

        public static IEnumerable<CodeAttributeDeclaration> ToEnumerable(this CodeAttributeDeclarationCollection collection)
        {
            return SafeToEnumerable(collection ?? throw new ArgumentNullException(nameof(collection)));
        }

        private static IEnumerable<CodeAttributeDeclaration> SafeToEnumerable(CodeAttributeDeclarationCollection collection)
        {
            foreach (CodeAttributeDeclaration codeTypeDeclaration in collection)
            {
                yield return codeTypeDeclaration;
            }
        }

        public static IEnumerable<CodeCommentStatement> ToEnumerable(this CodeCommentStatementCollection collection)
        {
            return SafeToEnumerable(collection ?? throw new ArgumentNullException(nameof(collection)));
        }

        private static IEnumerable<CodeCommentStatement> SafeToEnumerable(CodeCommentStatementCollection collection)
        {
            foreach (CodeCommentStatement codeCommentStatement in collection)
            {
                yield return codeCommentStatement;
            }
        }
    }
}
