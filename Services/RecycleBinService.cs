using RecycleBinTray.Helpers;
using RecycleBinTray.Models;
using RecycleBinTray.Views;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RecycleBinTray.Services
{
    public partial class RecycleBinService : IRecycleBinService
    {
        public void Empty()
        {
            // Prompt the user to confirm before emptying the Recycle Bin.
            var confirmation = new DialogView(
                title: "Are you sure you want to empty the Recycle Bin?",
                supTitle: "Confirm Emptying Recycle Bin.",
                message: " This action cannot be undone.",
                submitText: "Yes",
                cancelText: "No"
            );

            // Red color for supTitle
            confirmation.ChangeTextColor(DialogText.Subtitle, "#8a0101"); 

            confirmation.ShowDialog();

            // Wait for user input
            while (confirmation.DialogResult == null)
                Task.Delay(100).Wait(); 

            if (confirmation.DialogResult == true)
            {
                // The RecycleFlags enum is used to specify options for the SHEmptyRecycleBin function.
                SHEmptyRecycleBin(IntPtr.Zero, null,
                    RecycleFlags.SHERB_NOCONFIRMATION |
                    RecycleFlags.SHERB_NOPROGRESSUI |
                    RecycleFlags.SHERB_NOSOUND);
            }
        }

        public void Open()
        {
            // Opens the Recycle Bin folder in File Explorer.
            Process.Start("explorer.exe", "shell:RecycleBinFolder");
        }

        public RecycleBinStatus GetStatus()
        {
            // The SHQUERYRBINFO structure is used to retrieve information about the recycle bin.
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

        /// <summary>
        /// Empties the Recycle Bin, permanently deleting all items within it.
        /// </summary>
        /// <param name="hwnd"> Handle to the parent window for any dialog boxes that might be displayed.</param>
        /// <param name="pszRootPath"> The path of the root directory for which to empty the recycle bin. If this parameter is NULL, the function empties the recycle bin for all drives.</param>
        /// <param name="dwFlags"> Flags that specify options for emptying the recycle bin. These can include options like not showing confirmation dialogs, not showing progress UI, and not playing sounds.</param>
        /// <returns> Returns zero if the function succeeds, or a nonzero value if it fails. The specific error code can be retrieved using Marshal.GetLastWin32Error().</returns>
        [LibraryImport("shell32.dll", EntryPoint = "SHEmptyRecycleBinW", StringMarshalling = StringMarshalling.Utf16)]
        private static partial int SHEmptyRecycleBin(nint hwnd, string? pszRootPath, RecycleFlags dwFlags);

        /// <summary>
        /// Retrieves information about the recycle bin, such as its size and item count.
        /// </summary>
        /// <param name="pszRootPath"> The path of the root directory for which to query the recycle bin. If this parameter is NULL, the function retrieves information for all drives.</param>
        /// <param name="pSHQueryRBInfo"> A reference to a SHQUERYRBINFO structure that receives the information about the recycle bin. This structure must be initialized with its cbSize member set to the size of the structure before calling this function.</param>
        /// <returns> Returns zero if the function succeeds, or a nonzero value if it fails. The specific error code can be retrieved using Marshal.GetLastWin32Error().</returns>
        [LibraryImport("shell32.dll", EntryPoint = "SHQueryRecycleBinW", StringMarshalling = StringMarshalling.Utf16)]
        private static partial int SHQueryRecycleBin(string? pszRootPath, ref SHQUERYRBINFO pSHQueryRBInfo);
    }
}
