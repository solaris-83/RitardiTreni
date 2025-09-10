using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TrackMyTrain.Data.Implementations
{
    public record Fermata(
            string Id,
            string Stazione,
            string TipoFermata,
            int? RitardoArrivo,
            int? RitardoPartenza,
            int? Ritardo,
            long? PartenzaReale,
            long? ArrivoReale,
            long? Programmata,
            long? ProgrammataZero,
            int ActualFermataType
        );

    public record TrainJourney(
            string IdOrigine,
            string IdDestinazione,
            string Origine,
            string Destinazione,
            long? OrarioPartenza,
            long? OrarioArrivo,
            string CompOrarioPartenza,
            string CompOrarioArrivo,
            List<string> CompRitardo,
            List<string> CompRitardoAndamento,
            long? OraUltimoRilevamento,
            string StazioneUltimoRilevamento,
            string TipoTreno,
            int Provvedimento,
            string SubTitle,
            int NumeroTreno,
            string CompNumeroTreno,
            string Categoria,
            int? Ritardo,
            string compOraUltimoRilevamento,
            List<Fermata> Fermate
    )
    {
        public TrainJourney() : this(   
            default!, default!, default!, default!, default, default, default!, default!, default!, default!,
            default, default!, default!, default!, default!, default!, default!, default!, default!, default!, new List<Fermata>())
        {
        }

        public bool HasWarning()
        {
            if (TipoTreno == "PG")
                return false;
            if (TipoTreno == "ST" && Provvedimento == 1)
                return true;
            if ((TipoTreno == "PP" || TipoTreno == "SI" || TipoTreno == "SF") && (Provvedimento == 0 || Provvedimento == 2))
                return true;
            if (TipoTreno == "DV" && Provvedimento == 3)
                return true;
            throw new NotImplementedException($"TipoTreno {TipoTreno} and Provvedimento {Provvedimento} not recognized");
        }
    }
}
