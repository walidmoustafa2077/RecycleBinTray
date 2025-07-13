namespace RecycleBinTray.Helpers
{
    public class FormatSize
    {
        public static string Format(ulong bytes)
        {
            const double KB = 1024;
            const double MB = KB * 1024;
            const double GB = MB * 1024;

            if (bytes >= GB)
                return $"{bytes / GB:F2} GB";
            if (bytes >= MB)
                return $"{bytes / MB:F2} MB";
            if (bytes >= KB)
                return $"{bytes / KB:F2} KB";
            return $"{bytes} B";
        }
    }
}
