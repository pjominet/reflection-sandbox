﻿using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using System.Linq;

// https://www.thinktecture.com/en/net/roslyn-source-generators-introduction/
namespace CodeGenerators
{
    [Generator]
    public class MappingGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new ModelSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var receiver = (ModelSyntaxReceiver)context.SyntaxReceiver;

            foreach (var classDeclaration in receiver.Candidates)
            {
                var model = context.Compilation.GetSemanticModel(classDeclaration.SyntaxTree, true);
                var type = model.GetDeclaredSymbol(classDeclaration) as ITypeSymbol;

                if (type is null || !IsEnumeration(type))
                    continue;

                var code = GenerateCode(type);
                context.AddSource($"{type.Name}_Generated.cs", code);
            }
        }

        private static bool IsEnumeration(ISymbol type)
        {
            return type.GetAttributes().Any(a => a.AttributeClass?.ToString() == $"{nameof(CodeGenerators)}.{nameof(SwitchGenerationAttribute)}");
        }

        private static string GenerateCode(ITypeSymbol type)
        {
            var ns = type.ContainingNamespace.ToString();
            var name = type.Name;
            var items = GetItemNames(type);

            return @$"// <auto-generated />

using System.Collections.Generic;

{(string.IsNullOrWhiteSpace(ns) ? null : $"namespace {ns}")}
{{
   partial class {name}
   {{
      private static IReadOnlyList<{name}> _items;
      public static IReadOnlyList<{name}> Items => _items ??= GetItems();

      private static IReadOnlyList<{name}> GetItems()
      {{
         return new[] {{ {string.Join(", ", items)} }};
      }}
   }}
}}
";
        }

        private static IEnumerable<string> GetItemNames(ITypeSymbol type)
        {
            return type.GetMembers()
                .Select(m =>
                {
                    if (m.DeclaredAccessibility != Accessibility.Public || m is not IFieldSymbol field)
                        return null;

                    return field.Name;
                })
                .Where(field => field is not null);
        }
    }
}
