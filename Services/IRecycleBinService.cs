using RecycleBinTray.Models;

namespace RecycleBinTray.Services
{
    public interface IRecycleBinService
    {
        /// <summary>
        /// Empties the Recycle Bin, permanently deleting all items within it.
        /// </summary>
        void Empty();

        /// <summary>
        /// Opens the Recycle Bin in the file explorer.
        /// </summary>
        void Open();

        /// <summary>
        /// Retrieves the current status of the Recycle Bin, including its size and item count.
        /// </summary>
        /// <returns> A <see cref="RecycleBinStatus"/> object containing the size and item count of the Recycle Bin.</returns>
        RecycleBinStatus GetStatus();
    }
}
