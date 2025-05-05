using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using FacilityTracker.Models;
using System.Security.Claims;
using System.Threading.Tasks;

public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<User>
{
    public ApplicationUserClaimsPrincipalFactory(
        UserManager<User> userManager,
        IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, optionsAccessor)
    { }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
    {
        var identity = await base.GenerateClaimsAsync(user);
        identity.AddClaim(new Claim("role", user.Role.ToString()));
        identity.AddClaim(new Claim("FirstName", user.FirstName ?? ""));
        identity.AddClaim(new Claim("LastName", user.LastName ?? ""));
        return identity;
    }
}