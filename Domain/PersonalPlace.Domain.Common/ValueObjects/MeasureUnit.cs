using System.ComponentModel.DataAnnotations;
using PersonalPlace.Domain.Common.Properties;

namespace PersonalPlace.Domain.Common.ValueObjects
{
    public enum MeasureUnit
    {
        [Display(Description = "MeasureUnit_Acres", ResourceType = typeof(Resources))]
        Acres = 1,
        [Display(Description = "MeasureUnit_Hectares", ResourceType = typeof(Resources))]
        Hectares = 2,
        [Display(Description = "MeasureUnit_SquareFeet", ResourceType = typeof(Resources))]
        SquareFeet = 3,
        [Display(Description = "MeasureUnit_SquareKilometers", ResourceType = typeof(Resources))]
        SquareKilometers = 4,
        [Display(Description = "MeasureUnit_SquareMeters", ResourceType = typeof(Resources))]
        SquareMeters = 5,
        [Display(Description = "MeasureUnit_SquareMiles", ResourceType = typeof(Resources))]
        SquareMiles = 6,
        [Display(Description = "MeasureUnit_SquareYards", ResourceType = typeof(Resources))]
        SquareYards = 1
    }
}