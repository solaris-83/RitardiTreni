using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataServiceLibrary.Responses
{
    public class OrarioTreni
    {
        
        public class Fermate
        {
            public object orientamento { get; set; }
            public object kcNumTreno { get; set; }
            public string stazione { get; set; }
            public string id { get; set; }
            public object listaCorrispondenze { get; set; }
            public object programmata { get; set; }
            public object programmataZero { get; set; }
            public object effettiva { get; set; }
            public int ritardo { get; set; }
            public object partenzaTeoricaZero { get; set; }
            public object arrivoTeoricoZero { get; set; }
            public object partenza_teorica { get; set; }
            public long? arrivo_teorico { get; set; }
            public bool isNextChanged { get; set; }
            public object partenzaReale { get; set; }
            public long? arrivoReale { get; set; }
            public int ritardoPartenza { get; set; }
            public int ritardoArrivo { get; set; }
            public int progressivo { get; set; }
            public object binarioEffettivoArrivoCodice { get; set; }
            public string binarioEffettivoArrivoTipo { get; set; }
            public string binarioEffettivoArrivoDescrizione { get; set; }
            public object binarioProgrammatoArrivoCodice { get; set; }
            public string binarioProgrammatoArrivoDescrizione { get; set; }
            public object binarioEffettivoPartenzaCodice { get; set; }
            public string binarioEffettivoPartenzaTipo { get; set; }
            public string binarioEffettivoPartenzaDescrizione { get; set; }
            public object binarioProgrammatoPartenzaCodice { get; set; }
            public string binarioProgrammatoPartenzaDescrizione { get; set; }
            public string tipoFermata { get; set; }
            public bool visualizzaPrevista { get; set; }
            public bool nextChanged { get; set; }
            public int nextTrattaType { get; set; }
            public int actualFermataType { get; set; }
        }


        public string tipoTreno { get; set; }
        public object orientamento { get; set; }
        public int codiceCliente { get; set; }
        public object fermateSoppresse { get; set; }
        public object dataPartenza { get; set; }
        public List<Fermate> fermate { get; set; }
        public object anormalita { get; set; }
        public object provvedimenti { get; set; }
        public object segnalazioni { get; set; }
        public long? oraUltimoRilevamento { get; set; }
        public string stazioneUltimoRilevamento { get; set; }
        public string idDestinazione { get; set; }
        public string idOrigine { get; set; }
        public List<object> cambiNumero { get; set; }
        public bool hasProvvedimenti { get; set; }
        public List<string> descOrientamento { get; set; }
        public string compOraUltimoRilevamento { get; set; }
        public object motivoRitardoPrevalente { get; set; }
        public string descrizioneVCO { get; set; }
        public int numeroTreno { get; set; }
        public string categoria { get; set; }
        public object categoriaDescrizione { get; set; }
        public string origine { get; set; }
        public object codOrigine { get; set; }
        public string destinazione { get; set; }
        public object codDestinazione { get; set; }
        public object origineEstera { get; set; }
        public object destinazioneEstera { get; set; }
        public object oraPartenzaEstera { get; set; }
        public object oraArrivoEstera { get; set; }
        public int tratta { get; set; }
        public int regione { get; set; }
        public string origineZero { get; set; }
        public string destinazioneZero { get; set; }
        [JsonIgnore]
        public long orarioPartenzaZero { get; set; }
        [JsonIgnore]
        public long orarioArrivoZero { get; set; }
        public bool circolante { get; set; }
        public object binarioEffettivoArrivoCodice { get; set; }
        public object binarioEffettivoArrivoDescrizione { get; set; }
        public object binarioEffettivoArrivoTipo { get; set; }
        public object binarioProgrammatoArrivoCodice { get; set; }
        public object binarioProgrammatoArrivoDescrizione { get; set; }
        public object binarioEffettivoPartenzaCodice { get; set; }
        public object binarioEffettivoPartenzaDescrizione { get; set; }
        public object binarioEffettivoPartenzaTipo { get; set; }
        public object binarioProgrammatoPartenzaCodice { get; set; }
        public object binarioProgrammatoPartenzaDescrizione { get; set; }
        public string subTitle { get; set; }
        public string esisteCorsaZero { get; set; }
        public bool inStazione { get; set; }
        public bool haCambiNumero { get; set; }
        public bool nonPartito { get; set; }
        public int provvedimento { get; set; }
        public object riprogrammazione { get; set; }
        public long orarioPartenza { get; set; }
        public long orarioArrivo { get; set; }
        public object stazionePartenza { get; set; }
        public object stazioneArrivo { get; set; }
        public object statoTreno { get; set; }
        public object corrispondenze { get; set; }
        public List<object> servizi { get; set; }
        public int ritardo { get; set; }
        public string tipoProdotto { get; set; }
        public string compOrarioPartenzaZeroEffettivo { get; set; }
        public string compOrarioArrivoZeroEffettivo { get; set; }
        public string compOrarioPartenzaZero { get; set; }
        public string compOrarioArrivoZero { get; set; }
        public string compOrarioArrivo { get; set; }
        public string compOrarioPartenza { get; set; }
        public string compNumeroTreno { get; set; }
        public List<string> compOrientamento { get; set; }
        public string compTipologiaTreno { get; set; }
        public string compClassRitardoTxt { get; set; }
        public string compClassRitardoLine { get; set; }
        public string compImgRitardo2 { get; set; }
        public string compImgRitardo { get; set; }
        public List<string> compRitardo { get; set; }
        public List<string> compRitardoAndamento { get; set; }
        public List<string> compInStazionePartenza { get; set; }
        public List<string> compInStazioneArrivo { get; set; }
        public string compOrarioEffettivoArrivo { get; set; }
        public string compDurata { get; set; }
        public string compImgCambiNumerazione { get; set; }
    }
    
}
