using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SIMPL.Models;
using Microsoft.AspNetCore.Identity;

namespace SIMPL.Pages
{
    public class AdminModel : PageModel
    {
        private readonly SIMPL.Models.project_trackerContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminModel(SIMPL.Models.project_trackerContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<AspNetUsers> AspNetUsers { get; set; }

        public async Task OnGetAsync()
        {
            AspNetUsers = await _context.AspNetUsers.ToListAsync();
        }

        public async Task Lockout(string user)
        {

            
            //AspNetUsers userObj = await _userManager.FindByIdAsync(user);
            await _userManager.SetLockoutEndDateAsync(await _userManager.FindByIdAsync(user), DateTime.UtcNow.AddMinutes(42));
            //userObj.LockoutEnabled = true;
            //userObj.LockoutEnd = DateTime.UtcNow.AddMinutes(42);
            //await _userManager.UpdateAsync(userObj);
            
        }


    }
}