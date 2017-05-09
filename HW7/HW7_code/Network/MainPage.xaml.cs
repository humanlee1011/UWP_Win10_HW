using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;
using Windows.Data.Xml.Dom;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Network
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string ipadd = postcode.Text;
            string Url = "http://api.k780.com:88/?app=life.postcode&postcode=" + ipadd + "&appkey=10003&sign=b59bc3ef6191eb9f747dd4e83c99f2a4&format=xml";
            HttpClient httpclient = new HttpClient();
            HttpResponseMessage response = await httpclient.GetAsync(Url);
            response.EnsureSuccessStatusCode();
            string resultStr = await response.Content.ReadAsStringAsync();
            XmlDocument data = new XmlDocument();
            data.LoadXml(resultStr); //transfer the resultStr into xml format
            IXmlNode node = data.GetElementsByTagName("success")[0];
            if (node.InnerText != "1")
            {
                areacode.Text = "该邮编不正确或不存在";
                location.Text = "";
            }
            else
            {
                node = data.GetElementsByTagName("areanm")[0];
                location.Text = node.InnerText;
                node = data.GetElementsByTagName("areacode")[0];
                areacode.Text = node.InnerText;
            }
        }

        

        private void WeatherBarButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(WeatherSearch));
        }
    }
}
