using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todos.ViewModels
{
    class TodoItemViewModel
    {
        private ObservableCollection<Models.TodoItem> todoList = new ObservableCollection<Models.TodoItem>();
        public ObservableCollection<Models.TodoItem> TodoList { get { return this.todoList; } }
        private Models.TodoItem selectedItem = default(Models.TodoItem);
        public Models.TodoItem SelectedItem {
            get { return this.selectedItem; }
            set { this.selectedItem = value; }
        }

        public TodoItemViewModel()
        {
            //测试样例
            this.todoList.Add(new Models.TodoItem("现操作业", "周末必须完成", Convert.ToDateTime("03/17/2017")));
            this.todoList.Add(new Models.TodoItem("xx考试", "复习1、5、6、7、10章", Convert.ToDateTime("04/16/2017")));
            this.todoList.Add(new Models.TodoItem("消费者日", "有xxxxx活动", Convert.ToDateTime("03/15/2017")));
            this.todoList.Add(new Models.TodoItem("院系赛", "锦标赛", Convert.ToDateTime("03/20/2017")));
        }

        public void AddTodoItem(string title, string details, DateTime dueDate)
        {
            this.todoList.Add(new Models.TodoItem(title, details, dueDate));
        }

        public void RemoveTodoItem(string id)
        {
            var toDelete = todoList.Single<Models.TodoItem>(i => i.id == id);
            todoList.Remove(toDelete);
            this.selectedItem = null;
        }

        public void UpdateTodoItem(string id, string title, string details, DateTime dueDate, bool isFinished)
        {
            var toUpdate = todoList.Single<Models.TodoItem>(i => i.id == id);
            toUpdate.title = title;
            toUpdate.details = details;
            toUpdate.dueDate = dueDate;
            toUpdate.isFinished = isFinished;

            this.selectedItem = null;
        } 
    }
}