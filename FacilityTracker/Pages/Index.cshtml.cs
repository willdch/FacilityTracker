using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using FacilityTracker.Data;
using FacilityTracker.Models;
using Microsoft.AspNetCore.Authorization;

namespace FacilityTracker.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty] public string FirstName { get; set; } = string.Empty;

        [BindProperty] public string LastName { get; set; } = string.Empty;

        public void OnGet()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user != null)
            {
                FirstName = user.FirstName;
            }
        }

    }
}