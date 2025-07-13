using RecycleBinTray.Helpers;
using RecycleBinTray.Models;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RecycleBinTray.Services
{
    public class RecycleBinService : IRecycleBinService
    {
        public void Empty()
        {
            // The RecycleFlags enum is used to specify options for the SHEmptyRecycleBin function.
            SHEmptyRecycleBin(IntPtr.Zero, null,
                RecycleFlags.SHERB_NOCONFIRMATION |
                RecycleFlags.SHERB_NOPROGRESSUI |
                RecycleFlags.SHERB_NOSOUND);
        }

        public void Open()
        {
            // Opens the Recycle Bin folder in File Explorer.
            Process.Start("explorer.exe", "shell:RecycleBinFolder");
        }

        public RecycleBinStatus GetStatus()
        {
            // The SHQUERYRBINFO structure is used to retrieve information about the recycle bin.
            // https://learn.microsoft.com/en-us/windows/win32/api/shellapi/ns-shellapi-shqueryrbinfo
            // https://www.pinvoke.net/default.aspx/shell32/SHQueryRecycleBin.html
            SHQUERYRBINFO info = new SHQUERYRBINFO();

            // Set the size of the SHQUERYRBINFO structure.
            info.cbSize = (uint)Marshal.SizeOf(typeof(SHQUERYRBINFO));

            // The SHQueryRecycleBin function retrieves information about the recycle bin.
            int result = SHQueryRecycleBin(null, ref info);

            if (result != 0)
                throw new System.ComponentModel.Win32Exception(result);

            return new RecycleBinStatus
            {
                SizeBytes = info.i64Size,
                ItemCount = info.i64NumItems
            };
        }

        // https://learn.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shemptyrecyclebina

        /// <summary>
        /// The SHEmptyRecycleBin function empties the recycle bin for a specified root path.
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="pszRootPath"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        private static extern int SHEmptyRecycleBin(IntPtr hwnd, string? pszRootPath, RecycleFlags dwFlags);

        // https://learn.microsoft.com/en-us/windows/win32/api/shellapi/nf-shellapi-shqueryrecyclebinw

        /// <summary>
        /// The SHQueryRecycleBin function retrieves information about the recycle bin for a specified root path.
        /// </summary>
        /// <param name="pszRootPath"></param>
        /// <param name="pSHQueryRBInfo"></param>
        /// <returns></returns>
        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        static extern int SHQueryRecycleBin(string? pszRootPath, ref SHQUERYRBINFO pSHQueryRBInfo);
    }
}
