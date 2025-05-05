using FacilityTracker.Data;
using FacilityTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FacilityTracker.Pages.Facilities
{
    [Authorize(Policy = "AdminOrManager")]
    public class AssignStaffModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AssignStaffModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int FacilityId { get; set; }

        [BindProperty]
        public string UserId { get; set; }

        public SelectList FacilityList { get; set; }
        public SelectList UserList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            FacilityList = new SelectList(
                await _context.Facilities.ToListAsync(),
                "Id",
                "Name");

            var users = await _context.Users
                .Where(u => u.Role != UserType.Admin)
                .ToListAsync();

            var userItems = users.Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = $"{u.FirstName} {u.LastName}"
            }).ToList();

            UserList = new SelectList(userItems, "Value", "Text");

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(); // Rebuild dropdowns
                return Page();
            }

            var user = await _context.Users.FindAsync(UserId);
            if (user == null) return NotFound();

            user.FacilityId = FacilityId;
            await _context.SaveChangesAsync();

            return RedirectToPage("/Home");
        }
    }
}