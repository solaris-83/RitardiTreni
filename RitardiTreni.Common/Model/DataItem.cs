namespace RitardiTreni.Common.Model
{
    public class DataItem
    {
        public string NumeroTreno
        {
            get;
            private set;
        }

        public string NomeStazione
        {
            get;
            private set;
        }

        public int MinutiRitardo
        {
            get;
            private set;
        }

        public bool IsRitardoSignificativo
        {
            get { return MinutiRitardo > 10; }
        }

        public string OrarioProgrammatoPartenza
        {
            get;
            private set;
        }

        public bool IsBold
        {
            get;
            private set;
        }


        public DataItem(string numeroTreno, string nomeStazione, int minutiRitardo, string orarioProgrammatoPartenza,  bool isBold)
        {
            NumeroTreno = numeroTreno;
            NomeStazione = nomeStazione;
            MinutiRitardo = minutiRitardo;
            OrarioProgrammatoPartenza = orarioProgrammatoPartenza;
            IsBold = isBold;
        }
    }
}