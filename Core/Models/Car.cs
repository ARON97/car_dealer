using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cars.Core.Models
{
    // DataAnnotation to pluralize the Car table
    [Table("Cars")]
    public class Car
    {
        // properties
        public int Id { get; set; }
        // our car is going to have a model so we add the foreign key property
        public int ModelId { get; set; }
        // navigation property
        public Model Model { get; set; }
        // Car registration
        public bool IsRegistered { get; set; }
        // DataAnnotations on the Contact Name
        [Required]
        [StringLength(255)]
        // Contact details
        public string ContactName { get; set; }
        [StringLength(255)]
        public string ContactEmail { get; set; }
        // DataAnnotations on the Contact Phone
        [Required]
        [StringLength(255)]
        public string ContactPhone { get; set; }
        // // 
        public DateTime LastUpdate { get; set; }

        public ICollection<CarFeature> Features { get; set; }
        // The Upload Photo collection
        public ICollection<Photo> Photos { get; set; }
        // Initializing the collection properties
        public Car()
        {
            // setting Features to a new collection of CarFeatures
            Features = new Collection<CarFeature>();
            // setting Photos to a new collection of photo
            Photos = new Collection<Photo>();
        }
    }
}