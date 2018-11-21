using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace cars.Controllers.Resources
{
    public class CarResource
    {
        // properties
        public int Id { get; set; }
        // // our car is going to have a model so we add the foreign key property
        // public int ModelId { get; set; }
        // navigation property
        public KeyValuePairResource Model { get; set; }
        // Make Resource
        public KeyValuePairResource Make { get; set; }
        // Car registration
        public bool IsRegistered { get; set; }
        public ContactResource Contact { get; set; }
       
        // A date and time of when the class was updated last
        public DateTime LastUpdate { get; set; }

        public ICollection<KeyValuePairResource> Features { get; set; }

        // Initializing the Features in a constructor
        public CarResource()
        {
            // setting Features to a new collection of CarFeatures
            Features = new Collection<KeyValuePairResource>();
        }
    }
}