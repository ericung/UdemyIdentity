using IdentityApp.Data.Account;
using IdentityApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;

namespace IdentityApp.Pages.Account
{
    public class RegisterModel : PageModel
    {
        public RegisterModel(UserManager<User> userManager,
            IEmailService emailService)
        {
            UserManager = userManager;
            EmailService = emailService;
        }

        [BindProperty]
        public RegisterViewModel RegisterViewModel { get; set; }
        public UserManager<User> UserManager { get; }
        public IEmailService EmailService { get; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Validating Email address (Optional)

            // Create the user
            var user = new User
            {
                Email = RegisterViewModel.Email,
                UserName = RegisterViewModel.Email,
                Department = RegisterViewModel.Department,
                Position = RegisterViewModel.Position
            };

            var claimDepartment = new Claim("Department", RegisterViewModel.Department);
            var claimPosition = new Claim("Position", RegisterViewModel.Position);

            var result = await UserManager.CreateAsync(user, RegisterViewModel.Password);
            if (result.Succeeded)
            {
                await UserManager.AddClaimAsync(user, claimDepartment);
                await UserManager.AddClaimAsync(user, claimPosition);

                var confirmationToken = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.PageLink(pageName: "/Account/ConfirmEmail",
                    values: new { userId = user.Id, token = confirmationToken });

                await EmailService.SendAsync("ungericwei@gmail.com",
                    user.Email,
                    "Please confirm your email",
                    $"Please click on this link to confirm your email address: {confirmationLink}");

                return RedirectToPage("/Account/Login");
            }
            else
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }

                return Page();
            }
        }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        public string Position { get; set; }
    }
}
