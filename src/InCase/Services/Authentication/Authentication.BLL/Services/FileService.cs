using Authentication.BLL.Exceptions;
using System.Text.RegularExpressions;

namespace Authentication.BLL.Services
{
    public static class FileService
    {
        private static readonly string PATH_URI = Environment.CurrentDirectory
            .Split("src")[0] + @"static\images\";

        public static void UploadImageBase64(string base64, string filePath, string fileName)
        {
            (string extensionFile, base64) = SplitBase64(base64);

            fileName += extensionFile;

            string absolutePath = PATH_URI + filePath + fileName;

            CreateFolder(filePath);

            try
            {
                File.WriteAllBytes(absolutePath, Convert.FromBase64String(base64));
            }
            catch (Exception)
            {
                throw new ConflictException($"Не удалось создать файл {fileName}");
            }
        }

        public static void RemoveFile(string fileName, string filePath)
        {
            string absolutePath = PATH_URI + filePath + fileName;

            try
            {
                File.Delete(absolutePath);
            }
            catch (Exception)
            {
                throw new ConflictException($"Не удалось удалить файл {fileName}");
            }
        }

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

        public static (string extensionFile, string base64) SplitBase64(string base64)
        {
            string[] piecesFirst = base64.Split(";")[0].Split(@"/");
            string[] piecesSecond = base64.Split(",");

            if (piecesSecond.Length <= 1 || piecesFirst.Length <= 1) 
                throw new BadRequestException("Base64 не корректный, шаблон: " +
                    "data:image/{png/jpeg/jpg};base64,{base64}");

            string extensionFile = "." + piecesFirst[1];

            if (!Regex.IsMatch("(.*?)\\.(png|jpg|jpeg)$", extensionFile))
                throw new BadRequestException("Доступные форматы файла png/jpg/jpeg");

            base64 = piecesSecond[1];

            return (extensionFile, base64);
        }
    }
}
