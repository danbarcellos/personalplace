using System.ComponentModel.DataAnnotations;
using PersonalPlace.Domain.Common.Properties;

namespace PersonalPlace.Domain.Common.ValueObjects
{
    public enum RealtyStateType
    {
        [Display(Description = "RealtyStateType_Created", ResourceType = typeof(Resources))]
        Created = 1,
        [Display(Description = "RealtyStateType_Analysys", ResourceType = typeof(Resources))]
        Analysys = 2,
        [Display(Description = "RealtyStateType_Published", ResourceType = typeof(Resources))]
        Published = 3
    }
}