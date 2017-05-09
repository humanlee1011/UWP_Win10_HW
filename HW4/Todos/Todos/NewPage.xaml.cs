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
using Windows.UI.Popups;


namespace Todos
{
    public sealed partial class NewPage : Page
    {
        public NewPage()
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
                Frame rootFrame = Window.Current.Content as Frame;

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
            else
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey("TheWorkInProgress"))
                {
                    ApplicationDataCompositeValue composite = (ApplicationDataCompositeValue)ApplicationData.Current.LocalSettings.Values["TheWorkInProgress"];
                    title.Text = (string)composite["title"];
                    details.Text = (string)composite["details"];
                    date.Date = DateTimeOffset.Parse((string)composite["date"]);

                    ApplicationData.Current.LocalSettings.Values.Remove("TheWorkInProgress");
                }
            }
            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
            composite["title"] = title.Text;
            composite["details"] = details.Text;
            composite["date"] = date.Date.ToString();

            ApplicationData.Current.LocalSettings.Values["TheWorkInProgress"] = composite;
        }

        private void Create_Item(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), "");
        }
    }
}
