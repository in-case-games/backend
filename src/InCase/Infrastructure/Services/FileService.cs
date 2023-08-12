using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public static class FileService
    {
        public static bool Upload(IFormFile file, string path)
        {
            if (file is null)
                return false;

            string fileName = path.Split('\\')[path.Split('\\').Length - 1];
            CreateFolderIfNotExist(path.Replace("\\"+fileName, ""));

            using FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
            try
            {
                Task.Run(async () => await file.CopyToAsync(fileStream)).Wait();
            }
            catch (DirectoryNotFoundException)
            {
                return false;
            }

            return true;
        }
        public static void CreateFolderIfNotExist(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        public static bool RemoveFolder(string path)
        {
            try
            {
                Directory.Delete(path, recursive: true);
                return true;
            }
            catch (DirectoryNotFoundException)
            {
                return false;
            }
        }
    }
}
