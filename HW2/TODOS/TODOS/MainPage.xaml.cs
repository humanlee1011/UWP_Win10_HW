using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.VisualBasic;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace TODOS
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {

        List<ToDoItem> todolist = new List<ToDoItem>();

        public MainPage()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.CornflowerBlue;
            //create the todo item list
            //todolist.Add(new ToDoItem() { Title = "完成作业", Description = "周末必须完成", DueDate = Convert.ToDateTime("03/17/2017"), isChecked = false});
            //todolist.Add(new ToDoItem() { Title = "现操作业", Description = "周六完成", DueDate = Convert.ToDateTime("03/10/2017"), isChecked = false });
            //todolist.Add(new ToDoItem() { Title = "xx考试", Description = "看书复习、看ppt", DueDate = Convert.ToDateTime("04/17/2017"), isChecked = false });
            //todolist.Add(new ToDoItem() { Title = "女生节、妇女节", Description = "准备xx", DueDate = Convert.ToDateTime("03/07/2017"), isChecked = true });
            //todolist.Add(new ToDoItem() { Title = "打扫宿舍", Description = "xxx", DueDate = Convert.ToDateTime("03/16/2017"), isChecked = false });

            //Todolist.ItemsSource = todolist;
        }

        public class ToDoItem
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public DateTime DueDate { get; set; }
            public bool isChecked { get; set; }

            public ToDoItem()
            {
                Title = "";
                Description = "";
                DueDate = DateTime.Now;
                isChecked = false;
            }
            
        }

        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            //跳转到Editpage
            this.Frame.Navigate(typeof(EditPage));
        }
        
        private void Checkbox_Click1(object sender, RoutedEventArgs e)
        {
            CheckBox checkbox = sender as CheckBox;
            if (checkbox.IsChecked == true)
            {
                line1.Visibility = Visibility.Visible;
            }
            else
            {
                line1.Visibility = Visibility.Collapsed;
            }
        }

        private void Checkbox_Click2(object sender, RoutedEventArgs e)
        {
            CheckBox checkbox = sender as CheckBox;
            if (checkbox.IsChecked == true)
            {
                line2.Visibility = Visibility.Visible;
            }
            else
            {
                line2.Visibility = Visibility.Collapsed;
            }
        }
    }
}
