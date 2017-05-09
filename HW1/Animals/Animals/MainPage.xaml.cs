using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Animals
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //声明一个委托
        private delegate string AnimalSpeaking(object sender, myEventArgs e);
        //委托声明一个事件
        private event AnimalSpeaking Speak;
        private int times = 0;
        
        public MainPage()
        {
            this.InitializeComponent();
        }

        //声明一个接口
        interface Animals
        {
            //方法
            string speak(object sender, myEventArgs e);
        }

        class Cat : Animals
        {
            TextBlock words;
            
            public Cat(TextBlock w)
            {
                this.words = w;
            }

            public string speak(object sender, myEventArgs e)
            {
                this.words.Text += "I am a cat.\n";
                return "";
            }
        }

        class Dog : Animals
        {
            TextBlock words;

            public Dog(TextBlock w)
            {
                this.words = w;
            }

            public string speak(object sender, myEventArgs e)
            {
                this.words.Text += "I am a dog.\n";
                return "";
            }
        }

        class Pig : Animals
        {
            TextBlock words;

            public Pig(TextBlock w)
            {
                this.words = w;
            }

            public string speak(object sender, myEventArgs e)
            {
                this.words.Text += "I am a pig.\n";
                return "";
            }
        }

        private Cat cat;
        private Dog dog;
        private Pig pig;

        private void Publish_Or_Subscribe()
        {
            //实例化动物
            if (times == 0)
            {
                words.Text = "";
                cat = new Cat(words);
                dog = new Dog(words);
                pig = new Pig(words);
            }
            else if (Speak != null)
            {
                //注销事件
                Speak -= cat.speak;
                Speak -= dog.speak;
                Speak -= pig.speak;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Random ran = new Random();
            int randomNum = ran.Next(0, 3);

            //随机选取一种动物说话
            //注册事件
            Publish_Or_Subscribe();
            if (randomNum == 0)
            {
                Speak += cat.speak;
            }
            else if (randomNum == 1)
            {
                Speak += dog.speak;
            }
            else if (randomNum == 2)
            {
                Speak += pig.speak;
            }
            //执行事件
            Speak(this, new myEventArgs(times++));
        }

        

        private void Choose_Speak(object sender, RoutedEventArgs e)
        {
            string text = inputText.Text;
            Publish_Or_Subscribe();
            if (text == "dog")
            {
                Speak += dog.speak;
            }
            else if (text == "cat")
            {
                Speak += cat.speak;
            }
            else if (text == "pig")
            {
                Speak += pig.speak;
            }
            else
            {
                inputText.Text = "";
                return;
            }
            Speak(this, new myEventArgs(times++));
            inputText.Text = "";
        }

        class myEventArgs : EventArgs
        {
            public int t = 0;
            public myEventArgs(int tt)
            {
                t = tt;
            }
        }
    }
}
