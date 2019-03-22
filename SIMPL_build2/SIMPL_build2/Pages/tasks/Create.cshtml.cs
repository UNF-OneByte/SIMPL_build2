using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public IActionResult OnGet()
        {
        ViewData["CostTypeId"] = new SelectList(_context.CostTypes, "CostTypeId", "Name");
        ViewData["CreatedById"] = new SelectList(_context.AspNetUsers, "Id", "Id");
        ViewData["LocationId"] = new SelectList(_context.Locations, "LocationId", "LocationId");
        ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Description");
        ViewData["VendorId"] = new SelectList(_context.Vendors, "VendorId", "Name");
            return Page();
        }

        [BindProperty]
        public Tasks Tasks { get; set; }

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