using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using TrackMyAct.Pages;
using TrackMyAct.Models;
using System.Threading.Tasks;
using Windows.Storage;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TrackMyAct
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private DateTime startTime;
        private string timerdata;
        private TimeSpan timerdata_TimeSpan;
        //private DispatcherTimer _timer;
        private DispatcherTimer timer;
        private Library library;
        private RootObjectTrackAct rtrackact;
        private long countLimit;
        public MainPage()
        {
            this.InitializeComponent();
            library = new Library();
            countLimit = 14;
            //timerdata = "00:00:00";
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            bool res = await library.checkIfFileExists("activityDB");
            rtrackact = new RootObjectTrackAct();
            if (res)
            {
                string restring = await library.readFile("activityDB");
                rtrackact = TrackAct.trackactDataDeserializer(restring);
                Debug.WriteLine("Not the first Launch");
                int activity_pos = -1;
                for (int i = 0; i < rtrackact.activity.Count; i++)
                {
                    if (rtrackact.activity[i].name == activityName.Text)
                    {
                        activity_pos = i;
                    }
                }
                if (activity_pos == -1)
                {
                    StatisticsGrid.Opacity = 0;
                    personalBest.Opacity = 0;
                }
                else
                {
                    MedianTextBlock.Text = rtrackact.activity[activity_pos].median;
                    NinetyPercentileTextBlock.Text = rtrackact.activity[activity_pos].ninetypercentile;
                    personalBest.Text = "Your personal best is " + rtrackact.activity[activity_pos].personal_best;
                }
            }
            else
            {
                ApplicationData.Current.LocalSettings.Values["FirstLaunch"] = true;
                StatisticsGrid.Opacity = 0;
                personalBest.Opacity = 0;
            }
        }

        private void GoEllipse_Tapped(object sender, TappedRoutedEventArgs e)
        {
            startTimer();
        }

        private void RecycleButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void GoTextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            startTimer();
        }

        private void StopEllipse_Tapped(object sender, TappedRoutedEventArgs e)
        {
            stopTimer();
        }

        private void StopTextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            stopTimer();
        }

        private async void stopTimer()
        {
            timer.Stop();
            timer.Tick -= timer_Tick;
            Debug.WriteLine("Starting UpdateDB at : " + DateTime.Now.Millisecond);
            library.updateDB(TimerText.Text, timerdata_TimeSpan, activityName.Text);
            //Task.Delay(30);
            
            Storyboard myStoryboard;
            Debug.WriteLine("In Stop Timer");
            myStoryboard = (Storyboard)this.Resources["StopButtonPressed"];
            myStoryboard.Begin();
            GoTextBlock.IsTapEnabled = true;
            GoEllipse.IsTapEnabled = true;
            StopEllipse.IsTapEnabled = false;
            StopTextBlock.IsTapEnabled = false;
            
            RefreshUI();
            //library.updateDB(TimerText.Text, timerdata_TimeSpan, activityName.Text);

        }

        private void RefreshUI()
        {
            //string res = await library.readFile("activityDB");
            //RootObjectTrackAct rtrackact = TrackAct.trackactDataDeserializer(res);
            if ((bool)ApplicationData.Current.LocalSettings.Values["FirstLaunch"] == true)
            {
                try
                {
                    ActivityData ractivitydata = new ActivityData();
                    ractivitydata.name = activityName.Text;
                    TimerData tdata = new TimerData();
                    tdata.position = 0;             // Since this is a new activity, it won't have any data already associated with it.
                    tdata.time_in_seconds = (long)timerdata_TimeSpan.TotalSeconds;
                    ractivitydata.timer_data = new List<TimerData>();
                    ractivitydata.timer_data.Add(tdata);
                    ractivitydata.personal_best = String.Format("{0:00}:{1:00}:{2:00}", (long)timerdata_TimeSpan.TotalSeconds / 3600, ((long)timerdata_TimeSpan.TotalSeconds / 60) % 60, (long)timerdata_TimeSpan.TotalSeconds % 60);
                    ractivitydata.median = String.Format("{0:00}:{1:00}:{2:00}", (long)timerdata_TimeSpan.TotalSeconds / 3600, ((long)timerdata_TimeSpan.TotalSeconds / 60) % 60, (long)timerdata_TimeSpan.TotalSeconds % 60);
                    ractivitydata.ninetypercentile = String.Format("{0:00}:{1:00}:{2:00}", (long)timerdata_TimeSpan.TotalSeconds / 3600, ((long)timerdata_TimeSpan.TotalSeconds / 60) % 60, (long)timerdata_TimeSpan.TotalSeconds % 60);
                    rtrackact.activity = new List<ActivityData>();
                    rtrackact.activity.Add(ractivitydata);
                    personalBest.Text = "Your personal best is " + ractivitydata.personal_best;
                    personalBest.Opacity = 100;
                    MedianTextBlock.Text = ractivitydata.median;
                    NinetyPercentileTextBlock.Text = ractivitydata.ninetypercentile;
                    StatisticsGrid.Opacity = 100;
                    ApplicationData.Current.LocalSettings.Values["FirstLaunch"] = false;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("In Refresh UI FirstLaunch set to true : " + ex);
                }
            }
            else
            {

                int activity_pos = -1;
                for (int i = 0; i < rtrackact.activity.Count; i++)
                {
                    if (rtrackact.activity[i].name == activityName.Text)
                    {
                        activity_pos = i;
                    }
                }
                if (activity_pos == -1)
                {
                    ActivityData ractivitydata = new ActivityData();
                    ractivitydata.name = activityName.Text;
                    TimerData tdata = new TimerData();
                    tdata.position = 0;             // Since this is a new activity, it won't have any data already associated with it.
                    tdata.time_in_seconds = (long)timerdata_TimeSpan.TotalSeconds;
                    //ractivitydata.timer_data = new List<TimerData>();
                    ractivitydata.timer_data.Add(tdata);
                    ractivitydata.personal_best = String.Format("{0:00}:{1:00}:{2:00}", (long)timerdata_TimeSpan.TotalSeconds / 3600, ((long)timerdata_TimeSpan.TotalSeconds / 60) % 60, (long)timerdata_TimeSpan.TotalSeconds % 60);
                    ractivitydata.median = String.Format("{0:00}:{1:00}:{2:00}", (long)timerdata_TimeSpan.TotalSeconds / 3600, ((long)timerdata_TimeSpan.TotalSeconds / 60) % 60, (long)timerdata_TimeSpan.TotalSeconds % 60);
                    ractivitydata.ninetypercentile = String.Format("{0:00}:{1:00}:{2:00}", (long)timerdata_TimeSpan.TotalSeconds / 3600, ((long)timerdata_TimeSpan.TotalSeconds / 60) % 60, (long)timerdata_TimeSpan.TotalSeconds % 60);
                    rtrackact.activity.Add(ractivitydata);
                    personalBest.Text = "Your personal best is " + ractivitydata.personal_best;
                    personalBest.Opacity = 100;
                    MedianTextBlock.Text = ractivitydata.median;
                    NinetyPercentileTextBlock.Text = ractivitydata.ninetypercentile;
                    StatisticsGrid.Opacity = 100;
                }
                else
                {
                    TimerData tdata = new TimerData();
                    tdata.position = rtrackact.activity[activity_pos].timer_data[rtrackact.activity[activity_pos].timer_data.Count - 1].position + 1; // The mumbo jumbo is to get the value of 'position' in the last element in the track_data list and adding 1 to it.
                    if (tdata.position >= countLimit)
                    {
                        rtrackact.activity[activity_pos].timer_data.RemoveAt(0);
                    }
                    tdata.time_in_seconds = (long)timerdata_TimeSpan.TotalSeconds;

                    SortedSet<long> time_in_seconds = new SortedSet<long>();
                    for (int i = 0; i < rtrackact.activity[activity_pos].timer_data.Count; i++)
                    {
                        time_in_seconds.Add(rtrackact.activity[activity_pos].timer_data[i].time_in_seconds);
                    }
                    time_in_seconds.Add((long)timerdata_TimeSpan.TotalSeconds);
                    long mediansec = (time_in_seconds.ElementAtOrDefault(time_in_seconds.Count / 2));//time_in_seconds[time_in_seconds.Count / 2];
                    rtrackact.activity[activity_pos].median = String.Format("{0:00}:{1:00}:{2:00}", mediansec / 3600, (mediansec / 60) % 60, mediansec % 60);
                    int pos = (int)(0.9 * (time_in_seconds.Count - 1) + 1); // 0 1 3 4 5 8
                    long ninentypercentilesecond = (time_in_seconds.ElementAtOrDefault(pos));
                    rtrackact.activity[activity_pos].ninetypercentile = String.Format("{0:00}:{1:00}:{2:00}", ninentypercentilesecond / 3600, (ninentypercentilesecond / 60) % 60, ninentypercentilesecond % 60);
                    long personal_best = (time_in_seconds.ElementAtOrDefault(time_in_seconds.Count - 1));
                    rtrackact.activity[activity_pos].personal_best = String.Format("{0:00}:{1:00}:{2:00}", (personal_best) / 3600, ((personal_best) / 60) % 60, (personal_best) % 60);
                    rtrackact.activity[activity_pos].timer_data.Add(tdata);
                    personalBest.Text = "Your personal best is " + rtrackact.activity[activity_pos].personal_best;
                    personalBest.Opacity = 100;
                    MedianTextBlock.Text = rtrackact.activity[activity_pos].median;
                    NinetyPercentileTextBlock.Text = rtrackact.activity[activity_pos].ninetypercentile;
                    StatisticsGrid.Opacity = 100;
                }
            }
        }
        private void startTimer()
        {
            TimerText.Text = "00:00:00";
            Storyboard myStoryboard;
            Debug.WriteLine("In Start Timer");
            myStoryboard = (Storyboard)this.Resources["GoButtonPressed"];
            myStoryboard.Begin();
            GoTextBlock.IsTapEnabled = false;
            GoEllipse.IsTapEnabled = false;
            StopEllipse.IsTapEnabled = true;
            StopTextBlock.IsTapEnabled = true;
            startTime = DateTime.Now;
            //startTicks = DateTime.Now.Ticks;

            try
            {
                timer = new DispatcherTimer();
                timer.Tick += timer_Tick;
                timer.Interval = new TimeSpan(0, 0, 1);
                timer.Start();
                timer_Tick(null, null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in starting timer : " + ex);
            }
        }

        private void timer_Tick(object sender, object e)
        {
            try
            {
                timerdata_TimeSpan = DateTime.Now.Subtract(startTime);
                string subtract = (DateTime.Now.Subtract(startTime)).ToString();
                timerdata = subtract.Substring(0, 8);
                TimerText.Text = timerdata;
                Debug.WriteLine("Result of subtraction : " + timerdata);
                
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in timer_Tick : " + ex);
            }
        }

        private void Charts_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(AllTheData));
        }
    }
}
