
namespace Sim6Umut.Helpers
{
    public static class ExtensionMethods
    {
        public static bool CheckType(this IFormFile file,string type = "image")
        {
            return file.ContentType.Contains(type);
        }

        public static bool CheckSize(this IFormFile file,int mb)
        {
            return file.Length < mb * 1024 * 1024;
        }

        public static async Task<string> FileUploadAsync(this IFormFile file ,string folderPath)
        {
            var uniqueFileName = Guid.NewGuid().ToString() + file.FileName;
            var uniqueImagePath = Path.Combine(folderPath,uniqueFileName);
            FileStream fileStream = new(uniqueImagePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return uniqueFileName;
        }

        public static void DeleteFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

        }
    }
}
