namespace Refactor.utils;

class RabinKarpSimilarity
{
    private const int d = 256; // Number of characters in the input alphabet
    private const int q = 101; // A prime number for hashing

    // Method to compute rolling hash for a given string
    private static List<long> ComputeRollingHashes(string str, int subLen)
    {
        List<long> hashes = new List<long>();
        int n = str.Length;
        long h = 1;

        // Calculate h = d^(subLen-1) % q
        for (int i = 0; i < subLen - 1; i++)
            h = (h * d) % q;

        // Compute the initial hash value
        long hash = 0;
        for (int i = 0; i < subLen; i++)
            hash = (hash * d + str[i]) % q;

        hashes.Add(hash);

        // Compute the rolling hashes
        for (int i = 1; i <= n - subLen; i++)
        {
            hash = (d * (hash - str[i - 1] * h) + str[i + subLen - 1]) % q;
            if (hash < 0)
                hash = hash + q;

            hashes.Add(hash);
        }

        return hashes;
    }

    // Method to calculate similarity score between two strings
    public static double CalculateSimilarity(string str1, string str2)
    {
        var subLen = GetSublenValue(str1, str2);
        var hashes1 = ComputeRollingHashes(str1, subLen);
        var hashes2 = ComputeRollingHashes(str2, subLen);

        HashSet<long> set1 = new HashSet<long>(hashes1);
        HashSet<long> set2 = new HashSet<long>(hashes2);

        // Find the number of matching hashes
        set1.IntersectWith(set2);
        int matches = set1.Count;

        // Calculate similarity score as ratio of matches to total substrings
        int totalSubstrings = Math.Min(hashes1.Count, hashes2.Count);
        return (double)matches / totalSubstrings;
    }
    
    private static int GetSublenValue(string str1, string str2)
    {
        string[] str1Lines = str1.Split('\n');
        string[] str2Lines = str2.Split('\n');
        int avgStr1LineLength = str1Lines.Sum(l => l.Length) / str1Lines.Length;
        int avgStr2LineLength = str2Lines.Sum(l => l.Length) / str2Lines.Length;
        int avgLinesLength = (avgStr1LineLength + avgStr2LineLength) / 2;
        
        return avgLinesLength;
    }
    
}
