using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FacilityTracker.Models
{
    public class Facility
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Location { get; set; }

        // Navigation properties
        public List<User> Users { get; set; } = new();

    }
}