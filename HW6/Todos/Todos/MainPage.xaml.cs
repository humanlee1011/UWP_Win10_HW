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
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Media.Imaging;

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
            var item = (sender as MenuFlyoutItem).DataContext as Models.TodoItem;
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
                //Uri imgsrc = saveImage(image.Source);
                this.ViewModel.AddTodoItem(title.Text, details.Text, date.Date.DateTime, new Uri("ms-appx:///Assets/background.jpg"));


                this.Frame.Navigate(typeof(MainPage), ViewModel);
            }
        }
        
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            title.Text = "";
            details.Text = "";
            date.Date = DateTime.Now;
        }

        private void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.SelectedItem = (sender as MenuFlyoutItem).DataContext as Models.TodoItem;
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += DataTransferManager_Requested;
            DataTransferManager.ShowShareUI();  
        }

        public async void DataTransferManager_Requested(DataTransferManager sender,  DataRequestedEventArgs args)
        {
            var dp = args.Request.Data;
            var deferral = args.Request.GetDeferral();
            var photoFile = await StorageFile.GetFileFromApplicationUriAsync(
                                        new Uri("ms-appx:///Assets/background.jpg"));
            dp.Properties.Title = this.ViewModel.SelectedItem.title;
            dp.Properties.Description = this.ViewModel.SelectedItem.details;
            dp.SetStorageItems(new List<StorageFile> { photoFile });
            dp.SetText(this.ViewModel.SelectedItem.details + "\nDeadline:" + this.ViewModel.SelectedItem.dueDate.ToString());
            deferral.Complete();
        }
        private async void SelectPictureButton_Click(object sender, RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");

            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();
            if (file != null)
            {
                // Application now has read/write access to the picked file
                BitmapImage bitmap = new BitmapImage();
                using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
                {
                    bitmap.SetSource(stream);
                }
                Image.Source = bitmap;
            }
            else
            {
                var temp = new MessageDialog("File does not exist.").ShowAsync();
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.SelectedItem = (sender as CheckBox).DataContext as Models.TodoItem;
            var item = this.ViewModel.SelectedItem;
            CheckBox checkbox = sender as CheckBox;
            if (checkbox.IsChecked == true)
            {
                this.ViewModel.UpdateTodoItem(item.id, item.title, item.details, item.dueDate, (bool)true, item.imageUri);
            }
            else
            {
                this.ViewModel.UpdateTodoItem(item.id, item.title, item.details, item.dueDate, (bool)false, item.imageUri);
            }
        }

        private void Search_Handler(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs e)
        {
            List<Models.TodoItem> searchlist = new List<Models.TodoItem>();
            if (e.ChosenSuggestion != null)
            {

            }
            else
            {
                this.ViewModel.SearchTodoItem(e.QueryText, searchlist);
                string output = string.Empty;
                foreach (var n in searchlist)
                {
                    output = output + "Title:" + n.title + "   Details:"
                             + n.details + "   DueDate:" + n.dueDate.ToString() + "\n";
                }
                var temp = new MessageDialog(output).ShowAsync();
            }
        }
    }
}
