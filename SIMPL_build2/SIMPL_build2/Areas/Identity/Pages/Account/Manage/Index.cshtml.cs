using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using SIMPL.Models;

namespace SIMPL.Areas.Identity.Pages.Account.Manage
{
   public partial class IndexModel : PageModel
    {
        private readonly SIMPL.Models.project_trackerContext _context;
        private readonly RoleManager<AspNetRoles> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<IndexModel> _logger;
        private string userID;
        private IdentityUser userObj;

        public IndexModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<AspNetRoles> roleManager,
            IEmailSender emailSender,
            ILogger<IndexModel> logger,
            SIMPL.Models.project_trackerContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _logger = logger;
            _context = context;
        }

        
        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel : PageModel
        {
            
            [Display(Name = "IdName")]
            public string LastName { get; set; }
            public string FirstName { get; set; }

            public string ID { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            
            
            [Display(Name ="ID")]
            public IdentityUser userObj { get; set; }
            public string userID { get; set; }

            //[Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current password")]
            public string OldPassword { get; set; }

            //[Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string LockoutEnd { get; set; }
            public bool LockedOut { get; set; }
            public string LockedOutActive { get; set; }

            public List<SelectListItem> Roles { get; } = new List<SelectListItem>
            {

            };

        }



        public async Task<IActionResult> OnGetAsync(string ID)
        {

            ViewData["Roles"] = new SelectList(_context.AspNetRoles, "Id", "Name");
            //var user = await _userManager.GetUserAsync(User);
            if ((ID != null)) //&& (userObj == null))
            {
                HttpContext.Session.SetString("ID", ID);
                userID = HttpContext.Session.GetString("ID");
            }
            else
            {
                userID = HttpContext.Session.GetString("ID");
            }

           

            var user = await _userManager.FindByIdAsync(userID);
            //string userName = user.LastName;

            userObj = await _userManager.FindByIdAsync(userID);
            if ((userID == null)) //&& (userObj == null))
            {
                return NotFound($"GET ID value is NULL.");
            }

            var hasPassword = await _userManager.HasPasswordAsync(userObj);
            if (!hasPassword)
            {
                return RedirectToPage("./SetPassword");
            }

            string userName = await _userManager.GetUserNameAsync(userObj);
            String pattern = "[.]";
            String[] elements = Regex.Split(userName, pattern);
           

            var email = await _userManager.GetEmailAsync(userObj);
           
            Username = userName;

            Input = new InputModel
            {
                Email = email,
                ID = userID
            };

            if (Input.LockedOut == true)
            {
                await _userManager.SetLockoutEndDateAsync(userObj, DateTime.UtcNow.AddYears(20));
            }

            if (elements.Length > 0)
            {
                Input.FirstName = elements[0];
                Input.LastName = elements[1];
            }
            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(userObj);
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string ID)
        {
            
            if (ID != null)
            {
                HttpContext.Session.SetString("ID", ID);

                userID = HttpContext.Session.GetString("ID");
                
            }

            


            if (!ModelState.IsValid)
            {
                return Page();
            }
           

            IdentityUser userObj = await _userManager.FindByIdAsync(ID);
            
            if (userObj == null)
            {
                return NotFound($"POST ID Value is NULL");
            }

            var email = await _userManager.GetEmailAsync(userObj);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(userObj, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(userObj);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }
            if (await _userManager.GetUserNameAsync(userObj) != (Input.FirstName + "." + Input.LastName)){
                await _userManager.SetUserNameAsync(userObj, (Input.FirstName + "." + Input.LastName));
            }

            Input.LockedOutActive = "checked";
            if (await _userManager.GetLockoutEndDateAsync(userObj) < DateTime.UtcNow)
            {
                Input.LockedOutActive = "checked";
            }

            if (Input.LockedOut == true)
            {
                await _userManager.SetLockoutEndDateAsync(userObj, DateTime.UtcNow.AddYears(20));
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(userObj);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(userObj, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(userObj);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }
          
            if (Input.ConfirmPassword != null)
            {
                string token = await _userManager.GeneratePasswordResetTokenAsync(userObj);
                if (token == null)
                {
                    return NotFound($"Unable to generate Token for User with ID '{_userManager.GetUserIdAsync(userObj)}'.");
                }

                var changePasswordResult = await _userManager.ResetPasswordAsync(userObj, token, Input.NewPassword);

                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    return Page();
                }
            }
            Input.userID = HttpContext.Session.GetString("ID");
            //await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        
    }
}
