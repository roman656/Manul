using System.IO;
using Manul.Exceptions;

namespace Manul;

public static class Utils
{
    public static void CheckFile(string filename)
    {
        var fileInfo = new FileInfo(filename);

        if (!fileInfo.Exists)
        {
            throw new FileNotFoundException($"File {filename} not found");
        }

        if (fileInfo.Length <= 0)
        {
            throw new EmptyFileException($"File {filename} is empty");
        }
    }
}