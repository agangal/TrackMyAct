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

using TrackMyAct.Models;
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
        public MainPage()
        {
            this.InitializeComponent();
            library = new Library();
            //timerdata = "00:00:00";
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

        private void stopTimer()
        {
            Storyboard myStoryboard;
            Debug.WriteLine("In Stop Timer");
            myStoryboard = (Storyboard)this.Resources["StopButtonPressed"];
            myStoryboard.Begin();
            GoTextBlock.IsTapEnabled = true;
            GoEllipse.IsTapEnabled = true;
            StopEllipse.IsTapEnabled = false;
            StopTextBlock.IsTapEnabled = false;
            timer.Stop();
            timer.Tick -= timer_Tick;
            library.updateDB(TimerText.Text, timerdata_TimeSpan);
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
    }
}
