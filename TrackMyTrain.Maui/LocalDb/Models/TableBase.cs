using SQLite;

namespace TrackMyTrain.Maui.LocalDb.Models
{
    public class TableBase
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public long CreatedAt { get; set; } 
        public long LastUpdatedAt { get; set; }

        public TableBase()
        {
            
        }
    }
}