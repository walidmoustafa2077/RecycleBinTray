namespace RecycleBinTray.Models
{
    /// <summary>
    /// Represents the status of the Recycle Bin, including its size and item count.
    /// </summary>
    public class RecycleBinStatus
    {
        /// <summary>
        /// The total size of the Recycle Bin in bytes.
        /// </summary>
        public ulong SizeBytes { get; set; }

        /// <summary>
        /// The total number of items in the Recycle Bin.
        /// </summary>
        public ulong ItemCount { get; set; }
    }
}
