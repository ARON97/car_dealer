using System.ComponentModel.DataAnnotations;

namespace cars.Core.Models
{
    public class Photo
    {
        public int Id { get; set; }
        // Data Annotations to the FileName
        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        public int CarId { get; set; }
    }
}