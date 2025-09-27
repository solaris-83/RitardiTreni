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
            bool NonPartito,
            bool Arrivato,
            List<Fermata> Fermate
    )
    {
        public TrainJourney() : this(   
            default!, default!, default!, default!, default, default, default!, default!, default!, default!,default!,default!,
            default, default!, default!, default!, default!, default!, default!, default!, default!, default!, new List<Fermata>())
        {
        }

        public bool HasWarning()
        {
            if (TipoTreno == "PG")
            {
                // Se non è partito dopo 5 minuti dall'orario programmato
                if (NonPartito && OrarioPartenza.HasValue && DateTimeOffset.FromUnixTimeMilliseconds(OrarioPartenza.Value).ToLocalTime() < DateTime.Now.AddMinutes(-5))
                    return true;
                else
                    return false;
            }
            if (TipoTreno == "ST" && Provvedimento == 1)
                return true;
            if ((TipoTreno == "PP" || TipoTreno == "SI" || TipoTreno == "SF") && (Provvedimento == 0 || Provvedimento == 2))
                return true;
            if (TipoTreno == "VO" && Provvedimento == 3) // Parzialmente cancellato
                return true;
            if (TipoTreno == "DV" && Provvedimento == 3)
                return true;
            throw new NotImplementedException($"TipoTreno {TipoTreno} and Provvedimento {Provvedimento} not recognized");
        }

        public string FormatDelay()
        {
            if (NonPartito)
                return string.Empty;
            if (Ritardo == null)
                return string.Empty;
            if (HasWarning() && Ritardo == 0)
                return string.Empty;
            return $"{Ritardo}'";
        }
    }
}
