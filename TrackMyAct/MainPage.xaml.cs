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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TrackMyAct
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

       
        
        private void GoEllipse_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Storyboard myStoryboard;
            Debug.WriteLine("In Go Ellipse Tapped Event");
            myStoryboard = (Storyboard)this.Resources["GoButtonPressed"];
            myStoryboard.Begin();
            GoTextBlock.IsTapEnabled = false;
            GoEllipse.IsTapEnabled = false;
            StopEllipse.IsTapEnabled = true;
            StopTextBlock.IsTapEnabled = true;
        }

        private void RecycleButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void GoTextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Storyboard myStoryboard;
            Debug.WriteLine("In Go Text Tapped Event");
            myStoryboard = (Storyboard)this.Resources["GoButtonPressed"];
            myStoryboard.Begin();
            GoTextBlock.IsTapEnabled = false;
            GoEllipse.IsTapEnabled = false;
            StopEllipse.IsTapEnabled = true;
            StopTextBlock.IsTapEnabled = true;
        }

        private void StopEllipse_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Storyboard myStoryboard;
            Debug.WriteLine("In Stop Ellipse Tapped Event");
            myStoryboard = (Storyboard)this.Resources["StopButtonPressed"];
            myStoryboard.Begin();
            GoTextBlock.IsTapEnabled = true;
            GoEllipse.IsTapEnabled = true;
            StopEllipse.IsTapEnabled = false;
            StopTextBlock.IsTapEnabled = false;
        }

        private void StopTextBlock_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Storyboard myStoryboard;
            Debug.WriteLine("In Stop Text Tapped Event");
            myStoryboard = (Storyboard)this.Resources["StopButtonPressed"];
            myStoryboard.Begin();
            GoTextBlock.IsTapEnabled = true;
            GoEllipse.IsTapEnabled = true;
            StopEllipse.IsTapEnabled = false;
            StopTextBlock.IsTapEnabled = false;
        }
    }
}
