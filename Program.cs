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
        }
    }
}
