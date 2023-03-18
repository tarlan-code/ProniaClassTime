namespace Pronia.Utilies.Extensions
{
    public static class FileExtension
    {
        public static bool CheckType(this IFormFile file, string type)
            => file.ContentType.Contains(type);
        public static bool CheckSize(this IFormFile file, int kb)
            => kb*1024 > file.Length;

        public static string SaveFile(this IFormFile file,string path)
        {
            string filename = ChangeName(file.FileName);

            using(FileStream fs = new FileStream(Path.Combine(path, filename), FileMode.Create))
            {
                file.CopyTo(fs);
            }
            return filename;
        }

        public static void SaveWithName(this IFormFile file, string path,string filename)
        {
            using (FileStream fs = new FileStream(Path.Combine(path, filename), FileMode.Create))
            {
                file.CopyTo(fs);
            }
        }

        static string ChangeName(string oldName)
        {
            string extension = oldName.Substring(oldName.LastIndexOf('.'));

            if (oldName.Length < 32) oldName = oldName.Substring(0, oldName.LastIndexOf('.'));
            
            else oldName = oldName.Substring(0, 31);

            return Guid.NewGuid().ToString() + oldName + extension;
        }

        public static void DeleteFile(this string file, string root, string folder)
        {
            string filePath = Path.Combine(root, folder, file);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"File {filePath} is successfully deleted.");
                Console.ForegroundColor = ConsoleColor.White;

            }

        }

        public static string CheckValidate(this IFormFile file,string type,int kb)
        {
            string result = "";
            if (!file.CheckType(type)) result += $"The size of the {file.FileName} can not be large from {kb} KB";
            
            if (!file.CheckSize(kb)) result += $"\t\t{file.FileName} isn't {type.Replace("/", "")} file";

            return result;
        }
    }
}
