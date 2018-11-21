using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cars.Core.Models
{
    [Table("Models")] // applying the table attribute on the class itself to change the correspondding name
    public class Model
    {
        // Model properties
        public int Id { get; set; }
        [Required] // makes the column not nullable
        [StringLength(255)] // setting the character to 255 characters
        public string Name { get; set; }

        // Inverse property- Navigation property
        public Make Make { get; set; }

        // Foreign key property
        public int MakeId { get; set; }

    }
}