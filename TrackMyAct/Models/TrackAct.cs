using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace TrackMyAct.Models
{
    public class TrackAct
    {
        public static RootObjectTrackAct trackactDataDeserializer(string response)
        {
            RootObjectTrackAct rtrackact = JsonConvert.DeserializeObject<RootObjectTrackAct>(response);
            return rtrackact;
        }

        public static string trackactSerializer(RootObjectTrackAct rtrackact)
        {
            string response = JsonConvert.SerializeObject(rtrackact);
            return response;
        }
    }

    [DataContract]
    public class TimerData
    {
        [DataMember]
        public int time_in_seconds { get; set; }
    }

    [DataContract]
    public class ActivityData
    {
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string median { get; set; }
        [DataMember]
        public string ninetypercentile { get; set; }
        [DataMember]
        public int? current_count { get; set; }
        [DataMember]
        public List<TimerData> timer_data { get; set; }
    }

    [DataContract]
    public class Activity
    {
        [DataMember]
        public List<ActivityData> data { get; set; }
    }

    [DataContract]
    public class Collection
    {
        [DataMember]
        public List<Activity> activity { get; set; }
    }

    [DataContract]
    public class RootObjectTrackAct
    {
        [DataMember]
        public Collection collection { get; set; }
    }
}
