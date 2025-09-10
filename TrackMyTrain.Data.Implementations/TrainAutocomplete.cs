using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackMyTrain.Data.Implementations
{
    public record TrainAutocomplete(string DepartureStationName, string TrainNumber, string DepartureStationShortCode, long TimeStamp);
}
