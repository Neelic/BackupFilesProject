namespace BackupFilesProject.App
{
    internal class ConfigParams(string sourcePath, string destinationPath, string cronExpression)
    {
        public string SourcePath { get; } = sourcePath;
        public string DestinationPath { get; } = destinationPath;
        public string CronExpression { get; } = cronExpression;

        public override string ToString() => $"{SourcePath} -> {DestinationPath} ({CronExpression})";
    }
}
