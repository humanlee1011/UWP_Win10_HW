using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.Data.Xml.Dom;
using System.Net;
using SQLitePCL;

namespace Todos.ViewModels
{
    class TodoItemViewModel
    {
        private ObservableCollection<Models.TodoItem> todoList = new ObservableCollection<Models.TodoItem>();
        public ObservableCollection<Models.TodoItem> TodoList { get { return this.todoList; } }
        private Models.TodoItem selectedItem = default(Models.TodoItem);
        private List<Models.TodoItem> newlist = new List<Models.TodoItem>();
   
        public Models.TodoItem SelectedItem
        {
            get { return this.selectedItem; }
            set { this.selectedItem = value; }
        }

        public TodoItemViewModel()
        {
            var db = App.conn;
            string sql_load = @"SELECT * FROM Todo";
            using (var statement = db.Prepare(sql_load))
            {
                while (statement.Step() != SQLiteResult.DONE)
                {
                    Boolean is_finished = false;
                    if ((long)statement[3] == 1)
                    {
                        is_finished = true;
                    }
                    DateTime dateTime = DateTime.Parse((string)statement[4]);
                    Uri imgsrc = new Uri((string)statement[5]);
                    todoList.Add(new Models.TodoItem((long)statement[0],
                                                    (string)statement[1],
                                                    (string)statement[2],
                                                    is_finished,
                                                    dateTime,
                                                    imgsrc));
                }
            }
            //测试样例
        }

        public void AddTodoItem(string title, string details, DateTime dueDate, Uri imgSrc)
        {
            //TODO: 存入数据库并取出id
            var db = App.conn;
            //insert into db
            string sql_insert = @"INSERT
                                    INTO Todo(Title, Details, Is_Finished, DueDate, ImgSrc)
                                    VALUES (?,?,?,?,?)";
            var id = (long)0;
            string sql_select = @"SELECT last_insert_rowid() FROM Todo";
            try
            {
                using (var statement = db.Prepare(sql_insert))
                {
                    statement.Bind(1, title);
                    statement.Bind(2, details);
                    statement.Bind(3, 0);
                    statement.Bind(4, dueDate.ToString());
                    statement.Bind(5, imgSrc.ToString());
                    statement.Step();
                }
                using (var statement = db.Prepare(sql_select))
                {
                    while (statement.Step() != SQLiteResult.DONE)
                    {
                        id = (long)statement[0];
                    }
                }
            }
            catch (System.Exception ex)
            {
                //TODO:
            }

            var newitem = new Models.TodoItem(id, title, details,false, dueDate, imgSrc);
            this.todoList.Add(new Models.TodoItem(id, title, details, false, dueDate, imgSrc));
            
            newlist.Add(newitem);
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueueForWide310x150(true);
            updater.EnableNotificationQueueForSquare310x310(true);
            updater.EnableNotificationQueueForSquare150x150(true);
            updater.EnableNotificationQueue(true);
            updater.Clear();

            string AdaptiveTile = @"
                
<tile>
  <visual>

    <binding template='TileSmall' branding='none'>
      <image src='Assets\Tile_Background_4444Logo.png' placement='background' />
      <group>
        <subgroup hint-weight='40'>
          <text hint-style='subtitle'>{0}</text>
        </subgroup>
      </group>
    </binding>
    
    <binding template='TileMedium' branding='name'>
      <image src='Assets\Tile_Background_300300Logo.png' placement='background' />
      <group>
        <subgroup hint-weight='40'>
          <text hint-style='subtitle'>{0}</text>
          <text hint-style='captionsubtle' hint-wrap='true'>{1}</text>
        </subgroup>
      </group>
    </binding>

    <binding template='TileWide' branding='nameAndLogo'>
      <image src='Assets\Tile_Background_620300Logo.png' placement='background' />
      <group>
        <subgroup hint-weight='45'>
          <text hint-style='subtitle'>{0}</text>
          <text hint-style='captionsubtle' hint-wrap='true'>{1}</text>
        </subgroup>
      </group>
    </binding>

    <binding template='TileLarge' branding='nameAndLogo'>
      <image src='Assets\Tile_Background_300300Logo.png' placement='background' />
      <group>
        <subgroup hint-weight='45'>
          <text hint-style='subtitle'>{0}</text>
          <text hint-style='captionsubtle' hint-wrap='true'>{1}</text>
        </subgroup>
      </group>
    </binding>

  </visual>
</tile>";
            foreach (var n in newlist)
            {
                var doc = new XmlDocument();
                var xml = string.Format(AdaptiveTile, n.title, n.details);
                doc.LoadXml(WebUtility.HtmlDecode(xml), new XmlLoadSettings
                {
                    ProhibitDtd = false,
                    ValidateOnParse = false,
                    ElementContentWhiteSpace = false,
                    ResolveExternals = false
                });

                updater.Update(new TileNotification(doc));
            }
        }

