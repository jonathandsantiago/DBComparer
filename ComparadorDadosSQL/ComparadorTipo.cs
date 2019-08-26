using System.ComponentModel.DataAnnotations;

namespace ComparadorDadosSQL
{
    public enum ComparadorTipo
    {
        [Display(Name = "Diferente")]
        Diferente,
        [Display(Name = "Iguais")]
        Igual,
        [Display(Name = "Merge")]
        Merge
    }
}
