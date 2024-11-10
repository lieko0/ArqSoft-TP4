using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Refactor.dto;
using Refactor.utils;

namespace Refactor;

public class Analyzer
{
    private const double MAX_ALLOWED_SIMILARITY = 0.8;
    private FileClassDeclarations[] ClassDeclarations { get; }

    private readonly List<RefactorOportunity> _refactorOpportunities  = new();

    public Analyzer(IEnumerable<string> files)
    {
        CsFile[] syntaxTrees = files.Select(f => new CsFile(f)).ToArray();
        ClassDeclarations = syntaxTrees.SelectMany(f => f.classDeclarations).ToArray();
    }

    public void Analyze()
    {
        CheckSimilarNodes();
    }
    
    public Dictionary<MethodDeclarationSyntax, List<Relationship>> GetRefactorOpportunities()
    {
        var relationships = new Dictionary<MethodDeclarationSyntax, List<Relationship>>();
        foreach (var opportunity in _refactorOpportunities)
        {
            if (relationships.ContainsKey(opportunity.methodA))
            {
                var relationship = new Relationship(opportunity.fileB, opportunity.methodB);
                relationships[opportunity.methodA].Add(relationship);
            }
            else
            {
                var methodA = new Relationship(opportunity.fileA, opportunity.methodA);
                var methodB = new Relationship(opportunity.fileB, opportunity.methodB);
                var list = new List<Relationship> {methodA, methodB};
                
                relationships[opportunity.methodA] = list;
            }
        }
        return relationships;
    }

    private void CheckSimilarNodes()
    {
        for (int i = 0; i < ClassDeclarations.Length; i++)
        {
            var aClassMethods = ClassDeclarations[i]
                .classDeclaration.Members.OfType<MethodDeclarationSyntax>()
                .ToList();
            for (int j = i + 1; j < ClassDeclarations.Length; j++)
            {
                var bClassMethods = ClassDeclarations[j]
                    .classDeclaration.Members.OfType<MethodDeclarationSyntax>()
                    .ToList();
                CheckMethodsSimilarity(aClassMethods, bClassMethods, ClassDeclarations[i], ClassDeclarations[j]);
            }
        }
    }

    private void CheckMethodsSimilarity(List<MethodDeclarationSyntax> aClassMethods,
        List<MethodDeclarationSyntax> bClassMethods, FileClassDeclarations a, FileClassDeclarations b)
    {
        foreach (var aMethod in aClassMethods)
        {
            foreach (var bMethod in bClassMethods)
            {
                var sameReturnType = HasSameReturnType(aMethod, bMethod);
                var sameParameters = HasSameParameters(aMethod, bMethod);
                var similarName = CheckNameSimilarity(aMethod, bMethod);
                var similarBody = CheckBodySimilarity(aMethod, bMethod);
                
                Console.WriteLine($"Method {aMethod.Identifier.Text} and {bMethod.Identifier.Text} \t\t\t Type: {sameReturnType} \t Parameters: {sameParameters} \t Name: {similarName} \t Body: {similarBody}");

                if (sameReturnType && sameParameters && similarName && similarBody)
                {
                    _refactorOpportunities.Add(new RefactorOportunity(a, b, aMethod, bMethod));
                }
            }
        }
    }

    private bool HasSameReturnType(MethodDeclarationSyntax a, MethodDeclarationSyntax b)
    {
        return a.ReturnType.ToString() == b.ReturnType.ToString();
    }

    private bool HasSameParameters(MethodDeclarationSyntax a, MethodDeclarationSyntax b)
    {
        var aParams = a.ParameterList.Parameters.Select(p => p.ToString()).ToList();
        var bParams = b.ParameterList.Parameters.Select(p => p.ToString()).ToList();
        return aParams.SequenceEqual(bParams);
    }
    
    private bool CheckNameSimilarity(MethodDeclarationSyntax a, MethodDeclarationSyntax b)
    {
        return RabinKarpSimilarity.CalculateSimilarity(a.Identifier.Text, b.Identifier.Text, 3) >= MAX_ALLOWED_SIMILARITY;
    }
    private bool CheckBodySimilarity(MethodDeclarationSyntax a, MethodDeclarationSyntax b)
    {
        var aBody = a.Body?.ToString() ?? "";
        var aBodyAsTokens = a.DescendantNodesAndTokens()?.Select(t => t.Kind().ToString()).ToList();
        var aBodyAsTokensLines = a.DescendantNodesAndTokens()?.Select(l =>
            (l.GetLocation()?.ToString(), l.ToString(), l.Kind().ToString())
        ).ToList();
        /*if (aBodyAsTokens != null)
        {
            foreach (var token in aBodyAsTokens)
            {
                Console.WriteLine($"{token}");
            }
        }*/
        if (aBodyAsTokensLines != null)
        {
            foreach (var token in aBodyAsTokensLines)
            {
                Console.WriteLine($"{token}");
            }
        }
        var bBody = b.Body?.ToString() ?? "";
        return RabinKarpSimilarity.CalculateSimilarity(aBody, bBody, RabinKarpSimilarity.GetAvgLinesLength(aBody,bBody)) >= MAX_ALLOWED_SIMILARITY;
    }
}