using System.Runtime.InteropServices;

namespace RecycleBinTray.Helpers
{
    /// <summary>
    /// Structure used to retrieve information about the recycle bin.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SHQUERYRBINFO
    {
        /// <summary>
        /// Size of this structure, in bytes.
        /// </summary>
        public uint cbSize;

        /// <summary>
        /// Total size of items in the recycle bin, in bytes.
        /// </summary>
        public ulong i64Size;

        /// <summary>
        /// Number of items in the recycle bin.
        /// </summary>
        public ulong i64NumItems;
    }
}
