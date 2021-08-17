using HandyControl.Themes;

namespace CafeBazaarDownloader
{
    public partial class App
    {
        internal void UpdateSkin(ApplicationTheme theme)
        {
            ThemeManager.Current.ApplicationTheme = theme;
        }
    }
}
