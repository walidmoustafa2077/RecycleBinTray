using System.IO;

namespace RecycleBinTray.Helpers
{
    public class SetupRecycleBinWatcher
    {
        private readonly FileSystemWatcher? _watcher;

        // event handler to update the icon
        public event Action? UpdateIcon;

        public SetupRecycleBinWatcher()
        {
            var recycleBinPath = @"C:\$Recycle.Bin"; 

            _watcher = new FileSystemWatcher(recycleBinPath)
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Size,
                IncludeSubdirectories = true,
                EnableRaisingEvents = true
            };

            _watcher.Changed += OnRecycleBinChanged;
            _watcher.Created += OnRecycleBinChanged;
            _watcher.Deleted += OnRecycleBinChanged;
            _watcher.Renamed += OnRecycleBinChanged;
        }

        private void OnRecycleBinChanged(object sender, FileSystemEventArgs e)
        {
            UpdateIcon?.Invoke();
        }
    }
}
