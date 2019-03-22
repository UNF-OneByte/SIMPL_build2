using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SIMPL.Models;

namespace SIMPL.Pages.tasks
{
    public class DetailsModel : PageModel
    {
        private readonly SIMPL.Models.project_trackerContext _context;

        public DetailsModel(SIMPL.Models.project_trackerContext context)
        {
            _context = context;
        }

        public Tasks Tasks { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Tasks = await _context.Tasks
                .Include(t => t.CostType)
                .Include(t => t.CreatedBy)
                .Include(t => t.Location)
                .Include(t => t.Project)
                .Include(t => t.Vendor).FirstOrDefaultAsync(m => m.TaskId == id);

            if (Tasks == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
