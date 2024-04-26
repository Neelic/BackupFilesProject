using Quartz;

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
                JobDataMap dataMap = context.JobDetail.JobDataMap;
                FilesCopyJobsService.FileService ??= new FileService();
                FilesCopyJobsService.FileService.StartCopyFiles(dataMap.GetString("sourcePath"), dataMap.GetString("destinationPath"));
                _isRunningJob = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message + " in " + context.JobDetail.Key.Name);
            }
        }
    }
}
