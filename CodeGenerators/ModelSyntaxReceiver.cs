using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGenerators
{
    public class ModelSyntaxReceiver : ISyntaxReceiver
    {
        public List<ClassDeclarationSyntax> Candidates { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (syntaxNode is not AttributeSyntax attribute)
                return;

            var name = ExtractName(attribute.Name);

            if (name != "SwitchGeneration" && name != "SwitchGenerationAttribute")
                return;

            // "attribute.Parent" is "AttributeListSyntax"
            // "attribute.Parent.Parent" is a C# fragment the attribute is applied to
            if (attribute.Parent?.Parent is ClassDeclarationSyntax classDeclaration)
                Candidates.Add(classDeclaration);
        }

        private static string ExtractName(TypeSyntax type)
        {
            while (type != null)
            {
                switch (type)
                {
                    case IdentifierNameSyntax ins:
                        return ins.Identifier.Text;
                    case QualifiedNameSyntax qns:
                        type = qns.Right;
                        break;
                    default:
                        return null;
                }
            }

            return null;
        }
    }
}
