export interface andamentoTreno {
  tipoTreno: string;
  orientamento?: null;
  codiceCliente: number;
  fermateSoppresse?: null;
  dataPartenza?: null;
  fermate?: (FermateEntity)[] | null;
  anormalita?: null;
  provvedimenti?: null;
  segnalazioni?: null;
  oraUltimoRilevamento: number;
  stazioneUltimoRilevamento: string;
  idDestinazione: string;
  idOrigine: string;
  cambiNumero?: (null)[] | null;
  hasProvvedimenti: boolean;
  descOrientamento?: (string)[] | null;
  compOraUltimoRilevamento: string;
  motivoRitardoPrevalente?: null;
  descrizioneVCO: string;
  materiale_label?: null;
  numeroTreno: number;
  categoria: string;
  categoriaDescrizione?: null;
  origine: string;
  codOrigine?: null;
  destinazione: string;
  codDestinazione?: null;
  origineEstera?: null;
  destinazioneEstera?: null;
  oraPartenzaEstera?: null;
  oraArrivoEstera?: null;
  tratta: number;
  regione: number;
  origineZero: string;
  destinazioneZero: string;
  orarioPartenzaZero: number;
  orarioArrivoZero: number;
  circolante: boolean;
  binarioEffettivoArrivoCodice?: null;
  binarioEffettivoArrivoDescrizione?: null;
  binarioEffettivoArrivoTipo?: null;
  binarioProgrammatoArrivoCodice?: null;
  binarioProgrammatoArrivoDescrizione?: null;
  binarioEffettivoPartenzaCodice?: null;
  binarioEffettivoPartenzaDescrizione?: null;
  binarioEffettivoPartenzaTipo?: null;
  binarioProgrammatoPartenzaCodice?: null;
  binarioProgrammatoPartenzaDescrizione?: null;
  subTitle: string;
  esisteCorsaZero: string;
  inStazione: boolean;
  haCambiNumero: boolean;
  nonPartito: boolean;
  provvedimento: number;
  riprogrammazione?: null;
  orarioPartenza: number;
  orarioArrivo: number;
  stazionePartenza?: null;
  stazioneArrivo?: null;
  statoTreno?: null;
  corrispondenze?: null;
  servizi?: (null)[] | null;
  ritardo: number;
  tipoProdotto: string;
  compOrarioPartenzaZeroEffettivo: string;
  compOrarioArrivoZeroEffettivo: string;
  compOrarioPartenzaZero: string;
  compOrarioArrivoZero: string;
  compOrarioArrivo: string;
  compOrarioPartenza: string;
  compNumeroTreno: string;
  compOrientamento?: (string)[] | null;
  compTipologiaTreno: string;
  compClassRitardoTxt: string;
  compClassRitardoLine: string;
  compImgRitardo2: string;
  compImgRitardo: string;
  compRitardo?: (string)[] | null;
  compRitardoAndamento?: (string)[] | null;
  compInStazionePartenza?: (string)[] | null;
  compInStazioneArrivo?: (string)[] | null;
  compOrarioEffettivoArrivo: string;
  compDurata: string;
  compImgCambiNumerazione: string;
  dataPartenzaTreno: number;
}
export interface FermateEntity {
  orientamento?: null;
  kcNumTreno?: null;
  stazione: string;
  id: string;
  listaCorrispondenze?: null;
  programmata: number;
  programmataZero?: null;
  effettiva: number;
  ritardo: number;
  partenzaTeoricaZero?: null;
  arrivoTeoricoZero?: null;
  partenza_teorica?: number | null;
  arrivo_teorico?: number | null;
  isNextChanged: boolean;
  partenzaReale?: number | null;
  arrivoReale?: number | null;
  ritardoPartenza: number;
  ritardoArrivo: number;
  progressivo: number;
  binarioEffettivoArrivoCodice?: string | null;
  binarioEffettivoArrivoTipo?: string | null;
  binarioEffettivoArrivoDescrizione?: string | null;
  binarioProgrammatoArrivoCodice?: null;
  binarioProgrammatoArrivoDescrizione?: string | null;
  binarioEffettivoPartenzaCodice?: string | null;
  binarioEffettivoPartenzaTipo?: string | null;
  binarioEffettivoPartenzaDescrizione?: string | null;
  binarioProgrammatoPartenzaCodice?: null;
  binarioProgrammatoPartenzaDescrizione?: string | null;
  tipoFermata: string;
  visualizzaPrevista: boolean;
  nextChanged: boolean;
  nextTrattaType: number;
  actualFermataType: number;
  materiale_label?: null;
}