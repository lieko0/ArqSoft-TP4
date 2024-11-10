using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactor.utils;

namespace Refactor.dto;

public class FileClassDeclarations(CsFile file, ClassDeclarationSyntax classDeclaration)
{
    public CsFile parent { get; } = file;
    public ClassDeclarationSyntax classDeclaration { get;  } = classDeclaration;
}

public class CsFile
{
    public string path { get; }
    public SyntaxTree syntaxTree { get; }
    public FileClassDeclarations[] classDeclarations { get; }

    public CsFile(string path)
    {
        this.path = path;
        syntaxTree = GetSyntaxTree();
        classDeclarations = GetClassDeclarations()
            .Select(c => new FileClassDeclarations(this, c))
            .ToArray();
    }
        
    private SyntaxTree GetSyntaxTree()
    {
        var content = FilesManager.GetFileContent(path);
        return CSharpSyntaxTree.ParseText(content);
    }
    
    private List<ClassDeclarationSyntax> GetClassDeclarations()
    {
        return syntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().ToList();
    }
}