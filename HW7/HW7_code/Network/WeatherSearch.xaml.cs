using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Network
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class WeatherSearch : Page
    {
        public WeatherSearch()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // TODO

            if (rootFrame.CanGoBack)
            {
                // Show UI in title bar if opted-in and in-app backstack is not empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            else
            {
                // Remove the UI from the title bar if in-app back stack is empty.
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }
        }

        private async void search_Click(object sender, RoutedEventArgs e)
        {
            string cityValue = city.Text;
            string url = "https://api.heweather.com/v5/weather?city=" + cityValue + "&key=aba55fbc33dd41e5a909190a01aacadc";
            HttpClient httpclient = new HttpClient();
            string result = await httpclient.GetStringAsync(url);
            JObject jo = (JObject)JsonConvert.DeserializeObject(result);
            if (jo["HeWeather5"][0]["status"].ToString() == "unknown city")
            {
                maxtemperature.Text = "未知城市";
                mintemperature.Text = "";
                condition.Text = "";
            }
            else
            {
                mintemperature.Text = jo["HeWeather5"][0]["daily_forecast"][0]["tmp"]["min"].ToString();
                maxtemperature.Text = jo["HeWeather5"][0]["daily_forecast"][0]["tmp"]["max"].ToString();
                condition.Text = jo["HeWeather5"][0]["daily_forecast"][0]["cond"]["txt_n"].ToString();
            }

        }
    }

}
