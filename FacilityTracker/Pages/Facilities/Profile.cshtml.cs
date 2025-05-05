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

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public Facility Facility { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Facility = await _context.Facilities
                .Include(f => f.Users)
                .FirstOrDefaultAsync(f => f.Id == Id);

            if (Facility == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var facility = await _context.Facilities.FindAsync(Id);
            if (facility != null)
            {
                _context.Facilities.Remove(facility);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Home");
        }
    }
}