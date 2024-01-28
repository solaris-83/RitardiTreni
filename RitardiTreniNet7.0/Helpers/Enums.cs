using System;

namespace RitardiTreniNet7._0.Helpers
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class DisplayAttribute : Attribute
    {
        public DisplayAttribute(string displayName)
        {
            Description = displayName;
        }

        public string Description { get; set; }
    }

    public enum StazioniEnum
    {
        [Display("BIELLA S.PAOLO")]
        BIELLA,
        [Display("CARPIGNANO SESIA")]
        CARPIGNANO,
        [Display("CHIVASSO")]
        CHIVASSO,
        [Display("COSSATO")]
        COSSATO,
        [Display("MAGENTA")]
        MAGENTA, 
        [Display("MILANO CENTRALE")]
        MILANO_CENTRALE,
        [Display("MILANO PORTA GARIBALDI")]
        MILANO_PG,
        [Display("NOVARA")]
        NOVARA,
        [Display("RHO FIERA MILANO")]
        RHO_FIERA,
        [Display("ROVASENDA")]
        ROVASENDA,
        [Display("SALUSSOLA")]
        SALUSSOLA,
        [Display("SANTHIA`")]
        SANTHIA,
        [Display("TORINO P.NUOVA")]
        TORINO_PN,
        [Display("TORINO PORTA SUSA")]
        TORINO_PS,
        [Display("VERCELLI")]
        VERCELLI,
    }
}
