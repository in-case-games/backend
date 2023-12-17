using Authentication.BLL.Exceptions;

namespace Authentication.BLL.Services
{
    public static class FileService
    {
        private static readonly string PATH_URI = @"/static/images/";

        public static void RemoveFolder(string path)
        {
            string absolutePath = PATH_URI + path;

            if (Directory.Exists(absolutePath))
            {
                try
                {
                    Directory.Delete(absolutePath, recursive: true);
                }
                catch (Exception)
                {
                    throw new ConflictException($"Не удалось удалить папку {absolutePath}");
                }
            }
        }

        public static void CreateFolder(string path)
        {
            string absolutePath = PATH_URI + path;

            if (!Directory.Exists(absolutePath))
            {
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
    }
}
