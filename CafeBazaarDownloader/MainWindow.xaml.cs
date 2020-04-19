using HandyControl.Controls;
using HandyControl.Data;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Application = System.Windows.Application;
using Button = System.Windows.Controls.Button;

namespace CafeBazaarDownloader
{
    public partial class MainWindow
    {
        string apkName = string.Empty;
        public MainWindow()
        {
            InitializeComponent();
            txtLocation.Text = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";
        }

        private void btnBrowse_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    txtLocation.Text = dialog.SelectedPath + @"\";
                }
            }
        }

        private void txtUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnDownload.IsEnabled = !string.IsNullOrEmpty(txtUrl.Text);
        }

        private async void btnDownload_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                string apkID = txtUrl.Text;
                if (apkID.Contains("cafebazaar.ir"))
                {
                    apkID = apkID.Substring(26, apkID.Length - 26).Replace("/", "");
                }
                apkName = apkID;
                string json = @"
{""properties"":{""androidClientInfo"":{""adId"":""92d27488-6373-341d-a5f3-131d2a6ed6c7"",""adOptOut"":false,""androidId"":""090a641477b55052"",""city"":""NA"",""country"":""NA"",""cpu"":""x86,armeabi-v7a,armeabi"",""device"":"""",""dpi"":320,""hardware"":"""",""height"":1600,""locale"":"""",""manufacturer"":""samsung"",""mcc"":432,""mnc"":11,""model"":""SM-G920F"",""osBuild"":"""",""product"":""heroltektt"",""province"":""NA"",""sdkVersion"":22,""width"":900},""appThemeState"":0,""clientID"":""bCvRvAaGQs854eYG42sI8A"",""clientVersion"":""8.10.2"",""clientVersionCode"":801002,""isKidsEnabled"":false,""language"":2},""singleRequest"":{""appDownloadInfoRequest"":{""downloadStatus"":1,""packageName"":""XXXXXX"",""referrers"":[{""type"":11,""extraJson"":""{\""services\"":\""vitrin\"",\""slug\"":\""home\""}""},{""type"":1,""extraJson"":""{\""services\"":\""vitrin\"",\""index\"":3,\""title\"":\""1„71„9 1„51ƒ91†8 1Œ61”41…21„91†51„2\"",\""source\"":\""normal\"",\""is_shuffled\"":false,\""referrer_identifier\"":\""query_Trending Apps \\u0026 Games\""}""},{""type"":2,""extraJson"":""{\""services\"":\""vitrin\"",\""index\"":0,\""referrer_identifier\"":\""\""}""},{""type"":17,""extraJson"":""{\""package_name\"":\""XXXXXX\"",\""service\"":\""sejel\""}""}]}}}
";

                if (!string.IsNullOrEmpty(txtLocation.Text))
                {
                    json = json.Replace("XXXXXX", apkID);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");

                    using var client = new HttpClient();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer fh2OF2j45/dUyWibBlnY3ri3YtwtQq16Ie1yKLgY3M82xuBYDIpQzxmBD/Dc22NHHraM3d2JFD9RWLgEeD96nNUVqiPeYULPFdZxBu72EBPy4x6a5w==");
                    var result = await client.PostAsync("https://api.cafebazaar.ir/rest-v1/process/AppDownloadInfoRequest", data);

                    result.EnsureSuccessStatusCode();
                    var resp = await result.Content.ReadAsStringAsync();

                    var parse = System.Text.Json.JsonSerializer.Deserialize<CafeModel.RootObject>(resp);
                    string token = parse.singleReply.appDownloadInfoReply.token;
                    string downloadLink = parse.singleReply.appDownloadInfoReply.cdnPrefix[0] + $"apks/{token}.apk";

                    OnDownloadClick(downloadLink);

                }
            }
            catch (Exception ex)
            {

                Growl.ErrorGlobal(ex.Message);
            }
        }

        private WebClient client;
        private void OnDownloadClick(string link)
        {
            try
            {
                client = new WebClient();

                string loc = txtLocation.Text + apkName + ".apk";

                prg.Value = 0;
                btnDownload.IsEnabled = false;
                lblStatus.Visibility = System.Windows.Visibility.Visible;
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                client.DownloadFileAsync(new Uri(link), loc);
            }
            catch (Exception ex)
            {

                Growl.ErrorGlobal(ex.Message);
            }
        }
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            prg.Value = 0;
            lblStatus.Content = "Download Finished";
            btnDownload.IsEnabled = true;
        }
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;
            prg.Value = int.Parse(Math.Truncate(percentage).ToString());
            lblStatus.Content = $"{prg.Value}% Downloaded";
        }

        #region Change Skin and Language
        private void ButtonConfig_OnClick(object sender, RoutedEventArgs e)
        {
            PopupConfig.IsOpen = true;
        }

        private void ButtonSkins_OnClick(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource is Button button && button.Tag is SkinType tag)
            {
                PopupConfig.IsOpen = false;
                ((App)Application.Current).UpdateSkin(tag);
            }
        }
        #endregion
    }
}
