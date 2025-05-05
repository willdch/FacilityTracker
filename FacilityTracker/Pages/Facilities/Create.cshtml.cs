using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FacilityTracker.Data;
using FacilityTracker.Models;

namespace FacilityTracker.Pages.Facilities  
{
    [Authorize(Policy = "AdminOrManager")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Facility Facility { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Facilities.Add(Facility);
            await _context.SaveChangesAsync();

            ModelState.Clear();
            Facility = new Facility(); // reset form
            TempData["ShowSuccessToast"] = true;
            return Page();
        }
    }
}
