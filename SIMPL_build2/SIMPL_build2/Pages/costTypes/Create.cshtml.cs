using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIMPL.Models;

namespace SIMPL.Pages.costTypes
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
            return Page();
        }

        [BindProperty]
        public CostTypes CostTypes { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.CostTypes.Add(CostTypes);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}