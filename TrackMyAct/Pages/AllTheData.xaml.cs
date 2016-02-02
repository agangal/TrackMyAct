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

namespace TrackMyAct.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AllTheData : Page
    {
        public ObservableCollection<ActivityData> activity { get; set; }
        public ObservableCollection<TimerData> tmdata { get; set; }
        public Library library;
        public AllTheData()
        {
            this.InitializeComponent();
            library = new Library();
            activity = new ObservableCollection<ActivityData>();
            tmdata = new ObservableCollection<TimerData>();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if(await library.checkIfFileExists("activityDB"))
            {
                string fileres = await library.readFile("activityDB");
                RootObjectTrackAct rtrackact = TrackAct.trackactDataDeserializer(fileres);
                var activityD = rtrackact.activity;
                foreach(var actv in activityD)
                {
                    activity.Add(actv);
                }
                var timedata = rtrackact.activity[0].timer_data;
                foreach(var tdata in timedata)
                {
                    tmdata.Add(tdata);
                }
            }
        }
    }
}
