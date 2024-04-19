using BackupFilesProject.App;

namespace BackupFilesProject
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Exceprion: No arguments!");
                Environment.Exit(0);
            }

            try
            {
                ConfigParams? configs = FileReader.ReadJson<ConfigParams>(args[0]);
                Console.WriteLine(configs?.ToString());
            } catch (FileNotFoundException e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
    }
}
