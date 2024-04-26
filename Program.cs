using BackupFilesProject.App;
using BackupFilesProject.App.Jobs;
using Quartz.Logging;

namespace BackupFilesProject
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    throw new ArgumentException("No arguments!");
                }

                LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());
                ConfigParams? configs = FileService.ParseJson<ConfigParams>(args[0]);
                await FilesCopyJobsService.Start(configs);
            }
            catch (FormatException e)
            {
                Console.WriteLine("Exception: Invalid cron expression: " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

        }

        private class ConsoleLogProvider : ILogProvider
        {
            public Logger GetLogger(string name)
            {
                return (level, func, exception, parameters) =>
                {
                    if (level >= LogLevel.Info && func != null)
                    {
                        Console.WriteLine("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);
                    }
                    return true;
                };
            }

            public IDisposable OpenNestedContext(string message)
            {
                throw new NotImplementedException();
            }

            public IDisposable OpenMappedContext(string key, object value, bool destructure = false)
            {
                throw new NotImplementedException();
            }
        }
    }
}
