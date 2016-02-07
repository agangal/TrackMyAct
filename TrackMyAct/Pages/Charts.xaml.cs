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

using TrackMyAct.Models;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TrackMyAct.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Charts : Page
    {
        private Library library;
        public RootObjectTrackAct rtrackact;
        public class time_data
        {
            public int count { get; set; }
            public long time_in_seconds  { get; set; }
            public DateTime time_in_DT { get; set; }
        }

        public Charts()
        {
            this.InitializeComponent();
            library = new Library();
            this.Loaded += MainPage_Loaded;
            rtrackact = new RootObjectTrackAct();
        }
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadChartContents();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            rtrackact = (RootObjectTrackAct)e.Parameter;
           // progressRing.IsActive = true;
        }
        
        private void LoadChartContents()
        {
            
            List<time_data> financialStuffList = new List<time_data>();
            for(int i=0; i < rtrackact.activity[0].timer_data.Count; i++)
            {
                financialStuffList.Add(new time_data() { count = i, time_in_seconds = rtrackact.activity[0].timer_data[i].time_in_seconds, time_in_DT = new DateTime(1970,01,01,((int)rtrackact.activity[0].timer_data[i].time_in_seconds)/3600, (((int)rtrackact.activity[0].timer_data[i].time_in_seconds)/60)%60, ((int)rtrackact.activity[0].timer_data[i].time_in_seconds)%60)});
            }
           (MyChart.Series[0] as ColumnSeries).ItemsSource = financialStuffList;
           // progressRing.IsActive = false;

        }
    }

   
}
