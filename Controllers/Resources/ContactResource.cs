using System.ComponentModel.DataAnnotations;

namespace cars.Controllers.Resources
{
    public class ContactResource
    {
        // DataAnnotations on the Contact Name
        [Required]
        [StringLength(255)]
        // Contact details
        public string Name { get; set; }
        [StringLength(255)]
        public string Email { get; set; }
        // DataAnnotations on the Contact Phone
        [Required]
        [StringLength(255)]
        public string Phone { get; set; }
    }
}