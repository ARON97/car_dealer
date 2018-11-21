using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cars.Core.Models
{
    public class Make
    {
        // Make properties
        public int Id { get; set; }
        [Required] // makes the column not nullable
        [StringLength(255)] // setting the character to 255 characters
        public string Name { get; set; }

        // Collection of models
        public ICollection<Model> Models { get; set; }

        // we do not want to repeat make.Models = new Collection<Model>(); everywhere
        // when you have a collection property in a class you should initialize it in the constructor of that class
        public Make()
        {
            // Initialize models to a new collection
            Models = new System.Collections.ObjectModel.Collection<Model>();
        }
    }
}