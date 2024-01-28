using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DataServiceLibrary.Responses
{
    public class OrarioTreni
    {
        
        public class Fermate
        {
            [JsonIgnore]
            public object orientamento { get; set; }
            [JsonIgnore]
            public object kcNumTreno { get; set; }
            public string stazione { get; set; }
            [JsonIgnore]
            public string id { get; set; }
            [JsonIgnore]
            public object listaCorrispondenze { get; set; }
            public object programmata { get; set; }
            [JsonIgnore]
            public object programmataZero { get; set; }
            [JsonIgnore]
            public object effettiva { get; set; }
            public int ritardo { get; set; }
            [JsonIgnore]
            public object partenzaTeoricaZero { get; set; }
            [JsonIgnore]
            public object arrivoTeoricoZero { get; set; }
            [JsonIgnore]
            public object partenza_teorica { get; set; }
            [JsonIgnore]
            public long? arrivo_teorico { get; set; }
            [JsonIgnore]
            public bool isNextChanged { get; set; }
            [JsonIgnore]
            public object partenzaReale { get; set; }
            [JsonIgnore]
            public long? arrivoReale { get; set; }
            [JsonIgnore]
            public int ritardoPartenza { get; set; }
            [JsonIgnore]
            public int ritardoArrivo { get; set; }
            [JsonIgnore]
            public int progressivo { get; set; }
            [JsonIgnore]
            public object binarioEffettivoArrivoCodice { get; set; }
            [JsonIgnore]
            public string binarioEffettivoArrivoTipo { get; set; }
            [JsonIgnore]
            public string binarioEffettivoArrivoDescrizione { get; set; }
            [JsonIgnore]
            public object binarioProgrammatoArrivoCodice { get; set; }
            [JsonIgnore]
            public string binarioProgrammatoArrivoDescrizione { get; set; }
            [JsonIgnore]
            public object binarioEffettivoPartenzaCodice { get; set; }
            [JsonIgnore]
            public string binarioEffettivoPartenzaTipo { get; set; }
            [JsonIgnore]
            public string binarioEffettivoPartenzaDescrizione { get; set; }
            [JsonIgnore]
            public object binarioProgrammatoPartenzaCodice { get; set; }
            [JsonIgnore]
            public string binarioProgrammatoPartenzaDescrizione { get; set; }
            public string tipoFermata { get; set; }
            [JsonIgnore]
            public bool visualizzaPrevista { get; set; }
            [JsonIgnore]
            public bool nextChanged { get; set; }
            [JsonIgnore]
            public int nextTrattaType { get; set; }
            [JsonIgnore]
            public int actualFermataType { get; set; }
        }


        public string tipoTreno { get; set; }
        [JsonIgnore]
        public object orientamento { get; set; }
        [JsonIgnore]
        public int codiceCliente { get; set; }
        [JsonIgnore]
        public object fermateSoppresse { get; set; }
        [JsonIgnore]
        public object dataPartenza { get; set; }
        public List<Fermate> fermate { get; set; }
        [JsonIgnore]
        public object anormalita { get; set; }
        [JsonIgnore]
        public object provvedimenti { get; set; }
        [JsonIgnore]
        public object segnalazioni { get; set; }
        public long? oraUltimoRilevamento { get; set; }
        public string stazioneUltimoRilevamento { get; set; }
        [JsonIgnore]
        public string idDestinazione { get; set; }
        [JsonIgnore]
        public string idOrigine { get; set; }
        [JsonIgnore]
        public List<object> cambiNumero { get; set; }
        [JsonIgnore]
        public bool hasProvvedimenti { get; set; }
        [JsonIgnore]
        public List<string> descOrientamento { get; set; }
        [JsonIgnore]
        public string compOraUltimoRilevamento { get; set; }
        [JsonIgnore]
        public object motivoRitardoPrevalente { get; set; }
        [JsonIgnore]
        public string descrizioneVCO { get; set; }
        public int numeroTreno { get; set; }
        public string categoria { get; set; }
        [JsonIgnore]
        public object categoriaDescrizione { get; set; }
        [JsonIgnore]
        public string origine { get; set; }
        [JsonIgnore]
        public object codOrigine { get; set; }
        [JsonIgnore]
        public string destinazione { get; set; }
        [JsonIgnore]
        public object codDestinazione { get; set; }
        [JsonIgnore]
        public object origineEstera { get; set; }
        [JsonIgnore]
        public object destinazioneEstera { get; set; }
        [JsonIgnore]
        public object oraPartenzaEstera { get; set; }
        [JsonIgnore]
        public object oraArrivoEstera { get; set; }
        [JsonIgnore]
        public int tratta { get; set; }
        [JsonIgnore]
        public int regione { get; set; }
        [JsonIgnore]
        public string origineZero { get; set; }
        [JsonIgnore]
        public string destinazioneZero { get; set; }
        [JsonIgnore]
        public long orarioPartenzaZero { get; set; }
        [JsonIgnore]
        public long orarioArrivoZero { get; set; }
        [JsonIgnore]
        public bool circolante { get; set; }
        [JsonIgnore]
        public object binarioEffettivoArrivoCodice { get; set; }
        [JsonIgnore]
        public object binarioEffettivoArrivoDescrizione { get; set; }
        [JsonIgnore]
        public object binarioEffettivoArrivoTipo { get; set; }
        [JsonIgnore]
        public object binarioProgrammatoArrivoCodice { get; set; }
        [JsonIgnore]
        public object binarioProgrammatoArrivoDescrizione { get; set; }
        [JsonIgnore]
        public object binarioEffettivoPartenzaCodice { get; set; }
        [JsonIgnore]
        public object binarioEffettivoPartenzaDescrizione { get; set; }
        [JsonIgnore]
        public object binarioEffettivoPartenzaTipo { get; set; }
        [JsonIgnore]
        public object binarioProgrammatoPartenzaCodice { get; set; }
        [JsonIgnore]
        public object binarioProgrammatoPartenzaDescrizione { get; set; }
        public string subTitle { get; set; }
        [JsonIgnore]
        public string esisteCorsaZero { get; set; }
        [JsonIgnore]
        public bool inStazione { get; set; }
        [JsonIgnore]
        public bool haCambiNumero { get; set; }
        [JsonIgnore]
        public bool nonPartito { get; set; }
        public int provvedimento { get; set; }
        [JsonIgnore]
        public object riprogrammazione { get; set; }
        [JsonIgnore]
        public long orarioPartenza { get; set; }
        [JsonIgnore]
        public long orarioArrivo { get; set; }
        [JsonIgnore]
        public object stazionePartenza { get; set; }
        [JsonIgnore]
        public object stazioneArrivo { get; set; }
        [JsonIgnore]
        public object statoTreno { get; set; }
        [JsonIgnore]
        public object corrispondenze { get; set; }
        [JsonIgnore]
        public List<object> servizi { get; set; }
        [JsonIgnore]
        public int ritardo { get; set; }
        [JsonIgnore]
        public string tipoProdotto { get; set; }
        [JsonIgnore]
        public string compOrarioPartenzaZeroEffettivo { get; set; }
        [JsonIgnore]
        public string compOrarioArrivoZeroEffettivo { get; set; }
        [JsonIgnore]
        public string compOrarioPartenzaZero { get; set; }
        [JsonIgnore]
        public string compOrarioArrivoZero { get; set; }
        [JsonIgnore]
        public string compOrarioArrivo { get; set; }
        [JsonIgnore]
        public string compOrarioPartenza { get; set; }
        [JsonIgnore]
        public string compNumeroTreno { get; set; }
        [JsonIgnore]
        public List<string> compOrientamento { get; set; }
        [JsonIgnore]
        public string compTipologiaTreno { get; set; }
        [JsonIgnore]
        public string compClassRitardoTxt { get; set; }
        [JsonIgnore]
        public string compClassRitardoLine { get; set; }
        [JsonIgnore]
        public string compImgRitardo2 { get; set; }
        [JsonIgnore]
        public string compImgRitardo { get; set; }
        [JsonIgnore]
        public List<string> compRitardo { get; set; }
        [JsonIgnore]
        public List<string> compRitardoAndamento { get; set; }
        [JsonIgnore]
        public List<string> compInStazionePartenza { get; set; }
        [JsonIgnore]
        public List<string> compInStazioneArrivo { get; set; }
        [JsonIgnore]
        public string compOrarioEffettivoArrivo { get; set; }
        [JsonIgnore]
        public string compDurata { get; set; }
        [JsonIgnore]
        public string compImgCambiNumerazione { get; set; }
    }
    
}
