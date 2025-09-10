using SQLite;

namespace TrackMyTrain.Maui.LocalDb.Models
{
    [Table("Trains")]
    public class Trains : TableBase
    {
        [Indexed(Name = "IX_TrainNumber", Order = 1)]
        public string Number { get; set; }

        [Indexed(Name = "IX_DepartureStationName", Order = 2)]
        public string DepartureStationName { get; set; }

        public string DepartureStationShortCode {  get; set; }

        public string ArrivalStationName { get; set; }

        public string ArrivalStationShortCode { get; set; }

        public bool IsFavorite {  get; set; }

        public Trains()
        {
            
        }

    }
}
