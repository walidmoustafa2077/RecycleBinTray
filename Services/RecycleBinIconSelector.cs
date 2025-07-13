using RecycleBinTray.Helpers;
using RecycleBinTray.Models;
using System.IO;

namespace RecycleBinTray.Services
{
    public class RecycleBinIconSelector : IIconSelector
    {
        private readonly Icon _iconEmpty;
        private readonly Icon _iconLow;
        private readonly Icon _iconMedium;
        private readonly Icon _iconFull;
        private const ulong ThreeGB = 3UL * 1024 * 1024 * 1024;

        public RecycleBinIconSelector(string iconBasePath)
        {
            _iconEmpty = new Icon(Path.Combine(iconBasePath, "Light", "empty.ico"));
            _iconLow = new Icon(Path.Combine(iconBasePath, "Light", "low.ico"));
            _iconMedium = new Icon(Path.Combine(iconBasePath, "Light", "medium.ico"));
            _iconFull = new Icon(Path.Combine(iconBasePath, "Light", "full.ico"));
        }

        public Icon SelectIcon(RecycleBinStatus status)
        {
            if (status.SizeBytes > ThreeGB)
                return _iconFull;

            return status.ItemCount switch
            {
                0 => _iconEmpty,
                <= 10 => _iconLow,
                <= 50 => _iconMedium,
                _ => _iconFull,
            };
        }

        public string GetTooltip(RecycleBinStatus status)
        {
            if (status.SizeBytes > ThreeGB)
                return $"Recycle Bin ({status.ItemCount} items, {FormatSize.Format(status.SizeBytes)} - Full)";

            return status.ItemCount switch
            {
                0 => "Recycle Bin (Empty)",
                <= 10 => $"Recycle Bin ({status.ItemCount} item(s))",
                <= 50 => $"Recycle Bin ({status.ItemCount} items)",
                _ => $"Recycle Bin ({status.ItemCount} items - Full)",
            };
        }
    }
}
