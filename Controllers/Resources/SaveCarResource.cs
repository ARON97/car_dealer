using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace cars.Controllers.Resources
{
    public class SaveCarResource
    {
        // properties
        public int Id { get; set; }
        // our car is going to have a model so we add the foreign key property
        public int ModelId { get; set; }
        // Car registration
        public bool IsRegistered { get; set; }
        // Contact class property
        [Required]
        public ContactResource Contact { get; set; }
        public ICollection<int> Features { get; set; }

        // class constructor
        public SaveCarResource()
        {
            // set features to a new collection of integers
            Features = new Collection<int>();
        }
    }
}