        public void RemoveTodoItem(long id)
        {
            var toDelete = todoList.Single<Models.TodoItem>(i => i.id == id);
            todoList.Remove(toDelete);
            this.selectedItem = null;
            DeleteTodoItemDB(id);
        }

        public void UpdateTodoItem(long id, string title, string details, DateTime dueDate, bool isFinished, Uri imageSource)
        {
            var toUpdate = todoList.Single<Models.TodoItem>(i => i.id == id);
            toUpdate.title = title;
            toUpdate.details = details;
            toUpdate.dueDate = dueDate;
            toUpdate.isFinished = isFinished;
            toUpdate.imageUri = imageSource;

            UpdateTodoItemDB(toUpdate);

            this.selectedItem = null;
        }

        private Models.TodoItem GetTodoItemDB(long id)
        {
            var db = App.conn;
            Models.TodoItem todoitem = null;
            string sql_select = @"SELECT *
                                  FROM Todo
                                  WHERE Id = ?";
            using (var statement = db.Prepare(sql_select))
            {
                statement.Bind(1, id);
                while (SQLiteResult.DONE != statement.Step())
                {
                    Boolean is_finished = false;
                    if ((long)statement[3] == 1)
                    {
                        is_finished = true;
                    }
                    DateTime dateTime = DateTime.Parse((string)statement[4]);
                    Uri imgsrc = new Uri((string)statement[5]);
                    todoitem = new Models.TodoItem((long)statement[0],
                                                    (string)statement[1],
                                                    (string)statement[2],
                                                    is_finished,
                                                    dateTime,
                                                    imgsrc);
                }
            }
            return todoitem;
        }
        private void UpdateTodoItemDB(Models.TodoItem updatedItem)
        {
            var db = App.conn;
            string sql_update = @"UPDATE Todo
                                  SET Title = ?, Details = ?, Is_Finished = ?, DueDate = ?, ImgSrc = ?
                                  WHERE Id = ?";
            var existingTodoItem = GetTodoItemDB(updatedItem.id);
            if (existingTodoItem != null)
            {
                using (var statement = db.Prepare(sql_update))
                {
                    statement.Bind(1, updatedItem.title);
                    statement.Bind(2, updatedItem.details);
                    statement.Bind(3, updatedItem.isFinished?1:0);
                    statement.Bind(4, updatedItem.dueDate.ToString());
                    statement.Bind(5, updatedItem.imageUri.ToString());
                    statement.Bind(6, updatedItem.id);
                    statement.Step();
                }
            }
        }

        private void DeleteTodoItemDB(long id)
        {
            var db = App.conn;
            string sql_delete = @"DELETE
                                  FROM Todo
                                  WHERE Id = ?";
            using (var statement = db.Prepare(sql_delete))
            {
                statement.Bind(1, id);
                statement.Step();
            }
        }

        public void SearchTodoItem(string value, List<Models.TodoItem> result)
        {
            var db = App.conn;
            value = "%" + value + "%";
            string sql_search = @"SELECT *
                                  FROM Todo
                                  WHERE Title LIKE ? OR Details LIKE ? OR DueDate LIKE ?";
            using (var statement = db.Prepare(sql_search))
            {
                statement.Bind(1, value);
                statement.Bind(2, value);
                statement.Bind(3, value);
                while (statement.Step() != SQLiteResult.DONE)
                {
                    Boolean is_finished = false;
                    if ((long)statement[3] == 1)
                    {
                        is_finished = true;
                    }
                    DateTime dateTime = DateTime.Parse((string)statement[4]);
                    Uri imgsrc = new Uri((string)statement[5]);
                    result.Add(new Models.TodoItem((long)statement[0],
                                                    (string)statement[1],
                                                    (string)statement[2],
                                                    is_finished,
                                                    dateTime,
                                                    imgsrc));
                }
            }
        }
    }
}