using BackupFilesProject.App;
using BackupFilesProject.App.Jobs;
using Microsoft.Extensions.Logging;

namespace BackupFilesProject
{
    internal class Program
    {
        public static ILoggerFactory? LogFactory { get; set; }
        private static ILogger? Log { get; set; }
        private static async Task Main(string[] args)
        {
            try
            {
                LogFactory = LoggerFactory.Create(builder =>
                {
                    builder.AddConsole();
                });
                Log = LogFactory.CreateLogger<Program>();

                if (args.Length == 0)
                {
                    throw new ArgumentException("No arguments!");
                }

                ConfigParams? configs = FileService.ParseJson<ConfigParams>(args[0]);
                await FilesCopyJobsService.Start(configs);
            }
            catch (FormatException e)
            {
                Log?.LogError("Invalid format of cron expression: " + e.Message);
            }
            catch (Exception e)
            {
                Log?.LogError(e.Message);
            }
        }
    }
}
