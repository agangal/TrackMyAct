using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace TrackMyAct
{
    public class Library
    {
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

        public void updateDB(string timer, TimeSpan timerdata)
        {

        }

    }
}
