using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SIMPL.Models;

namespace SIMPL.Pages.reports
{
    public class indexModel : PageModel
    {
        private readonly SIMPL.Models.project_trackerContext _context;

        public indexModel(SIMPL.Models.project_trackerContext context)
        {
            _context = context;            
        }

        public IList<Projects> Projects { get;set; }
        public IList<Tasks> Tasks { get; set; }       

        public async Task OnGetAsync()
        {
            Projects = await _context.Projects
                .Include(p => p.ProjectManager).ToListAsync();   
            
        }     
    }
}
