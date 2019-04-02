using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SIMPL.Models;

namespace SIMPL.Pages
{
    public class AdminModel : PageModel
    {
        private readonly SIMPL.Models.project_trackerContext _context;

        public AdminModel(SIMPL.Models.project_trackerContext context)
        {
            _context = context;
        }

        public IList<AspNetUsers> AspNetUsers { get;set; }

        public async Task OnGetAsync()
        {
            AspNetUsers = await _context.AspNetUsers.ToListAsync();
        }
    }
}
