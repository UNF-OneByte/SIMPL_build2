﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SIMPL.Models;

namespace SIMPL.Pages.reports
{
    public class Project_Task_ReportModel : PageModel
    {
        private readonly SIMPL.Models.project_trackerContext _context;

        public Project_Task_ReportModel(SIMPL.Models.project_trackerContext context)
        {
            _context = context;
        }

        //This allows for a query sting named QueryProjectId 
        //?QueryProjectId= <Project ID>
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }


        public IList<Projects> Projects { get; set; }
        public IList<Tasks> Tasks { get;set; }   
        public IList<Tasks> TasksToProjects { get; set; }
        public Task ProjectName { get; set; }

        public async Task OnGetAsync()
        {
            int Project;

            Tasks = await _context.Tasks
                .Include(t => t.CostType)
                .Include(t => t.CreatedBy)
                .Include(t => t.Location)
                .Include(t => t.Project)
                .Include(t => t.Vendor).ToListAsync();

            Projects = await _context.Projects.ToListAsync();

            if (Id != null)
            {
                if (int.TryParse(Id, out var ParsedProjectId))
                {
                    Tasks = Tasks.Where(i => i.ProjectId == ParsedProjectId).ToList();
                    Project = ParsedProjectId;                                    
                }
            }

            //joins Tasks.project.id on project id                                          
            TasksToProjects = Tasks.Join(Projects,
                                    pro => pro.ProjectId,
                                    tas => tas.ProjectId,
                                    (pro, tas) => pro).ToList();
        }
    }
}