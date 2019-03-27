using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIMPL.Models;

namespace SIMPL.Pages.projects
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
            ViewData["ProjectManagerId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            //ViewData["ProjectManagerId"] = new SelectList(_context.AspNetUsers, "UserName", "UserName"); Make this work
            return Page();
        }

        [BindProperty]
        public Projects Projects { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Projects.Add(Projects);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}