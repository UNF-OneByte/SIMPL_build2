using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SIMPL.Models;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace SIMPL.Areas.Identity.Pages.Account.Manage
{
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<AspNetUsers, IdentityRole>
    {
        public AppClaimsPrincipalFactory(
            UserManager<AspNetUsers> userManager
            , RoleManager<IdentityRole> roleManager
            , IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
        { }

        public async override Task<ClaimsPrincipal> CreateAsync(AspNetUsers user)
        {
            var principal = await base.CreateAsync(user);

            if (!string.IsNullOrWhiteSpace(user.FirstName))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
        new Claim(ClaimTypes.GivenName, user.FirstName)
    });
            }

            if (!string.IsNullOrWhiteSpace(user.LastName))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
         new Claim(ClaimTypes.Surname, user.LastName),
    });
            }

            return principal;
        }
    }
}
