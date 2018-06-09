using System.ComponentModel.DataAnnotations;
using PersonalPlace.Domain.Common.Properties;

namespace PersonalPlace.Domain.Common.ValueObjects
{
    public enum UserStateType
    {
        [Display(Description = "UserStateType_Created", ResourceType = typeof(Resources))]
        Created = 1,
        [Display(Description = "UserStateType_Active", ResourceType = typeof(Resources))]
        Active = 2,
        [Display(Description = "UserStateType_Blocked", ResourceType = typeof(Resources))]
        Blocked = 3,
        [Display(Description = "UserStateType_Inactive", ResourceType = typeof(Resources))]
        Inactive = 4
    }
}
