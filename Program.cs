using BackupFilesProject.App;
using BackupFilesProject.App.Jobs;
using Serilog;

namespace BackupFilesProject
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            try
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Console()
                    .CreateLogger();

                if (args.Length == 0)
                {
                    throw new ArgumentException("No arguments!");
                }

                ConfigParams? configs = FileService.ParseJson<ConfigParams>(args[0]);
                await FilesCopyJobsService.Start(configs);
            }
            catch (FormatException e)
            {
                Log.Logger.Error("Invalid format of cron expression: " + e.Message);
            }
            catch (Exception e)
            {
                Log.Logger.Error(e.Message);
            }
        }
    }
}
