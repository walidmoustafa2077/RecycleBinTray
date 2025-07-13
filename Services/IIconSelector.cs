using RecycleBinTray.Models;

namespace RecycleBinTray.Services
{
    public interface IIconSelector
    {
        Icon SelectIcon(RecycleBinStatus status);
        string GetTooltip(RecycleBinStatus status);
    }
}
