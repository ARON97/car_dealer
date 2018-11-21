using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace cars.Controllers.Resources
{
    public class MakeResource : KeyValuePairResource
    {
         // Make properties
        // public int Id { get; set; }
        // public string name { get; set; }

        // Collection of models
        public ICollection<KeyValuePairResource> Models { get; set; }

        // we do not want to repeat make.Models = new Collection<Model>(); everywhere
        // when you have a collection property in a class you should initialize it in the constructor of that class
        public MakeResource()
        {
            // Initialize models to a new collection
            Models = new Collection<KeyValuePairResource>();
        }
    }
}