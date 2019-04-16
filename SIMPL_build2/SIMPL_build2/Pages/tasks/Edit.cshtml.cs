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
    public class EditModel : PageModel
    {
        private readonly SIMPL.Models.project_trackerContext _context;

        public EditModel(SIMPL.Models.project_trackerContext context)
        {
            _context = context;
        }

        [BindProperty]
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
           ViewData["CostTypeId"] = new SelectList(_context.CostTypes, "CostTypeId", "Name");
           ViewData["CreatedById"] = new SelectList(_context.AspNetUsers, "Id", "UserName");
           ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "LocationId");
           ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "ProjectId");
           ViewData["VendorId"] = new SelectList(_context.Vendors, "VendorId", "Name");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Tasks).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TasksExists(Tasks.TaskId))
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

        private bool TasksExists(int id)
        {
            return _context.Tasks.Any(e => e.TaskId == id);
        }
    }
}
