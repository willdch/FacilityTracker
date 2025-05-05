using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FacilityTracker.Data;
using FacilityTracker.Models;

namespace FacilityTracker.Pages.Admin
{
    [Authorize] // Authenticated users only
    public class DashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DashboardModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<User> Users { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (currentUser == null || currentUser.Role != UserType.Admin)
            {
                return Forbid();
            }

            Users = await _context.Users.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostPromoteToManagerAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null && user.Role != UserType.Manager)
            {
                user.Role = UserType.Manager;
                await _context.SaveChangesAsync();
            }
            Console.WriteLine("Promoted to manager");
            return RedirectToPage();
            
        }
        
        public async Task<IActionResult> OnPostPromoteToAdminAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null && user.Role != UserType.Admin)
            {
                user.Role = UserType.Admin;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
        
        public async Task<IActionResult> OnPostDeleteUserAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDemoteAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null && user.Role != UserType.Staff)
            {
                user.Role = UserType.Staff;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }


    }
}