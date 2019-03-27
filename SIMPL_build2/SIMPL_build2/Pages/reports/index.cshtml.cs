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
        
        public IList<Projects> ClosedProjects { get; set; }
        public Projects SingleProject  { get; set; }

        public async Task OnGetAsync()
        {
            Projects = await _context.Projects
                .Include(p => p.ProjectManager)            
                .ToListAsync();

            Tasks = await _context.Tasks.ToListAsync();

            ClosedProjects = Projects.Where(p => !p.ProjectId.Equals(1)).ToList();
            //var y = Projects.Where(p => !p.ProjectId.Equals(3));
            SingleProject = Projects.Where(p => p.ProjectId.Equals(500)).FirstOrDefault();


        }     
    }
}
