using System.Text.Json;

namespace BackupFilesProject.App
{
    internal class FileReader
    {
        public static T? ReadJson<T>(string path)
        {
            using var reader = new StreamReader(path);
            string json = reader.ReadToEnd();
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}