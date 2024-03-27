using Authentication.BLL.Exceptions;

namespace Authentication.BLL.Services;
public static class FileService
{
    private const string PathUri = "/static/images/";

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
}