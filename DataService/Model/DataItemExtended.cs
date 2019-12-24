using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataServiceLibrary.Model
{
    public class DataItemExtended 
    {


        public string OraUltimoRilevamento
        {
            get;
            set;
        }

        public string StazioneUltimoRilevamente
        {
            get;
            set;
        }

        public string Categoria
        {
            get;
            set;
        }

        public string NumeroTreno
        {
            get;
            set;
        }

        public List<DataItem> DataList { get; set; }

        public DataItemExtended()
        {
            if (DataList == null)
                DataList = new List<DataItem>();
        }
    }
}
