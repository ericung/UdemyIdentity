using IdentityApp.Data.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityApp.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        public UserManager<User> UserManager { get; }

        [BindProperty]
        public string Message { get; set; }

        public ConfirmEmailModel(UserManager<User> userManager)
        {
            UserManager = userManager;
        }


        public async Task<IActionResult> OnGetAsync(string userId, string token)
        {
            var user = await UserManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await UserManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    this.Message = "Email address successfully confirmed";
                    return Page();
                }
            }

            this.Message = "Failed to validate email.";

            return Page();
        }
    }
}
