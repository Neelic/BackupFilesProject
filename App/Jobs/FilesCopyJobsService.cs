using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;

namespace BackupFilesProject.App.Jobs
{
    internal class FilesCopyJobsService
    {
        private static ILogger? _log { get; set; }
        internal static FileService? FileService { get; set; }
        public static async Task Start(ConfigParams config)
        {
            _log = Program.LogFactory?.CreateLogger<FilesCopyJobsService>();

            IHost builder = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddQuartz();
                    services.AddQuartzHostedService(opt =>
                    {
                        opt.WaitForJobsToComplete = true;
                    });
                })
                .ConfigureLogging((hostContext, logging) =>
                {
                    logging.AddConsole();
                })
                .Build();

            ISchedulerFactory schedulerFactory = builder.Services.GetRequiredService<ISchedulerFactory>();
            IScheduler scheduler = await schedulerFactory.GetScheduler();

            IJobDetail job = JobBuilder.Create<FilesCopyJob>()
                .WithIdentity("CopyFilesJob", "CopingProjectGroup")
                .UsingJobData("sourcePath", config.SourcePath)
                .UsingJobData("destinationPath", config.DestinationPath)
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("CopyFilesTrigger", "CopingProjectGroup")
                .WithCronSchedule(config.CronExpression, x => x.WithMisfireHandlingInstructionFireAndProceed())
                .StartNow()
                .Build();

            await scheduler.ScheduleJob(job, trigger);
            await builder.RunAsync();
        }
    }
}
