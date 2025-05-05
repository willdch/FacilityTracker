using FacilityTracker.Data;
using FacilityTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        [BindProperty]
        public string UserId { get; set; }
        public SelectList UnassignedStaff { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Facility = await _context.Facilities
                .Include(f => f.Users)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (Facility == null) return NotFound();

            var assignableUsers = await _context.Users
                .Where(u => u.Role != UserType.Admin)
                .ToListAsync();

            UnassignedStaff = new SelectList(assignableUsers, "Id", "FullName");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var facility = await _context.Facilities
                .Include(f => f.Users)
                .FirstOrDefaultAsync(f => f.Id == Id);

            if (facility == null) return NotFound();

            var user = await _context.Users.FindAsync(UserId);
            if (user == null) return NotFound();

            user.FacilityId = facility.Id;
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id = facility.Id });
        }
    }
}