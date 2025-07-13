using RecycleBinTray.Helpers;
using RecycleBinTray.Services;

namespace RecycleBinTray.Core
{
    public class TrayIconManager : IDisposable
    {
        private readonly NotifyIcon _trayIcon;
        private readonly IRecycleBinService _recycleBinService;
        private readonly SetupRecycleBinWatcher _setupRecycleBinWatcher;
        private readonly IIconSelector _iconSelector;

        public TrayIconManager(IRecycleBinService recycleBinService, SetupRecycleBinWatcher setupRecycleBinWatcher, IIconSelector iconSelector)
        {
            _recycleBinService = recycleBinService;
            _setupRecycleBinWatcher = setupRecycleBinWatcher;
            _iconSelector = iconSelector;

            _trayIcon = new NotifyIcon
            {
                Icon = _iconSelector.SelectIcon(_recycleBinService.GetStatus()),
                Visible = true,
                Text = "Recycle Bin",
                ContextMenuStrip = ContextMenu.BuildContextMenu(_recycleBinService),
            };

            _trayIcon.MouseDoubleClick += TrayIcon_MouseDoubleClick;
            _trayIcon.MouseClick += TrayIcon_MouseClick;

            UpdateIcon();

            _setupRecycleBinWatcher.UpdateIcon += UpdateIcon;
            _iconSelector = iconSelector;
        }

        private void TrayIcon_MouseDoubleClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _recycleBinService.Open();
        }

        private void TrayIcon_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                ContextMenu.ShowContextMenuWithoutTaskbar(_trayIcon);
        }

        private void UpdateIcon()
        {
            var status = _recycleBinService.GetStatus();
            var icon = _iconSelector.SelectIcon(status);
            var tooltip = _iconSelector.GetTooltip(status);

            if (_trayIcon.Icon != icon)
                _trayIcon.Icon = icon;

            _trayIcon.Text = tooltip;
        }

        public void Dispose()
        {
            _trayIcon.Visible = false;
            _trayIcon.Dispose();
        }
    }
}
