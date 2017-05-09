using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.Graphics.Imaging;
// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Todos
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class NewPage : Page
    {
        public NewPage()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.CornflowerBlue;
            
        }

        ViewModels.TodoItemViewModel ViewModel { get; set; }

        private bool editState = false;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
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

            if (e.Parameter.GetType() == typeof(ViewModels.TodoItemViewModel))
            {
                this.ViewModel = (ViewModels.TodoItemViewModel)(e.Parameter);
                if (this.ViewModel.SelectedItem == null)
                {
                    editState = false;
                    createOrUpdateButton.Content = "Create";
                    textBlock.Text = "Create Todo Item";
                }
                else
                {
                    editState = true;
                    createOrUpdateButton.Content = "Update";
                    textBlock.Text = "Edit Todo Item";
                    //Bind the selected item
                    var selectedItem = this.ViewModel.SelectedItem;
                    title.Text = selectedItem.title;
                    description.Text = selectedItem.details;
                    date.Date = selectedItem.dueDate;
                }

            }
            var i = new MessageDialog("Welcome!").ShowAsync();
        }

        private void createOrUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            bool suc = true;
            if (title.Text == "")
            {
                suc = false;
                var temp = new MessageDialog("Title cannot be empty.").ShowAsync();
            }
            if (description.Text == "")
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
                //WriteableBitmap imgsrc = image.Source as WriteableBitmap;
                //string filename = string.Empty;
                //saveImage(imgsrc, filename);
                //Uri imguri = new Uri(ApplicationData.Current.LocalFolder.Path + filename);
                if (editState == true)
                {
                    this.ViewModel.UpdateTodoItem(this.ViewModel.SelectedItem.id, title.Text, description.Text,
                                                date.Date.DateTime, false, new Uri("ms-appx:///Assets/background.jpg"));
                }
                else
                {
                    this.ViewModel.AddTodoItem(title.Text, description.Text, date.Date.DateTime, new Uri("ms-appx:///Assets/background.jpg"));
                }
                this.Frame.Navigate(typeof(MainPage), ViewModel);
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (editState == false)
            {
                title.Text = "";
                description.Text = "";
                date.Date = DateTime.Now;
            }
            else
            {
                //当edit时，cancel会恢复到原来的值
                var selectedItem = this.ViewModel.SelectedItem;
                title.Text = selectedItem.title;
                description.Text = selectedItem.details;
                date.Date = selectedItem.dueDate;
            }
            
        }
        //private async void saveImage(WriteableBitmap image, string filename)
        //{

        //    DateTime now = DateTime.Now;
        //    filename = now.ToString();
        //    StorageFolder pictureFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Pictures", CreationCollisionOption.OpenIfExists);
        //    var pictureFile = await pictureFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);

        //    using (var stream = await pictureFile.OpenStreamForWriteAsync())
        //    {
        //        BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream.AsRandomAccessStream());
        //        var pixelStream = image.PixelBuffer.AsStream();
        //        byte[] pixels = new byte[pixelStream.Length];
        //        await pixelStream.ReadAsync(pixels, 0, pixels.Length);
        //        encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)image.PixelWidth,(uint)image.PixelHeight, 96, 96, pixels);
        //        await encoder.FlushAsync();
        //    }
        //}

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
                image.Source = bitmap;
            }
            else
            {
                var temp = new MessageDialog("File does not exist.").ShowAsync();
            }
        }
    }

    
}
