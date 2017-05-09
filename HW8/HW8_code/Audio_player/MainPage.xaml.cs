using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace Audio_player
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            _mediaPlayer = new MediaPlayer();
            var mediaSource = MediaSource.CreateFromUri(new Uri("ms-appx:///Assets/default.mp3"));
            mediaSource.OpenOperationCompleted += MediaSource_OpenOperationCompleted;
            _mediaPlayer.Source = mediaSource;
            //_mediaPlayer.AudioCategory = MediaPlayerAudioCategory.Movie;
            _mediaTimelineController = new MediaTimelineController();
            _mediaPlayer.CommandManager.IsEnabled = false;
            _mediaPlayer.TimelineController = _mediaTimelineController;
            _mediaTimelineController.PositionChanged += _mediaTimelineController_PositionChanged;
            //bind to mediaPlayerElement
            _mediaPlayerElement.SetMediaPlayer(_mediaPlayer);
        }

        private MediaPlayer _mediaPlayer;
        private TimeSpan _duration;
        private MediaTimelineController _mediaTimelineController;
        private double value = 0;
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            _mediaTimelineController.Start();
            InitailizePropertyValues();
        }
        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_mediaTimelineController.State == MediaTimelineControllerState.Running)
            {
                _mediaTimelineController.Pause();
                PauseButton.Label = "继续";
            }
            else
            {
                _mediaTimelineController.Resume();
                PauseButton.Label = "暂停";
            }

        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _mediaTimelineController.Start();
            _mediaTimelineController.Pause();
            _positionSlider.Value = (double)0;
        }

        private async void SelectAudioButton_Click(object sender, RoutedEventArgs e)
        {
            await SetLocalMedia();
        }

        private void FullScreenToggle(object sender, RoutedEventArgs e)
        {
            var view = ApplicationView.GetForCurrentView();
            if (view.IsFullScreenMode)
            {
                view.ExitFullScreenMode();
                // The SizeChanged event will be raised when the exit from full screen mode is complete.
            }
            else
            {
                view.TryEnterFullScreenMode();
                _mediaPlayerElement.Width = Window.Current.Bounds.Width;
                _mediaPlayerElement.Height = Window.Current.Bounds.Height - 50;
            }
        }

        

        private void volumeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            _mediaPlayer.Volume = (double)volumeSlider.Value * 0.01;
            
        }

        void InitailizePropertyValues()
        {
            _mediaPlayer.Volume = (double)0.5;
            volumeSlider.Value = (double)50;
        }

        private async void MediaSource_OpenOperationCompleted(MediaSource sender, MediaSourceOpenOperationCompletedEventArgs args)
        {
            _duration = sender.Duration.GetValueOrDefault();

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                _positionSlider.Minimum = 0;
                _positionSlider.Maximum = _duration.TotalSeconds;
                total_time.Text = _duration.TotalMinutes.ToString("F0") + ":" + (_duration.TotalSeconds % 60).ToString("F0");
                _positionSlider.StepFrequency = 1;
            });
        }
        private async void _mediaTimelineController_PositionChanged(MediaTimelineController sender, object args)
        {
            if (_duration != TimeSpan.Zero)
            {
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    if (value == 0)
                    {
                        _positionSlider.Value = sender.Position.TotalSeconds;
                        cur_time.Text = sender.Position.TotalMinutes.ToString("F0") + ":" + (sender.Position.TotalSeconds % 60).ToString("F0");
                    }
                });
            }
        }

        private void _positionslider_valuechanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (_mediaPlayer != null && value == 0)
            {
                value = _positionSlider.Value;
                _mediaTimelineController.Position += TimeSpan.FromSeconds(value - _mediaTimelineController.Position.TotalSeconds);
                value = 0;
            }
        }
        async private System.Threading.Tasks.Task SetLocalMedia()
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            openPicker.FileTypeFilter.Add(".wmv");
            openPicker.FileTypeFilter.Add(".mp4");
            openPicker.FileTypeFilter.Add(".wma");
            openPicker.FileTypeFilter.Add(".mp3");

            var file = await openPicker.PickSingleFileAsync();

            // mediaPlayer is a MediaPlayerElement defined in XAML
            if (file != null)
            {
                var mediaSource  = MediaSource.CreateFromStorageFile(file);
                mediaSource.OpenOperationCompleted += MediaSource_OpenOperationCompleted;
                _mediaPlayer.Source = mediaSource;
                if (file.FileType == ".mp3")
                {
                    ellipse.Visibility = Visibility.Visible;
                }
                else
                {
                    ellipse.Visibility = Visibility.Collapsed;
                }
                _mediaTimelineController.Start();
            }
        }
    }
}
