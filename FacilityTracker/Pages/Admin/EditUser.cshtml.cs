using FacilityTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace FacilityTracker.Pages.Admin
{
    [Authorize]
    public class EditUserModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public EditUserModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string UserId { get; set; }
        public SelectList Roles { get; set; }

        public class InputModel
        {
            [Required]
            public string FirstName { get; set; }
            [Required]
            public string LastName { get; set; }
            [Required, EmailAddress]
            public string Email { get; set; }
            [Required]
            public UserType Role { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User.Claims.Any(c => c.Type == "role" && c.Value == "Admin");

            if (currentUserId != user.Id && !isAdmin)
                return Forbid();

            UserId = user.Id;
            Input = new InputModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role
            };

            Roles = new SelectList(Enum.GetValues(typeof(UserType)));
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var currentUser = await _userManager.GetUserAsync(User); // NEW
            var isAdmin = currentUser.Role == UserType.Admin;        // CHANGED
            var isSelf = currentUser.Id == user.Id;                  // NEW

            if (!isAdmin && !isSelf) // CHANGED
                return Forbid();

            if (!ModelState.IsValid)
            {
                Roles = new SelectList(Enum.GetValues(typeof(UserType)));
                return Page();
            }

            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.Email = Input.Email;
            user.UserName = Input.Email;

            if (isAdmin)
            {
                user.Role = Input.Role;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                Roles = new SelectList(Enum.GetValues(typeof(UserType)));
                return Page();
            }

            // ✅ Conditional Redirect:
            if (isAdmin)
                return RedirectToPage("Dashboard");
            else
                return RedirectToPage("UserProfile", new { id = user.Id });
        }
    }
}
