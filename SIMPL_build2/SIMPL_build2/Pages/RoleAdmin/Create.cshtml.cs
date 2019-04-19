using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SIMPL.Models;
using Microsoft.AspNetCore.Identity;

namespace SIMPL.Pages.RoleAdmin
{


    public class CreateModel : PageModel
    {
        private readonly SIMPL.Models.project_trackerContext _context;
        


        public CreateModel(SIMPL.Models.project_trackerContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {

                byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
                byte[] key = Guid.NewGuid().ToByteArray();
                string token = Convert.ToBase64String(time.Concat(key).ToArray());
                

                return Page();
        }

        private int Random()
        {
            throw new NotImplementedException();
        }

        [BindProperty]
        public AspNetRoles AspNetRoles { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.AspNetRoles.Add(AspNetRoles);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}