using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIMPL.Models;

namespace SIMPL.Pages.costTypes
{
    public class EditModel : PageModel
    {
        private readonly SIMPL.Models.project_trackerContext _context;

        public EditModel(SIMPL.Models.project_trackerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CostTypes CostTypes { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CostTypes = await _context.CostTypes.FirstOrDefaultAsync(m => m.CostTypeId == id);

            if (CostTypes == null)
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

            _context.Attach(CostTypes).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CostTypesExists(CostTypes.CostTypeId))
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

        private bool CostTypesExists(int id)
        {
            return _context.CostTypes.Any(e => e.CostTypeId == id);
        }
    }
}
