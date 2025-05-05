using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using FacilityTracker.Data;
using FacilityTracker.Models;

namespace FacilityTracker.Pages
{
    [Authorize(Roles = "StaffMember")]
    public class UsersModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public UsersModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<User> AllUsers { get; set; } = new();

        public void OnGet()
        {
            AllUsers = _context.Users.ToList();
        }
    }
}