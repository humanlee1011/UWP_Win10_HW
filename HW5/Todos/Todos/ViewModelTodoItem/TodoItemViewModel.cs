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
            //测试样例
            this.todoList.Add(new Models.TodoItem("现操作业", "周末必须完成", Convert.ToDateTime("03/17/2017")));
            this.todoList.Add(new Models.TodoItem("xx考试", "复习1、5、6、7、10章", Convert.ToDateTime("04/16/2017")));
            this.todoList.Add(new Models.TodoItem("消费者日", "有xxxxx活动", Convert.ToDateTime("03/15/2017")));
            this.todoList.Add(new Models.TodoItem("院系赛", "锦标赛", Convert.ToDateTime("03/20/2017")));
        }

        public void AddTodoItem(string title, string details, DateTime dueDate)
        {
            var newitem = new Models.TodoItem(title, details, dueDate);
            this.todoList.Add(new Models.TodoItem(title, details, dueDate));
            
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

            //Construct the tile content
            //TileContent content = new TileContent()
            //{
            //    Visual = new TileVisual()
            //    {
            //        TileSmall = new TileBinding()
            //        {
            //            Branding = TileBranding.None,
            //            Content = new TileBindingContentAdaptive()
            //            {
            //                BackgroundImage = new TileBackgroundImage()
            //                {
            //                    Source = "Assets/Tile_Background_4444Logo.png",
            //                    HintOverlay = 40
            //                },

            //                Children =
            //                {
            //                    new AdaptiveGroup()
            //                    {
            //                        Children =
            //                        {
            //                            new AdaptiveSubgroup()
            //                            {
            //                                Children =
            //                                {
            //                                    new AdaptiveText()
            //                                    {
            //                                        Text = title,
            //                                        HintStyle = AdaptiveTextStyle.Subtitle
            //                                    }
            //                                },
            //                                HintWeight = 1
            //                            }
            //                        }
            //                    },
                                
            //                }
            //            }
            //        },

            //        TileMedium = new TileBinding()
            //        {
            //            Branding = TileBranding.Name,
            //            Content = new TileBindingContentAdaptive()
            //            {
            //                BackgroundImage = new TileBackgroundImage()
            //                {
            //                    Source = "Assets/Tile_Background_300300Logo.png",
            //                    HintOverlay = 40
            //                },
            //                Children =
            //                {
                                
            //                    new AdaptiveText()
            //                    {
            //                        Text = title,
            //                        HintStyle = AdaptiveTextStyle.Subtitle
            //                    },

            //                    new AdaptiveText()
            //                    {
            //                        Text = details,
            //                        HintStyle = AdaptiveTextStyle.CaptionSubtle
            //                    }
            //                }
            //            }
            //        },

            //        TileWide = new TileBinding()
            //        {
            //            Branding = TileBranding.Name,
            //            Content = new TileBindingContentAdaptive()
            //            {
            //                BackgroundImage = new TileBackgroundImage()
            //                {
            //                    Source = "Assets/Tile_Background_620300Logo.png",
            //                    HintOverlay = 40
            //                },
            //                Children =
            //                {
            //                    new AdaptiveText()
            //                    {
            //                        Text = title,
            //                        HintStyle = AdaptiveTextStyle.Subtitle
            //                    },

            //                    new AdaptiveText()
            //                    {
            //                        Text = details,
            //                        HintStyle = AdaptiveTextStyle.CaptionSubtle
            //                    }
            //                }
            //            }
            //        },
            //        TileLarge = new TileBinding()
            //        {
            //            Branding = TileBranding.Name,
            //            Content = new TileBindingContentAdaptive()
            //            {
            //                BackgroundImage = new TileBackgroundImage()
            //                {
            //                    Source = "Assets/Tile_Background_300300Logo.png",
            //                    HintOverlay = 40
            //                },

            //                Children =
            //                {
            //                    new AdaptiveText()
            //                    {
            //                        Text = title,
            //                        HintStyle = AdaptiveTextStyle.Subtitle
            //                    },

            //                    new AdaptiveText()
            //                    {
            //                        Text = details,
            //                        HintStyle = AdaptiveTextStyle.CaptionSubtle
            //                    }
            //                }
            //            }
            //        }
            //    }
            //};
            //var notification = new TileNotification(content.GetXml());
            //TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
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