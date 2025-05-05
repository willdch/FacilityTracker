using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using FacilityTracker.Data;
using FacilityTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FacilityTracker.Pages
{
    [Authorize]
    public class HomeModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeModel(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserType Role { get; set; }
        public List<Facility> Facilities { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Challenge(); 

            FirstName = user.FirstName;
            LastName = user.LastName;
            Role = user.Role;
            
            Facilities = await _context.Facilities
                .Include(f => f.Users)
                .ToListAsync();

            return Page();
        }
    }
}