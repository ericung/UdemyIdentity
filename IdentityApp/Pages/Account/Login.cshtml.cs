using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        public LoginModel(SignInManager<IdentityUser> signInManager)
        {
            SignInManager = signInManager;
        }

        [BindProperty]
        public CredentialViewModel Credential { get; set; }
        public SignInManager<IdentityUser> SignInManager { get; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var result = await SignInManager.PasswordSignInAsync(
                this.Credential.Email, 
                this.Credential.Password, 
                this.Credential.RememberMe,
                false);

            if (result.Succeeded)
            {
                return RedirectToPage("/Index");
            }
            else
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("Login", "You are locked out");
                }
                else
                {
                    ModelState.AddModelError("Login", "Failed to login");
                }

                return Page();
            }
        }
    }

    public class CredentialViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
