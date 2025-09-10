

namespace TrackMyTrain.Data.Implementations
{
    public record RecentTrain(string Number, string DepartureStationName, string ArrivalStationName, string DepartureTime, string ArrivalTime, int CurrentDelay, bool HasWarning);
}
