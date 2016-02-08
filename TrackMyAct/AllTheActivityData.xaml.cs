using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TrackMyAct
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AllTheActivityData : Page
    {
        public ObservableCollection<formatTimeData> tmdata { get; set; }
        public AllTheActivityData()
        {
            this.InitializeComponent();
            tmdata = new ObservableCollection<formatTimeData>();
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ActivityData actdata = (ActivityData)e.Parameter;
            activityName.Text = actdata.name;
            tmdata.Clear();
            var tdata = actdata.timer_data;
            foreach(var td in tdata)
            {
                formatTimeData frtd = new formatTimeData();
                frtd.pos = td.position + 1;
                frtd.time_in_string = String.Format("{0:00}:{1:00}:{2:00}", (long)td.time_in_seconds / 3600, ((long)td.time_in_seconds / 60) % 60, (long)td.time_in_seconds % 60);
                frtd.datetime = String.Format("{0:ddd, MMM d, yyyy}", td.startTime);
                tmdata.Add(frtd);
            }
        }
    }
}
