using SimpleFeedReader;
using System.Text.RegularExpressions;

namespace TrackMyTrain.Data.Implementations
{
    public record ExtendedFeedItem : FeedItem
    {
        string pattern1 = @"modalità:|Data fine:|Settore:|Rilevanza:|Regione:|Provincia:|Sindacati:|Categoria interessata:|Data proclamazione:|Data ricezione:";
        string pattern2 = @"Data inizio:";
        public ExtendedFeedItem()
        {

        }

        public ExtendedFeedItem(FeedItem item) : base(item)
        {
            // Split the string using Regex
            string[] tokens = Regex.Split(item.Summary, pattern1, RegexOptions.IgnoreCase);
            Modalita = tokens[1].Trim();
            DataFine = tokens[2].Trim();
            Settore = tokens[3].Trim();
            Rilevanza = tokens[4].Trim();
            Regione = tokens[5].Trim();
            Provincia = tokens[6].Trim();
            Sindacati = tokens[7].Trim();
            CategoriaInteressata = tokens[8].Trim();
            DataProclamazione = tokens[9].Trim();
            DataRicezione = tokens[10].Trim();
            tokens = Regex.Split(item.Title, pattern2, RegexOptions.IgnoreCase);
            var index = tokens[1].IndexOf('-');
            if (index > -1)
            {
                DataInizio = tokens[1].Substring(0, index).Trim();
            }
            else
                DataInizio = tokens[1].Trim();
        }

        public string Modalita { get; set; }
        public string DataFine { get; set; }
        public string Settore { get; set; }
        public string Rilevanza { get; set; }
        public string Regione { get; set; }
        public string Provincia { get; set; }
        public string Sindacati { get; set; }
        public string CategoriaInteressata { get; set; }
        public string DataProclamazione { get; set; }
        public string DataRicezione { get; set; }
        public string DataInizio { get; set; }
    }
}
