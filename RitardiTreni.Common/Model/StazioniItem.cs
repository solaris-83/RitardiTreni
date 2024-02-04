using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RitardiTreni.Common.Model
{
    public class StazioniItem
    {
        public string NomeStazione {get;set;}
        public string CodiceStazione { get; set; }
        public string RegioneNome { get; set; }
        public string CodiceRegione { get; set; }
        public string City { get; set; }
        public double Latitudine { get; set; }
        public string Longitudine { get; set; }
    }
}
