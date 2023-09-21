namespace Infrastructure.Services
{
    public static class FileService
    {
        private const string PATH_URI = "\\static\\images\\";
        public static bool Upload(string base64, string path)
        {
            path = PATH_URI + path;

            string fileName = path.Split('\\')[^1];
            CreateFolderIfNotExist(path.Replace("\\"+fileName, ""));

            try
            {
                File.WriteAllBytes(path, Convert.FromBase64String(base64));
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
