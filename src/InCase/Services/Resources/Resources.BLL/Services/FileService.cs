using ImageMagick;
using Resources.BLL.Exceptions;
using System.Text.RegularExpressions;

namespace Resources.BLL.Services;
public static class FileService
{
    private const string PathUri = "/static/images/";

    public static void UploadImageBase64(string base64, string filePath, string fileName)
    {
        (var extensionFile, base64) = SplitBase64(base64);

        fileName += extensionFile;

        var absolutePath = PathUri + filePath + fileName;

        CreateFolder(filePath);

        try
        {
            File.WriteAllBytes(absolutePath, Convert.FromBase64String(base64));
            Compress(absolutePath);
        }
        catch (Exception)
        {
            throw new ConflictException($"Не удалось создать файл {fileName}");
        }
    }

    public static void RemoveFolder(string path)
    {
        var absolutePath = PathUri + path;

        if (!Directory.Exists(absolutePath)) return;

        try
        {
            Directory.Delete(absolutePath, recursive: true);
        }
        catch (Exception)
        {
            throw new ConflictException($"Не удалось удалить папку {absolutePath}");
        }
    }

    public static void CreateFolder(string path)
    {
        var absolutePath = PathUri + path;

        if (Directory.Exists(absolutePath)) return;

        try
        {
            Directory.CreateDirectory(absolutePath);
        }
        catch(Exception)
        {
            throw new ConflictException($"Не удалось создать папку {absolutePath}");
        }
    }

    private static void Compress(string path)
    {
        var file = new FileInfo(path);
        var optimizer = new ImageOptimizer
        {
            OptimalCompression = true
        };
        optimizer.Compress(file);
    }

    public static (string extensionFile, string base64) SplitBase64(string base64)
    {
        var piecesFirst = base64.Split(";")[0].Split(@"/");
        var piecesSecond = base64.Split(",");

        if (piecesSecond.Length <= 1 || piecesFirst.Length <= 1) 
            throw new BadRequestException("Base64 не корректный, шаблон: " +
                "data:image/{png/jpeg/jpg};base64,{base64}");

        var extensionFile = "." + piecesFirst[1];

        if (!Regex.IsMatch("(.*?)\\.(png|jpg|jpeg)$", extensionFile))
            throw new BadRequestException("Доступные форматы файла png/jpg/jpeg");

        base64 = piecesSecond[1];

        return (extensionFile, base64);
    }
}