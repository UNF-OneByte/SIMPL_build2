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

        public IList<Projects> Projects { get; set; }
        public IList<Tasks> Tasks { get; set; }

        //public IList<Projects> OldestProject { get; set; }
        public Projects OldestProject { get; set; }

        public IList<Projects> ClosedProjects { get; set; }
        public Projects SingleProject { get; set; }
        public IList<ProjectManagerCountDto> ProjectManagerCount { get; set; }

        public async Task OnGetAsync()
        {
            Projects = await _context.Projects
                .Include(p => p.ProjectManager)
                .ToListAsync();

            Tasks = await _context.Tasks.ToListAsync();

            //Example from mentor session
            ClosedProjects = Projects.Where(p => !p.ProjectId.Equals(1)).ToList();
            SingleProject = Projects.Where(p => p.ProjectId.Equals(1)).FirstOrDefault();

            //How many projects one user is assigned
            ProjectManagerCount = Projects.GroupBy(p => p.ProjectManager.UserName)
                .Select(group => new ProjectManagerCountDto { UserName = group.Key, ProjectCount = group.Count() })
                .ToList();

            //Get the oldest project
            DateTime OldestDate = new DateTime(1776, 01, 01, 12, 00, 00);
            OldestProject = Projects.Where(p => p.ActualStartDate > OldestDate)
                                    .OrderBy(p => p.ActualStartDate).First();                                    

        }
        public class ProjectManagerCountDto
        {
            public string UserName { get; set; }
            public int ProjectCount { get; set; }
            public int Date { get; set; }
        }
    }
}
