using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIMPL.Models;

namespace SIMPL.Pages.UserRoleAdmin
{
    public class CreateModel : PageModel
    {
        private readonly SIMPL.Models.project_trackerContext _context;

        public CreateModel(SIMPL.Models.project_trackerContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["RoleId"] = new SelectList(_context.AspNetRoles, "Id", "Name");
        ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "UserName");
            return Page();
        }

        [BindProperty]
        public AspNetUserRoles AspNetUserRoles { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.AspNetUserRoles.Add(AspNetUserRoles);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}