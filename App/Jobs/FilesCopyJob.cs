using Quartz;
using Quartz.Impl;

namespace BackupFilesProject.App.Jobs
{
    internal class FilesCopyController
    {
        public static FileManager? _fileManager;
        public static async Task Start()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.Start();
            IJobDetail job = JobBuilder.Create<FilesCopyJob>()
                .WithIdentity("job1", "group1")
                .Build();
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .WithCronSchedule("*/10 * * * * ?", x => x.WithMisfireHandlingInstructionFireAndProceed())
                //.WithSimpleSchedule(x => x
                //   .WithIntervalInSeconds(10)
                //   .RepeatForever())
                .Build();
            await scheduler.ScheduleJob(job, trigger);
        }
    }

    internal class FilesCopyJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            FilesCopyController._fileManager ??= new FileManager();
            await Console.Out.WriteLineAsync("Greetings from HelloJob!");
            await Console.Out.WriteLineAsync(Path.Combine(@"C:", @"Program Files", "inc_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));
            FilesCopyController._fileManager.FindForCopy(@"C:\sourceTmp", @"C:\sourceCopy");
            FilesCopyController._fileManager.CopyFiles(@"C:\sourceTmp", @"C:\sourceCopy");
        }
    }
}
