using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SIMPL.Models;

namespace SIMPL.Pages.vendors
{
    public class DetailsModel : PageModel
    {
        private readonly SIMPL.Models.project_trackerContext _context;

        public DetailsModel(SIMPL.Models.project_trackerContext context)
        {
            _context = context;
        }

        public Vendors Vendors { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Vendors = await _context.Vendors.FirstOrDefaultAsync(m => m.VendorId == id);

            if (Vendors == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
