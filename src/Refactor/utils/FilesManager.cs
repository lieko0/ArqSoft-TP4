namespace Refactor.utils;

public static class FilesManager
{
    
    public static bool IsValidPath(string path)
    {
        return Directory.Exists(path) || (File.Exists(path) && path.EndsWith(".html"));
    }
    
    public static bool IsFolder(string path)
    {
        return Directory.Exists(path);
    }
    
    public static List<string> GetAllCsFilesFromDirectory(string path)
    {
        return Directory.GetFiles(path, "*.cs", SearchOption.AllDirectories).ToList();
    }
    
    public static void DeleteDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            
            Directory.Delete(path, true);
        }
    }

    public static void SaveFile(string dir, string path, string content)
    {
        if (!Directory.Exists(dir))
        { 
            Directory.CreateDirectory(dir);
        }
        
        File.WriteAllText(dir + "/" + path, content);
    }
    public static void SaveFile(string path, string content)
    {
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        
        File.WriteAllText(path, content);
    }
    
    public static string GetFileName(string path)
    {
        return Path.GetFileName(path);
    }
    
    public static string HashFileName(string path)
    {
        return Path.GetFileNameWithoutExtension(path) + "_" + Guid.NewGuid() + Path.GetExtension(path);
    }
    
    public static string GetFileContent(string path)
    {
        return File.ReadAllText(path);
    }

}