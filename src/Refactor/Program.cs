using Refactor.utils;

namespace Refactor;

class Program
{
    static void Main(string[] args)
    {
        // Definindo o caminho do arquivo na pasta "testes" na raiz do projeto
        string rootPath = Directory.GetCurrentDirectory();
        string dirPath = Path.Combine(rootPath, "testes");

        if (!FilesManager.IsFolder(dirPath))
        {
            Console.WriteLine($"Arquivo não encontrado no caminho: {dirPath}");
            return;
        }

        var files = FilesManager.GetAllCsFilesFromDirectory(dirPath);

        var analyzer = new Analyzer(files);
        analyzer.Analyze();
        
        var opportunities = analyzer.GetRefactorOpportunities();
        
        Report.GenerateReport(opportunities);
    }
    
}
