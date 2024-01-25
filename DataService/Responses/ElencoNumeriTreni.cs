using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataServiceLibrary.Responses
{
    public class ElencoNumeriTreni
    {
        public class Vehicle
        {
            [JsonIgnore]
            public string origine { get; set; }
            [JsonIgnore]
            public string destinazione { get; set; }
            [JsonIgnore]
            public DateTime orarioPartenza { get; set; }
            [JsonIgnore]
            public DateTime orarioArrivo { get; set; }
            public string categoria { get; set; }
            public string categoriaDescrizione { get; set; }
            public string numeroTreno { get; set; }
        }

        public class Soluzioni
        {
            [JsonIgnore]
            public string durata { get; set; }
            public List<Vehicle> vehicles { get; set; }
        }
       
        public List<Soluzioni> soluzioni { get; set; }
        [JsonIgnore]
        public string origine { get; set; }
        [JsonIgnore]
        public string destinazione { get; set; }
        [JsonIgnore]
        public object errore { get; set; }
    }
}
