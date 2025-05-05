using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FacilityTracker.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        [Required]
        public UserType Role { get; set; }
        
        public Facility? Facility { get; set; }
        public int? FacilityId { get; set; }

    }
}