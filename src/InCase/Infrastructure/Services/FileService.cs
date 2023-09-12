using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public static class FileService
    {
        private const string PATH_URI = "\\static\\images\\";
        public static bool Upload(IFormFile file, string path)
        {
            if (file is null)
                return false;

            path = PATH_URI + path;

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
            if (!Directory.Exists(PATH_URI + path))
            {
                Directory.CreateDirectory(PATH_URI + path);
            }
        }
        public static bool RemoveFolder(string path)
        {
            try
            {
                Directory.Delete(PATH_URI + path, recursive: true);
                return true;
            }
            catch (DirectoryNotFoundException)
            {
                return false;
            }
        }
    }
}
