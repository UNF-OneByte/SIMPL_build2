using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SIMPL.Models;

namespace SIMPL.Pages.RoleAdmin
{
    public class DeleteModel : PageModel
    {
        private readonly SIMPL.Models.project_trackerContext _context;

        public DeleteModel(SIMPL.Models.project_trackerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AspNetRoles AspNetRoles { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AspNetRoles = await _context.AspNetRoles.FirstOrDefaultAsync(m => m.Id == id);

            if (AspNetRoles == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            AspNetRoles = await _context.AspNetRoles.FindAsync(id);

            if (AspNetRoles != null)
            {
                _context.AspNetRoles.Remove(AspNetRoles);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
