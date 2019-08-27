using System.ComponentModel.DataAnnotations;

namespace DBComparer
{
    public enum ComparatorType
    {
        [Display(Name = "Different")]
        Different,
        [Display(Name = "Equals")]
        Equals,
        [Display(Name = "Merge")]
        Merge
    }
}
