using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Refactor.dto;

public class RefactorOportunity(FileClassDeclarations a, FileClassDeclarations b, MethodDeclarationSyntax methodA, MethodDeclarationSyntax methodB)
{
    public FileClassDeclarations fileA { get; } = a;
    public FileClassDeclarations fileB { get; } = b;
    public MethodDeclarationSyntax methodA { get; } = methodA;
    public MethodDeclarationSyntax methodB { get; } = methodB;
}