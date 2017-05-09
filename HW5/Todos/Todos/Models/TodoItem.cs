using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todos.Models
{
    public class TodoItem:INotifyPropertyChanged
    {
        public string id { get; }
        private string _title;
        private string _details;
        private bool _isFinished;
        private DateTime _dueDate;

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

        public TodoItem(string title, string details, DateTime dueDate)
        {
            this.id = Guid.NewGuid().ToString();
            this.title = title;
            this.details = details;
            this.isFinished = false;//默认为未完成
            this.dueDate = dueDate;
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