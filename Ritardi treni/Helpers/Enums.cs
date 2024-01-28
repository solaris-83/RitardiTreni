using System;

namespace Ritardi_treni.Helpers
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

    public enum TratteEnum
    {
        [Display("BIELLA - NOVARA")]
        BIELLA_NOVARA,
        [Display("BIELLA - SANTHIA'")]
        BIELLA_SANTHIA,
        [Display("TORINO - MILANO")]
        TORINO_MILANO
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
