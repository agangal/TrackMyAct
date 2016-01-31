using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using TrackMyAct.Models;
using System.Diagnostics;

namespace TrackMyAct
{
    public class Library
    {
        private int countLimit;
        public Library()
        {
            countLimit = 14;
        }
        private async Task<bool> checkIfFileExists(string filename)
        {
            var item = await ApplicationData.Current.LocalFolder.TryGetItemAsync(filename);
            if (item == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<string> readFile(string filename)
        {
            var applicationData = Windows.Storage.ApplicationData.Current;
            var localFolder = applicationData.LocalFolder;
            string response = null;
            try
            {
                Debug.WriteLine("reading file : " + filename);
                StorageFile sampleFile = await localFolder.GetFileAsync(filename);
                response = await FileIO.ReadTextAsync(sampleFile);
            }
            catch (System.UnauthorizedAccessException e)
            {
                Debug.WriteLine("In reading file :"+ filename +" : System Unauthorized exception : " + e);
            }
            return response;
        }

        public async Task<bool> writeFile(string filename, string response)
        {
            var applicationData = Windows.Storage.ApplicationData.Current;
            var localFolder = applicationData.LocalFolder;
            Debug.WriteLine("In writeFile : " + filename);
            try
            {
                //Debug.WriteLine("In try of write.");
                StorageFile sampleFile = await localFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(sampleFile, response);
            }
            catch (System.UnauthorizedAccessException e)
            {
                Debug.WriteLine("in write to file : " + filename + " : " + e);
                return false;
            }
            return true;
        }

        public async void updateDB(string timerText, TimeSpan timerdata, string activityName)
        {
            bool res = await checkIfFileExists("activityDB");
            RootObjectTrackAct rtrackact = new RootObjectTrackAct();
            if (res)
            {
                string response = await readFile("activityDB");
                rtrackact = TrackAct.trackactDataDeserializer(response);
                int activity_pos = -1;
                for (int i = 0; i < rtrackact.activity.Count; i++)
                {
                    if (rtrackact.activity[i].name == activityName)
                    {
                        activity_pos = i;
                    }
                }
                /// If the activity exists 
                if (activity_pos != -1)
                {
                    TimerData tdata = new TimerData();
                    tdata.position = rtrackact.activity[activity_pos].timer_data[rtrackact.activity[activity_pos].timer_data.Count - 1].position + 1; // The mumbo jumbo is to get the value of 'position' in the last element in the track_data list and adding 1 to it.
                    if (tdata.position >= countLimit)
                    {
                        rtrackact.activity[activity_pos].timer_data.RemoveAt(0);
                    }
                    tdata.time_in_seconds = timerdata.Seconds;
                    rtrackact.activity[activity_pos].timer_data.Add(tdata);
                }
                /// If the activity does not exist
                else
                {
                    ActivityData ractivitydata = new ActivityData();
                    ractivitydata.name = activityName;
                    TimerData tdata = new TimerData();
                    tdata.position = 0;             // Since this is a new activity, it won't have any data already associated with it.
                    tdata.time_in_seconds = timerdata.Seconds;
                    ractivitydata.timer_data.Add(tdata);
                    rtrackact.activity.Add(ractivitydata);
                }
            }
            else
            {
                ActivityData ractivitydata = new ActivityData();
                ractivitydata.name = activityName;
                TimerData tdata = new TimerData();
                tdata.position = 0;             // Since this is a new activity, it won't have any data already associated with it.
                tdata.time_in_seconds = timerdata.Seconds;
                ractivitydata.timer_data = new List<TimerData>();
                ractivitydata.timer_data.Add(tdata);
                rtrackact.activity = new List<ActivityData>();
                rtrackact.activity.Add(ractivitydata);
            }
            await writeFile("activityDB", TrackAct.trackactSerializer(rtrackact));
        }

        private void updateDBFileExists(string timerText, TimeSpan timerdata, int activity_pos)
        {
            if(activity_pos != -1)
            {
                TimerData tdata = new TimerData();
                //tdata.position = 
            }
        }

    }
}
