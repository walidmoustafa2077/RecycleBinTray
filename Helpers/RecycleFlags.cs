namespace RecycleBinTray.Helpers
{
    /// <summary>
    /// Flags for the SHEmptyRecycleBin function to specify options for emptying the recycle bin.
    /// </summary>
    [Flags]
    public enum RecycleFlags : uint
    {
        /// <summary>
        /// Do not display a confirmation dialog box before emptying the recycle bin.
        /// </summary>
        SHERB_NOCONFIRMATION = 0x00000001,

        /// <summary>
        /// Do not display a progress dialog box while emptying the recycle bin.
        /// </summary>
        SHERB_NOPROGRESSUI = 0x00000002,

        /// <summary>
        /// Do not play a sound when the recycle bin is emptied.
        /// </summary>
        SHERB_NOSOUND = 0x00000004
    }
}
