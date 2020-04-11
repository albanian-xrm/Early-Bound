using System;
using System.CodeDom;
using System.Collections.Generic;

namespace AlbanianXrm.CrmSvcUtilExtensions.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<CodeTypeDeclaration> ToEnumerable(this CodeTypeDeclarationCollection collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            foreach (CodeTypeDeclaration codeTypeDeclaration in collection)
            {
                yield return codeTypeDeclaration;
            }
        }

        public static IEnumerable<CodeTypeMember> ToEnumerable(this CodeTypeMemberCollection collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            foreach (CodeTypeMember codeTypeMember in collection)
            {
                yield return codeTypeMember;
            }
        }

        public static IEnumerable<T> ToEnumerable<T>(this CodeTypeMemberCollection collection) where T : CodeTypeMember
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

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
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            foreach (CodeNamespace @namespace in collection)
            {
                yield return @namespace;
            }
        }

        public static IEnumerable<CodeAttributeDeclaration> ToEnumerable(this CodeAttributeDeclarationCollection collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            foreach (CodeAttributeDeclaration codeTypeDeclaration in collection)
            {
                yield return codeTypeDeclaration;
            }
        } 
        
        public static IEnumerable<CodeCommentStatement> ToEnumerable(this CodeCommentStatementCollection collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            foreach (CodeCommentStatement codeCommentStatement in collection)
            {
                yield return codeCommentStatement;
            }
        }
    }
}
