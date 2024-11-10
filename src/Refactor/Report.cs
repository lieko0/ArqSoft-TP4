using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactor.dto;
using Refactor.utils;

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
            var methodBodyA = methodA.Body?.ToString() ?? "";
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
            // Console.WriteLine("    {");
            Console.WriteLine($"    {methodBodyA}");
            // Console.WriteLine("    }");
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
    
    public static void GenerateFiles(Dictionary<MethodDeclarationSyntax, List<Relationship>> relationships)
    {
        foreach (var relationship in relationships.Keys)
        {
            var classDeclarationA = relationships[relationship][0].classDeclaration;
            var methodA = relationships[relationship][0].methodDeclaration;
            var fileA = classDeclarationA.parent.path;
            var fileADirectory = fileA.Substring(0, fileA.LastIndexOf('\\'));
            var methodParamsA = methodA.ParameterList.Parameters;
            var methodBodyA = methodA.Body?.ToString() ?? "";
            var returnTypeA = methodA.ReturnType.ToString();
            
            var foundClassesNames = relationships[relationship].Select(r => r.classDeclaration.classDeclaration.Identifier.Text).ToList();
            var foundMethodNames = relationships[relationship].Select(r => r.methodDeclaration.Identifier.Text).ToList();
            var newSuperclassName = "New"+classDeclarationA.classDeclaration.Identifier.Text;
            var newSuperclass = $"class {newSuperclassName}\n{{\n    public {returnTypeA} NewMethod({GetFormattedParamsList(methodParamsA)})\n    {{\n        {methodBodyA}\n    }}\n}}";
            var newSuperclassPath = Path.Combine(fileADirectory, $"generated/{newSuperclassName}.cs");

            if (!Directory.Exists(Path.Combine(fileADirectory, "generated")))
            {
                Directory.CreateDirectory(Path.Combine(fileADirectory, "generated"));
            }
            FilesManager.SaveFile(newSuperclassPath, newSuperclass);
            
            foreach (var relation in relationships[relationship])
            {
                var className = relation.classDeclaration.classDeclaration.Identifier.Text;
                var file = relation.classDeclaration.parent.path;
                
                var newClass = $"class {className} : {newSuperclassName}\n{{\n    public {returnTypeA} NewMethod({GetFormattedParamsList(methodParamsA)}) : base({GetFormattedParamsList(methodParamsA)})\n    {{\n          {methodBodyA}\n     }}\n}}";
                var newClassPath = Path.Combine(file.Substring(0, file.LastIndexOf('\\')), $"generated\\{className}.cs");
                FilesManager.SaveFile(newClassPath, newClass);
            }
        }
    }
    private static string GetFormattedMethodListWithExclude(IEnumerable<MethodDeclarationSyntax> methods)
    {
        // WIP
        return string.Join("\n", methods.Select(m =>  $"{m.ReturnType} {m.Identifier} ({GetFormattedParamsList(m.ParameterList.Parameters)}) {{\n{m.Body}\n}}"));;
    }

    private static string GetFormattedParamsList(IEnumerable<ParameterSyntax> parameters)
    {
        return string.Join(", ", parameters.Select(p =>  $"{p.Type} {p.Identifier}"));;
    }
}