using System.ComponentModel.DataAnnotations;
using PersonalPlace.Domain.Common.Properties;

namespace PersonalPlace.Domain.Common.ValueObjects
{
    public enum UnitStateType
    {
        [Display(Description = "UnitStateType_Active", ResourceType = typeof(Resources))]
        Active = 1,
        [Display(Description = "UnitStateType_Inactive", ResourceType = typeof(Resources))]
        Inactive = 2
    }
}