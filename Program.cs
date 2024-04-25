using BackupFilesProject.App;
using BackupFilesProject.App.Jobs;
using Cronos;
using Quartz;
using Quartz.Logging;

namespace BackupFilesProject
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Exceprion: No arguments!");
                Environment.Exit(0);
            }

            try
            {
                LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());
                ConfigParams? configs = FileManager.ParseJson<ConfigParams>(args[0]);
                Console.WriteLine(configs?.ToString());
                await FilesCopyController.Start();

                Console.WriteLine("Press any key to close the application");
                Console.ReadKey();
            }
            catch (FileNotFoundException e)
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
