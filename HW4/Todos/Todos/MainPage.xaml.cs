using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Todos
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.CornflowerBlue;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                ApplicationData.Current.LocalSettings.Values.Remove("TheWorkInProgress");
            }
            else
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("TheWorkInProgress"))
                {
                    ApplicationDataCompositeValue composite = (ApplicationDataCompositeValue)ApplicationData.Current.LocalSettings.Values["TheWorkInProgress"];

                    checkbox1.IsChecked = (bool)composite["checkbox1"];
                    checkbox2.IsChecked = (bool)composite["checkbox2"];
                    line1.Opacity = (double)composite["line1"];
                    line2.Opacity = (double)composite["line2"];
                    ApplicationData.Current.LocalSettings.Values.Remove("TheWorkInProgress");
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
            composite["checkbox1"] = checkbox1.IsChecked;
            composite["checkbox2"] = checkbox2.IsChecked;
            composite["line1"] = line1.Opacity;
            composite["line2"] = line2.Opacity;

            ApplicationData.Current.LocalSettings.Values["TheWorkInProgress"] = composite;
        }

        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewPage), "");
        }
        

        private void checkbox2_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkbox = sender as CheckBox;
            if (checkbox.IsChecked == true)
            {
                line2.Opacity = 1.0;
            }
            else
            {
                line2.Opacity = 0.0;
            }
        }
        

        private void checkbox1_Click(object sender, RoutedEventArgs e)
        {
            CheckBox checkbox = sender as CheckBox;
            if (checkbox.IsChecked == true)
            {
                line1.Opacity = 1.0;
            }
            else
            {
                line1.Opacity = 0.0;
            }
        }
    }
}
