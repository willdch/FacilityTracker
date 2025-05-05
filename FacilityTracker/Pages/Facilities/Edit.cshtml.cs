using FacilityTracker.Data;
using FacilityTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FacilityTracker.Pages.Facilities
{
    [Authorize(Policy = "AdminOrManager")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Facility Facility { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Facility = await _context.Facilities.FindAsync(id);

            if (Facility == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
                return Page();

            var facilityToUpdate = await _context.Facilities.FindAsync(id);

            if (facilityToUpdate == null)
                return NotFound();

            facilityToUpdate.Name = Facility.Name;
            facilityToUpdate.Location = Facility.Location;

            await _context.SaveChangesAsync();

            return RedirectToPage("Profile", new { id = id });
        }


    }
}