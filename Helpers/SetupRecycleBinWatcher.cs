using System.IO;
using System.Security.Principal;
using Timer = System.Threading.Timer;

namespace RecycleBinTray.Helpers
{
    public class SetupRecycleBinWatcher : IDisposable
    {
        private readonly List<FileSystemWatcher> _watchers = [];
        private readonly object _lock = new();
        private Timer? _debounceTimer;
        private bool _disposed;

        public event Action? UpdateIcon;

        public SetupRecycleBinWatcher()
        {
            string? userSid = WindowsIdentity.GetCurrent().User?.Value;
            if (userSid is null)
                throw new InvalidOperationException("Unable to get current user SID");

            string userBinSubPath = Path.Combine("$Recycle.Bin", userSid);

            var drives = DriveInfo.GetDrives().Where(d =>
                d.DriveType is DriveType.Fixed or DriveType.Network &&
                d.IsReady);

            foreach (var drive in drives)
            {
                string fullBinPath = Path.Combine(drive.RootDirectory.FullName, userBinSubPath);

                if (!Directory.Exists(fullBinPath))
                    continue;

                var watcher = new FileSystemWatcher(fullBinPath)
                {
                    NotifyFilter = NotifyFilters.FileName,
                    // Watch only new recycle bin metadata files
                    Filter = "$I*", 
                    IncludeSubdirectories = false,
                    EnableRaisingEvents = true,
                    // safer for bulk deletes
                    InternalBufferSize = 64 * 1024
                };

                watcher.Created += OnChanged;
                watcher.Deleted += OnChanged;

                _watchers.Add(watcher);
            }
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            lock (_lock)
            {
                _debounceTimer?.Dispose();
                _debounceTimer = new Timer(_ =>
                {
                    lock (_lock)
                    {
                        UpdateIcon?.Invoke();
                        _debounceTimer?.Dispose();
                        _debounceTimer = null;
                    }
                }, null, 500, Timeout.Infinite);
            }
        }

        public void Dispose()
        {
            if (_disposed) return;

            lock (_lock)
            {
                _debounceTimer?.Dispose();
                _debounceTimer = null;
            }

            foreach (var watcher in _watchers)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }

            _disposed = true;
        }
    }
}
