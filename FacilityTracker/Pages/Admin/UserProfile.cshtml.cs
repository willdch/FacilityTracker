using FacilityTracker.Data;
using FacilityTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace FacilityTracker.Pages.Admin
{
    [Authorize]
    public class UserProfileModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserProfileModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public User ViewedUser { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsManager { get; set; }
        public bool IsSelf { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            IsAdmin = currentUser.Role == UserType.Admin;
            IsManager = currentUser.Role == UserType.Manager;
            IsSelf = currentUser.Id == Id;

            if (!IsAdmin && !IsManager && !IsSelf)
            {
                return Forbid();
            }

            ViewedUser = await _context.Users
                .Include(u => u.Facility)
                .FirstOrDefaultAsync(u => u.Id == Id);

            if (ViewedUser == null)
            {
                return NotFound();
            }

            return Page();
        }
        
        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            IsAdmin = currentUser.Role == UserType.Admin;
            IsManager = currentUser.Role == UserType.Manager;

            var userToDelete = await _context.Users.FindAsync(Id);
            if (userToDelete == null) return NotFound();

            bool canDelete =
                (IsAdmin && currentUser.Id != Id) ||
                (IsManager && userToDelete.Role != UserType.Admin && currentUser.Id != Id);

            if (!canDelete)
                return Forbid();

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Admin/Dashboard");
        }


    }
}