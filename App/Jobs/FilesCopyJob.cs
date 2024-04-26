using Microsoft.Extensions.Logging;
using Quartz;

namespace BackupFilesProject.App.Jobs
{
    internal class FilesCopyJob : IJob
    {
        private bool _isRunningJob = false;
        private ILogger<FilesCopyJob>? _log;

        public async Task Execute(IJobExecutionContext context)
        {
            _log ??= Program.LogFactory?.CreateLogger<FilesCopyJob>();

            try
            {
                if (_isRunningJob)
                {
                    return;
                }

                _isRunningJob = true;
                _log?.LogInformation("Start job: " + context.JobDetail.Key.Name);
                JobDataMap dataMap = context.JobDetail.JobDataMap;
                FilesCopyJobsService.FileService ??= new FileService();
                FilesCopyJobsService.FileService.StartCopyFiles(dataMap.GetString("sourcePath"), dataMap.GetString("destinationPath"));
                _log?.LogInformation("End job: " + context.JobDetail.Key.Name);
                _isRunningJob = false;
            }
            catch (Exception ex)
            {
                _log?.LogError($"Exception: {ex.Message} in {context.JobDetail.Key.Name}");
            }
        }
    }
}
