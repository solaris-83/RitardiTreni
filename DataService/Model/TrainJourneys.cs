using System.Collections.Generic;

namespace DataServiceLibrary.Model
{
    public class TrainJourneys
    {
        public string Name { get; set; }
        public List<string> StationCodesFrom { get; set; }
        public List<string> StationCodesTo { get; set; }
        public string Pattern { get; set; }
    }
}
