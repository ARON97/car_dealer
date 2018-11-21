using System.ComponentModel.DataAnnotations.Schema;

namespace cars.Core.Models
{
    // DataAnnotation to pluralize CarFeature
    [Table("CarFeatures")]
    public class CarFeature
    {
        // Foreign key properties
        public int CarId { get; set; }
        public int FeatureId { get; set; }

        // Navigation properties
        public Car Car { get; set; }
        public Feature Feature { get; set; }
    }
}