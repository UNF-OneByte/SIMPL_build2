using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SIMPL.Models;

namespace SIMPL.Pages.projects
{
    public class DetailsModel : PageModel
    {
        private readonly SIMPL.Models.project_trackerContext _context;

        public DetailsModel(SIMPL.Models.project_trackerContext context)
        {
            _context = context;
        }

        public Projects Projects { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Projects = await _context.Projects
                .Include(p => p.ProjectManager).FirstOrDefaultAsync(m => m.ProjectId == id);

            if (Projects == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
