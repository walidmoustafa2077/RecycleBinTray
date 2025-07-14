using RecycleBinTray.Services;
using RecycleBinTray.Views;

namespace RecycleBinTray.Helpers
{
    public class ContextMenu
    {
        public static ContextMenuStrip BuildContextMenu(IRecycleBinService recycleBinService)
        {
            var menu = new ContextMenuStrip();
            menu.Items.Add("Settings", null, (_, _) =>
            {
                var settingsWindow = new DialogView(title: "Settings coming soon", message: "Not Implemented", submitText: "Okey");
                settingsWindow.ShowDialog();
            });

            menu.Items.Add("Open Recycle Bin", null, (_, _) => recycleBinService.Open());
            menu.Items.Add("Empty Recycle Bin", null, (_, _) => recycleBinService.Empty());

            menu.Items.Add("About", null, (_, _) =>
            {
                var aboutWindow = new AboutWindow();
                aboutWindow.ShowDialog();
            });

            menu.Items.Add("Close Appliaction", null, (_, _) => Environment.Exit(0));

            return menu;
        }

        public static void ShowContextMenuWithoutTaskbar(NotifyIcon trayIcon)
        {
            if (trayIcon.ContextMenuStrip == null)
                return;

            var owner = new Form()
            {
                ShowInTaskbar = false,
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual,
                Location = Cursor.Position,
                Size = new Size(1, 1),
                Opacity = 0,
                TopMost = true
            };

            owner.Load += (s, e) =>
            {
                // Hide right after load, so it doesn't flash
                owner.Hide();

                // Show the context menu at the current cursor position
                trayIcon.ContextMenuStrip.Closing += (s2, e2) =>
                {
                    trayIcon.ContextMenuStrip.Closing -= null!;
                    owner.Close();
                    owner.Dispose();
                };

                trayIcon.ContextMenuStrip.Show(owner, owner.PointToClient(Cursor.Position));
            };

            owner.Show();
            owner.Activate();
        }


    }
}
