using System.ComponentModel.DataAnnotations;
using PersonalPlace.Domain.Common.Properties;

namespace PersonalPlace.Domain.Common.ValueObjects
{
    public enum CommentsStateType
    {
        [Display(Description = "CommentsStateType_Created", ResourceType = typeof(Resources))]
        Created = 1,
        [Display(Description = "CommentsStateType_Accepted", ResourceType = typeof(Resources))]
        Accepted = 2,
        [Display(Description = "CommentsStateType_Rejected", ResourceType = typeof(Resources))]
        Rejected = 3
    }
}