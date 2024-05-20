

namespace RitardiTreni.Common.Model
{
    public class DataResult
    {
        public string Journey { get; set; }
        public List<TrainResult> Results { get; set; }
        public DataResult()
        {
            Results = new List<TrainResult>();
        }
    }

    public class TrainResult
    {
        public TrainResult()
        {
            TrainStations = new List<Station>();
        }

        public string Number { get; set; }
        public List<Station> TrainStations { get; set; }
        public DelayType SummaryResult => GetSummaryResult();

        private DelayType GetSummaryResult()
        {
            if (TrainStations.Any(s => s.DelayInMinutes > 5 && s.DelayInMinutes <= 10))
                return DelayType.SmallDelay;
            else if (TrainStations.Any(s => s.DelayInMinutes > 10))
                return DelayType.BigDelay;
            return DelayType.NoDelay;
        }
    }

    public class Station
    {
        public string Name { get; set; }
        public int DelayInMinutes { get; set; }
        public string ExpectedLeaving { get; set; }
    }

    public enum DelayType
    {
        NoDelay = 0,
        SmallDelay,
        BigDelay
    }
}