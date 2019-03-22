using System;
using System.Collections.Generic;

namespace SIMPL.Models
{
    public partial class CostTypes
    {
        public CostTypes()
        {
            Tasks = new HashSet<Tasks>();
        }

        public int CostTypeId { get; set; }
        public string Name { get; set; }

        public ICollection<Tasks> Tasks { get; set; }
    }
}
