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
            //Adaptive UI
            this.SizeChanged += (s, e) =>
            {
                var state = "VisualStateMin0";
                if (e.NewSize.Width > 501)
                    state = "VisualStateMin500";
                if (e.NewSize.Width > 800)
                    state = "VisualStateMin800";
                VisualStateManager.GoToState(this, state, true);
            };

            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.CornflowerBlue;
            this.ViewModel = new ViewModels.TodoItemViewModel();
        }

        ViewModels.TodoItemViewModel ViewModel { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter.GetType() == typeof(ViewModels.TodoItemViewModel))
            {
                this.ViewModel = (ViewModels.TodoItemViewModel)(e.Parameter);
            }
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            this.ViewModel.SelectedItem = null;
        }

        private void TodoItem_ItemClicked(object sender, ItemClickEventArgs e)
        {
            ViewModel.SelectedItem = (Models.TodoItem)(e.ClickedItem);
            this.Frame.Navigate(typeof(NewPage), ViewModel);
        }

        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            //当窗口>800时无响应
            if (Window.Current.Bounds.Width <= 800)
                this.Frame.Navigate(typeof(NewPage), ViewModel);
        }
        

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as MenuFlyoutItem).Tag as Models.TodoItem;
            ViewModel.RemoveTodoItem(item.id);
        }

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            bool suc = true;
            if (title.Text == "")
            {
                suc = false;
                var temp = new MessageDialog("Title cannot be empty.").ShowAsync();
            }
            if (details.Text == "")
            {
                suc = false;
                var temp = new MessageDialog("Description cannot be empty.").ShowAsync();
            }
            if (date.Date < DateTime.Now)
            {
                suc = false;
                var temp = new MessageDialog("DueDate is not correct.").ShowAsync();
            }
            if (suc)
            {
                this.ViewModel.AddTodoItem(title.Text, details.Text, date.Date.DateTime);
                
                this.Frame.Navigate(typeof(MainPage), ViewModel);
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            title.Text = "";
            details.Text = "";
            date.Date = DateTime.Now;
        }
    }
}
