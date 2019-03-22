using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SIMPL.Models;

namespace SIMPL.Pages.costTypes
{
    public class DeleteModel : PageModel
    {
        private readonly SIMPL.Models.project_trackerContext _context;

        public DeleteModel(SIMPL.Models.project_trackerContext context)
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CostTypes = await _context.CostTypes.FindAsync(id);

            if (CostTypes != null)
            {
                _context.CostTypes.Remove(CostTypes);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
