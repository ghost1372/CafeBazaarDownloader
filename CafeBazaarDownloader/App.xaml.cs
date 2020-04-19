using HandyControl.Data;
using HandyControl.Tools;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using System.Windows;

namespace CafeBazaarDownloader
{
    public partial class App
    {
        public App()
        {
            AppCenter.Start("8c0300f8-3b97-41d7-8252-143aa125a85d",
                   typeof(Analytics), typeof(Crashes));
        }
        internal void UpdateSkin(SkinType skin)
        {
            Resources.MergedDictionaries.Add(ResourceHelper.GetSkin(skin));
            Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/HandyControl;component/Themes/Theme.xaml")
            });
            Current.MainWindow?.OnApplyTemplate();
        }
    }
}
