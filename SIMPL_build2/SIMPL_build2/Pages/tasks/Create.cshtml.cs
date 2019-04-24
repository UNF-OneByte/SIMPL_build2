using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIMPL.Models;

namespace SIMPL.Pages.tasks
{
    public class CreateModel : PageModel
    {
        private readonly SIMPL.Models.project_trackerContext _context;        

        public CreateModel(SIMPL.Models.project_trackerContext context)
        {
            _context = context;
        }

        public IList<Tasks> TheTask { get; set; }

        public IActionResult OnGet()
        {     
        ViewData["CostTypeId"] = new SelectList(_context.CostTypes, "CostTypeId", "Name");
        ViewData["CreatedById"] = new SelectList(_context.AspNetUsers, "Id", "UserName");
        ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "Name");
        ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectName");
        ViewData["VendorId"] = new SelectList(_context.Vendors, "VendorId", "Name");

            TheTask = _context.Tasks
                 .Include(p => p.TaskId).ToList();

            return Page();
        }

        [BindProperty]
        public Tasks Tasks { get; set; }


        [BindProperty(SupportsGet = true)]
        public string QueryProjectId { get; set; }

        /*public async Task OnGetAsync()
        {
            Tasks = await _context.Tasks
            Include(t => t.ProjectId).ToListAsync();

            if (QueryProjectId != null)
            {
                if (int.TryParse(QueryProjectId, out var ParsedProjectId))
                {
                    Tasks = Tasks.Where(i => i.ProjectId == ParsedProjectId).ToList();
                }
            }
        }*/


       public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }        
            _context.Tasks.Add(Tasks);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}