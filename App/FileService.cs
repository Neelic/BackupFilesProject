using System.Text.Json;

namespace BackupFilesProject.App
{
    internal class FileService
    {
        private static readonly string FILE_TAG = "F:";
        private static readonly string DIR_TAG = "D:";
        private readonly Dictionary<string, DateTime> _source = [];
        private string _sourcePath = "";
        private readonly List<FileInfo> _filesToCopy = [];
        private readonly List<DirectoryInfo> _dirsToCopy = [];

        public List<string> SourceList { get => [.. _source.Keys]; }

        public static T? ParseJson<T>(string path)
        {
            using var reader = new StreamReader(path);
            string json = reader.ReadToEnd();
            return JsonSerializer.Deserialize<T>(json);
        }

        public void StartCopyFiles(string sourceDirName, string destDirName)
        {
            try
            {
                FindForCopy(sourceDirName, destDirName);
                CopyFiles(destDirName);
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void FindForCopy(string sourceDirName, string destDirName, bool copySubDirs = true, bool firstCopy = true)
        {
            var dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            if (firstCopy)
            {
                _sourcePath = sourceDirName;
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            foreach (FileInfo file in dir.GetFiles())
            {
                string tempFilePath = FILE_TAG + file.FullName.Remove(0, _sourcePath.Length);
                if (!_source.ContainsKey(tempFilePath))
                {
                    _source.Add(tempFilePath, file.LastWriteTime);
                    _filesToCopy.Add(file);
                }
                else if (_source[tempFilePath].CompareTo(file.LastWriteTime) < 0)
                {
                    _source[tempFilePath] = file.LastWriteTime;
                    _filesToCopy.Add(file);
                }

            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDirName, subdir.Name);
                    string tempDirPath = DIR_TAG + subdir.FullName.Remove(0, _sourcePath.Length);
                    if (!_source.ContainsKey(tempDirPath))
                    {
                        _source.Add(tempDirPath, subdir.LastWriteTime);
                        _dirsToCopy.Add(subdir);
                    }
                    else if (_source[tempDirPath].CompareTo(subdir.LastWriteTime) < 0)
                    {
                        _source[tempDirPath] = subdir.LastWriteTime;
                        _dirsToCopy.Add(subdir);
                    }

                    FindForCopy(subdir.FullName, tempPath, copySubDirs, firstCopy: false);
                }
            }
        }

        private void CopyFiles(string destDirName)
        {
            if (_dirsToCopy.Count == 0 && _filesToCopy.Count == 0)
            {
                return;
            }

            string baseDirPath = Path.Combine(destDirName, "base");
            if (!Directory.Exists(baseDirPath))
            {
                destDirName = baseDirPath;
            }
            else
            {
                destDirName = Path.Combine(destDirName, "inc_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss"));
            }

            Directory.CreateDirectory(destDirName);

            foreach (DirectoryInfo directoryInfo in _dirsToCopy)
            {
                Directory.CreateDirectory(Path.Combine(destDirName,
                    directoryInfo.FullName.Remove(0, _sourcePath.Length + 1)));
            }

            foreach (FileInfo file in _filesToCopy)
            {
                string tempDirPath = destDirName + file.Directory.FullName.Remove(0, _sourcePath.Length);
                if (!Directory.Exists(tempDirPath))
                {
                    Directory.CreateDirectory(tempDirPath);
                }

                file.CopyTo(Path.Combine(destDirName,
                    file.FullName.Remove(0, _sourcePath.Length + 1)), false);
            }

            _filesToCopy.Clear();
            _dirsToCopy.Clear();
        }
    }
}