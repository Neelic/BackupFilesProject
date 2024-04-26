using Quartz;
using Serilog;

namespace BackupFilesProject.App.Jobs
{
    internal class FilesCopyJob : IJob
    {
        private bool _isRunningJob = false;

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                if (_isRunningJob)
                {
                    return;
                }

                _isRunningJob = true;
                Log.Logger.Information("Start job: " + context.JobDetail.Key.Name);
                JobDataMap dataMap = context.JobDetail.JobDataMap;
                FilesCopyJobsService.FileService ??= new FileService();
                FilesCopyJobsService.FileService.StartCopyFiles(dataMap.GetString("sourcePath"), dataMap.GetString("destinationPath"));
                Log.Logger.Information("End job: " + context.JobDetail.Key.Name);
                _isRunningJob = false;
            }
            catch (Exception ex)
            {
                Log.Logger.Error("Exception: " + ex.Message + " in " + context.JobDetail.Key.Name);
            }
        }
    }
}
