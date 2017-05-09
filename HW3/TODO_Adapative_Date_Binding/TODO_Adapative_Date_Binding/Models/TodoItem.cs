using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todos.Models
{
    class TodoItem
    {
        public string id { get; }

        public string title { get; set; }

        public string details { get; set; }

        public bool isFinished { get; set; }

        public DateTime dueDate { get; set; }

        public TodoItem(string title, string details, DateTime dueDate)
        {
            this.id = Guid.NewGuid().ToString();
            this.title = title;
            this.details = details;
            this.isFinished = false;//默认为未完成
            this.dueDate = dueDate;
        }
    }
}