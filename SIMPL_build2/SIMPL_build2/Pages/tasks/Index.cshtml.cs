using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SIMPL.Models;

namespace SIMPL.Pages.tasks
{
    public class IndexModel : PageModel
    {
        private readonly SIMPL.Models.project_trackerContext _context;

        public IndexModel(SIMPL.Models.project_trackerContext context)
        {
            _context = context;
        }

        //This allows for a query sting named QueryProjectId 
        //?QueryProjectId= <Project ID>
        [BindProperty(SupportsGet = true)]
        public string QueryProjectId { get; set; }

        public IList<Tasks> Tasks { get;set; }

        public async Task OnGetAsync()
        {
            Tasks = await _context.Tasks
                .Include(t => t.CostType)
                .Include(t => t.CreatedBy)
                .Include(t => t.Location)
                .Include(t => t.Project)
                .Include(t => t.Vendor).ToListAsync();

            if(QueryProjectId != null)
            {
                if(int.TryParse(QueryProjectId, out var ParsedProjectId))
                {
                    Tasks = Tasks.Where(i => i.ProjectId == ParsedProjectId).ToList();
                }
            }
        }
    }
}
