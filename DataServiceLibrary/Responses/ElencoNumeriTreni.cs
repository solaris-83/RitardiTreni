using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary.Responses
{
    public class ElencoNumeriTreni
    {
        public class Vehicle
        {
            public string origine { get; set; }
            public string destinazione { get; set; }
            public DateTime orarioPartenza { get; set; }
            public DateTime orarioArrivo { get; set; }
            public string categoria { get; set; }
            public string categoriaDescrizione { get; set; }
            public string numeroTreno { get; set; }
        }

        public class Soluzioni
        {
            public string durata { get; set; }
            public List<Vehicle> vehicles { get; set; }
        }

       
            public List<Soluzioni> soluzioni { get; set; }
            public string origine { get; set; }
            public string destinazione { get; set; }
            public object errore { get; set; }
        
    }
}
