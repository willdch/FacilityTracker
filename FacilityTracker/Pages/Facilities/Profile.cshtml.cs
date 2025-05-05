using FacilityTracker.Data;
using FacilityTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FacilityTracker.Pages.Facilities
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ProfileModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Facility Facility { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Facility = await _context.Facilities
                .Include(f => f.Users)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (Facility == null)
                return NotFound();

            return Page();
        }
    }
}