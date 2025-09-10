using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackMyTrain.Maui.Utilities
{
    public static class GenericUtilities
    {
        public static bool CheckInternetConnection()
        {
            return Connectivity.NetworkAccess == NetworkAccess.Internet;
        }
    }
}
