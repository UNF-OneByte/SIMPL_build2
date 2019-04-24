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
    public class cost_type_breakdownModel : PageModel
    {
        private readonly SIMPL.Models.project_trackerContext _context;

        public cost_type_breakdownModel(SIMPL.Models.project_trackerContext context)
        {
            _context = context;
        }

        //This allows for a query sting named QueryProjectId 
        //?QueryProjectId= <Project ID>
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Status { get; set; }


        public IList<Projects> Projects { get; set; }
        public IList<Tasks> Tasks { get; set; }
        public IList<Tasks> TasksToProjects { get; set; }
        public Task ProjectName { get; set; }
        public IList<VendorCostDto> VendorCost { get; set; }
        public IList<HoursWokredDto> HoursWorked { get; set; }
        public IList<CostTypeDto> CostType { get; set; }

        public async Task OnGetAsync()
        {
            int CostTypeID;
        

            if (Id != null & Status != null)
            {
                Tasks = await _context.Tasks
                    .Include(t => t.CostType)
                    .Include(t => t.CreatedBy)
                    .Include(t => t.Location)
                    .Include(t => t.Project)
                    .Include(t => t.Vendor)
                    .ToListAsync();                

                if (int.TryParse(Id, out var ParsedCostType))
                {
                    Tasks = Tasks.Where(i => i.CostType.CostTypeId == ParsedCostType).ToList();
                    CostTypeID = ParsedCostType;
                }

                if (Status == "open")
                {
                    Tasks = await _context.Tasks
                       .Include(t => t.CostType)
                       .Include(t => t.CreatedBy)
                       .Include(t => t.Location)
                       .Include(t => t.Project)
                       .Include(t => t.Vendor)
                       .Where(t => t.Project.ActualEndDate == null).ToListAsync();

                    Projects = await _context.Projects.ToListAsync();

                    //joins Tasks.project.id on project id                                          
                    TasksToProjects = Tasks.Join(Projects,
                                            pro => pro.ProjectId,
                                            tas => tas.ProjectId,
                                            (pro, tas) => pro).ToList();


                    //How many projects one user is assigned
                    VendorCost = Tasks.Where(t => t.Project.ActualEndDate == null).GroupBy(v => v.Vendor.Name)
                        .Select(group => new VendorCostDto { Vendor = group.Key, Count = group.Count(), Cost = group.Sum(c => c.ActualCost).ToString() })
                        .ToList();

                    //How many hours one task was worked
                    HoursWorked = Tasks.Where(t => t.Project.ActualEndDate == null).GroupBy(v => v.Name)
                        .Select(group => new HoursWokredDto { Task = group.Key, HoursWorked = group.Sum(c => c.ActualHours).ToString() })
                        .ToList();


                    //How many projects one user is assigned
                    CostType = Tasks.Where(t => t.Project.ActualEndDate == null).GroupBy(v => v.CostType.Name.ToString())
                        .Select(group => new CostTypeDto { TheCostType = group.Key, ActSpent = group.Sum(c => c.ActualCost).ToString() })
                        .ToList();
                }
                else if (Status == "closed")
                {
                    Tasks = await _context.Tasks
                       .Include(t => t.CostType)
                       .Include(t => t.CreatedBy)
                       .Include(t => t.Location)
                       .Include(t => t.Project)
                       .Include(t => t.Vendor)
                       .Where(t => t.Project.ActualEndDate != null).ToListAsync();

                    Projects = await _context.Projects.ToListAsync();

                    //joins Tasks.project.id on project id                                          
                    TasksToProjects = Tasks.Join(Projects,
                                            pro => pro.ProjectId,
                                            tas => tas.ProjectId,
                                            (pro, tas) => pro).ToList();

                    //How many projects one user is assigned
                    VendorCost = Tasks.Where(t => t.Project.ActualEndDate != null).GroupBy(v => v.Vendor.Name)
                        .Select(group => new VendorCostDto { Vendor = group.Key, Count = group.Count(), Cost = group.Sum(c => c.ActualCost).ToString() })
                        .ToList();

                    //How many hours one task was worked
                    HoursWorked = Tasks.Where(t => t.Project.ActualEndDate != null).GroupBy(v => v.Name)
                        .Select(group => new HoursWokredDto { Task = group.Key, HoursWorked = group.Sum(c => c.ActualHours).ToString() })
                        .ToList();

                    //How many projects one user is assigned
                    CostType = Tasks.Where(t => t.Project.ActualEndDate != null).GroupBy(v => v.CostType.Name.ToString())
                        .Select(group => new CostTypeDto { TheCostType = group.Key, ActSpent = group.Sum(c => c.ActualCost).ToString() })
                        .ToList();
                }
            }         
        }

        public class VendorCostDto
        {
            public string Vendor { get; set; }
            public int Count { get; set; }
            public string Cost { get; set; }
        }

        public class HoursWokredDto
        {
            public string Task { get; set; }
            public string HoursWorked { get; set; }
        }

        public class CostTypeDto
        {
            public string TheCostType { get; set; }
            public string ActSpent { get; set; }
        }
    }
}
