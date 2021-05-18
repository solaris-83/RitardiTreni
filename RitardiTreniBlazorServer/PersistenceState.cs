using DataServiceLibrary.Model;
using System.Collections.Generic;

namespace RitardiTreniBlazorServer
{
    public class PersistenceState
    {
        public string TrattaSelezionata { get; set; }
        public IEnumerable<DataItem> ListaTreni { get; set; }
    }
}
