using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SIMPL.Models
{
    public partial class AspNetRoles : IdentityRole

    {
        
        public AspNetRoles(string name) : base(name)
        {
            Name = name;
        }

            public AspNetRoles() :base()
        {
            AspNetRoleClaims = new HashSet<AspNetRoleClaims>();
            AspNetUserRoles = new HashSet<AspNetUserRoles>();


        }

       
        public string Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }

        public ICollection<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public ICollection<AspNetUserRoles> AspNetUserRoles { get; set; }
    }
}
