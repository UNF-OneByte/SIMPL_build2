using System;
using System.Collections.Generic;
using System.Linq;

namespace SIMPL.Models
{
    public partial class Projects
    {
        public Projects()
        {
            Tasks = new HashSet<Tasks>();

        }

        public int ProjectId { get; set; }
        public string ProjectManagerId { get; set; }
        public string ProjectManagerName { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? EstimatedStartDate { get; set; }
        public DateTime? EstimatedEndDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public decimal? EstimatedInitialCost { get; set; }
        public decimal? EstimatedInitialHours { get; set; }
        public string Notes { get; set; }
        //public int Status { get; set; }

        public AspNetUsers ProjectManager { get; set; }
        //public AspNetUsers IdName { get; set; }
        public ICollection<Tasks> Tasks { get; set; }

        public void getProjects()
        {
            var proj = from pro in ProjectName
                       where ProjectId == 1
                       select pro;
        }
    }
}
