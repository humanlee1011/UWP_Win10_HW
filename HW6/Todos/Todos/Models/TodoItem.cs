using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;


namespace Todos.Models
{
    public class TodoItem:INotifyPropertyChanged
    {
        public long id { get; }
        private string _title;
        private string _details;
        private bool _isFinished;
        private DateTime _dueDate;
        private Uri _imageUri = default(Uri);
        private BitmapImage _imageSource;
        public string title {
            get { return _title; }
            set {
                _title = value;
                RaisePropertyChanged("title");
            } }

        public string details {
            get { return _details; }
            set
            {
                _details = value;
                RaisePropertyChanged("details");
            }
        }

        public bool isFinished {
            get { return _isFinished; }
            set
            {
                _isFinished = value;
                RaisePropertyChanged("isFinished");
            }
        }

        public DateTime dueDate {
            get { return _dueDate; }
            set
            {
                _dueDate = value;
                RaisePropertyChanged("dueDate");
            }
        }

        public Uri imageUri
        {
            get { return _imageUri; }
            set
            {
                if (!object.Equals(_imageUri, value))
                {
                    SetImage(value);
                    _imageUri = value;
                    RaisePropertyChanged("imageUri");
                }
            }
        }

        public BitmapImage imageSource
        {
            get { return _imageSource; }
            set
            {
                if (!object.Equals(_imageSource, value)) {
                    _imageSource = value;
                    RaisePropertyChanged("imageSource");
                }
            }
        }

        private async void SetImage(Uri targetImageUri)
        {
            if (targetImageUri == null)
            {
                imageSource = null;
            }
            else
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(targetImageUri);
                var fileStraem = await file.OpenAsync(FileAccessMode.Read);
                var img = new BitmapImage();
                img.SetSource(fileStraem);
                imageSource = img;
            }
        }
        public TodoItem(long id, string title, string details, Boolean is_finished, DateTime dueDate, Uri imageuri)
        {
            this.id = id;
            this.title = title;
            this.details = details;
            this.isFinished = is_finished;//默认为未完成
            this.dueDate = dueDate;
            this.imageUri = imageuri;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}