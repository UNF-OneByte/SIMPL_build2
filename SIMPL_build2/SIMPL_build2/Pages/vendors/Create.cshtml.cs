using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIMPL.Models;

namespace SIMPL.Pages.vendors
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
        public Vendors Vendors { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Vendors.Add(Vendors);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}