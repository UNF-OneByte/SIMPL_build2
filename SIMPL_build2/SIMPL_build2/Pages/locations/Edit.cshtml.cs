using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIMPL.Models;

namespace SIMPL.Pages.locations
{
    public class EditModel : PageModel
    {
        private readonly SIMPL.Models.project_trackerContext _context;

        public EditModel(SIMPL.Models.project_trackerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Locations Locations { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Locations = await _context.Locations.FirstOrDefaultAsync(m => m.LocationId == id);

            if (Locations == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Locations).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationsExists(Locations.LocationId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool LocationsExists(int id)
        {
            return _context.Locations.Any(e => e.LocationId == id);
        }
    }
}
