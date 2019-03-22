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
    public class DetailsModel : PageModel
    {
        private readonly SIMPL.Models.project_trackerContext _context;

        public DetailsModel(SIMPL.Models.project_trackerContext context)
        {
            _context = context;
        }

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
    }
}
