using Microsoft.Win32;
using RecycleBinTray.Core;
using RecycleBinTray.Helpers;
using RecycleBinTray.Services;
using System.IO;
using System.Windows;
using Application = System.Windows.Application;

namespace RecycleBinTray;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public readonly string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "RecycleBin");

    private TrayIconManager? _trayIconManager;

    protected override void OnStartup(StartupEventArgs e)
    {

        // check if app already running
        Manager.AlreadyRunning();

        // Add App to Windows Startup
        Manager.AddToStartup();

        ShutdownMode = ShutdownMode.OnExplicitShutdown;

        base.OnStartup(e);
        // Initialize the tray icon manager with services
        var recycleBinService = new RecycleBinService();
        var setupRecycleBinWatcher = new SetupRecycleBinWatcher();
        var recycleBinIconSelector = new RecycleBinIconSelector(basePath);

        _trayIconManager = new TrayIconManager(recycleBinService, setupRecycleBinWatcher, recycleBinIconSelector);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        _trayIconManager?.Dispose();
        base.OnExit(e);
    }


}

