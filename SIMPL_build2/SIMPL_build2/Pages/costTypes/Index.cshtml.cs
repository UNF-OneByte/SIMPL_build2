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
    public class IndexModel : PageModel
    {
        private readonly SIMPL.Models.project_trackerContext _context;

        public IndexModel(SIMPL.Models.project_trackerContext context)
        {
            _context = context;
        }

        public IList<CostTypes> CostTypes { get;set; }

        public async Task OnGetAsync()
        {
            CostTypes = await _context.CostTypes.ToListAsync();
        }
    }
}
