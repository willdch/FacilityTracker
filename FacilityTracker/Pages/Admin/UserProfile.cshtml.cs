using FacilityTracker.Data;
using FacilityTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FacilityTracker.Pages.Admin
{
    [Authorize(Policy = "Admin")]
    public class UserProfileModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public UserProfileModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public User User { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
                return NotFound();

            User = await _context.Users
                .Include(u => u.Facility)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (User == null)
                return NotFound();

            return Page();
        }
    }
}