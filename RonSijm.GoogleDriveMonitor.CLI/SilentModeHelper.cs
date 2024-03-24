using System.Runtime.InteropServices;

namespace RonSijm.GoogleDriveMonitor.CLI;

public static class SilentModeHelper
{
    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    // ReSharper disable once InconsistentNaming
    const int SW_HIDE = 0;

    public static void HideHostIfSilentMode(bool silent)
    {
        if (!silent)
        {
            return;
        }

        // I don't think you can hide windows on linux by their process id
        if (Environment.OSVersion.Platform != PlatformID.Win32NT && Environment.OSVersion.Platform != PlatformID.Win32Windows)
        {
            return;
        }

        // Hide the console window
        var hWnd = GetConsoleWindow();

        if (hWnd != IntPtr.Zero)
        {
            ShowWindow(hWnd, SW_HIDE);
        }
    }
}