using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using FacilityTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace FacilityTracker.Pages
{
    [Authorize]
    public class HomeModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public HomeModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public string FirstName { get; set; }
        public UserType Role { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            FirstName = user.FirstName;
            Role = user.Role;

            return Page();
        }
    }
}