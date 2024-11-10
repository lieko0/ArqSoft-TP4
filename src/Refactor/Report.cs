using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactor.dto;

namespace Refactor;

public static class Report
{
    public static void GenerateReport(Dictionary<MethodDeclarationSyntax, List<Relationship>> relationships) 
    {
        foreach (var relationship in relationships.Keys)
        {
            var classDeclarationA = relationships[relationship][0].classDeclaration;
            var methodA = relationships[relationship][0].methodDeclaration;
            var fileA = classDeclarationA.parent.path;
            var methodParamsA = methodA.ParameterList.Parameters;
            var returnTypeA = methodA.ReturnType.ToString();
            
            var foundClassesNames = relationships[relationship].Select(r => r.classDeclaration.classDeclaration.Identifier.Text).ToList();
            var foundMethodNames = relationships[relationship].Select(r => r.methodDeclaration.Identifier.Text).ToList();
            Console.WriteLine($"Oportunidade de refatoração encontrada nas classes {string.Join(", ", foundClassesNames)}");
            Console.WriteLine();

            foreach (var relation in relationships[relationship])
            {
                var className = relation.classDeclaration.classDeclaration.Identifier.Text;
                var file = relation.classDeclaration.parent.path;
                Console.WriteLine($"{className}: {file}");
            }
            
            Console.WriteLine();
            Console.WriteLine($"Métodos {string.Join(", ", foundMethodNames)} são similares");
            Console.WriteLine();
            Console.WriteLine("Sugestão de refatoração:");
            Console.WriteLine();
            Console.WriteLine("Crie uma nova classe e implemente um método comum. Ex.:");
            Console.WriteLine();
            Console.WriteLine($"class NewSuperclass");
            Console.WriteLine("{");
            Console.WriteLine($"    public {returnTypeA} NewMethod({methodParamsA})");
            Console.WriteLine("    {");
            Console.WriteLine("        // Implementação do método comum");
            Console.WriteLine("    }");
            Console.WriteLine("}");
            Console.WriteLine();
            Console.WriteLine($"Modifique as subclasses para utilizarem a nova classe:");
            
            foreach (var relation in relationships[relationship])
            {
                var className = relation.classDeclaration.classDeclaration.Identifier.Text;
                
                Console.WriteLine();
                Console.WriteLine($"class {className} : NewSuperclass");
                Console.WriteLine("{");
                Console.WriteLine($"    public {returnTypeA} NewMethod({methodParamsA}) : base({methodParamsA})");
                Console.WriteLine("          // Mantenha apenas membros específicos dessa classe");
                Console.WriteLine("     }");
                Console.WriteLine("}");
                Console.WriteLine();
            }
            
            Console.WriteLine("\n----------------------------------------------------------------");
            Console.WriteLine("----------------------------------------------------------------\n");
        }
    }
    
    private static string GetFormattedParamsList(IEnumerable<ParameterSyntax> parameters)
    {
        return string.Join(", ", parameters.Select(p =>  $"{p.Type} {p.Identifier}"));;
    }
}