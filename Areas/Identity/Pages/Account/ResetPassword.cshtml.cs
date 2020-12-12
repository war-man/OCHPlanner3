using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;

namespace OCHPlanner3.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IStringLocalizer<ResetPasswordModel> _localizer;

        public ResetPasswordModel(UserManager<IdentityUser> userManager,
            IStringLocalizer<ResetPasswordModel> localizer)
        {
            _userManager = userManager;
            _localizer = localizer;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }

            public TranslationModel Translation { get; set; }
        }

        public class TranslationModel
        {
            public string Message { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string ConfirmPassword { get; set; }
            public string ResetPassword { get; set; }
        }
        public IActionResult OnGet(string code = null)
        {
            if (code == null)
            {
                return BadRequest("A code must be supplied for password reset.");
            }
            else
            {
                if (HttpContext.Request.Query.ContainsKey("lang"))
                    CultureInfo.CurrentUICulture = new CultureInfo(HttpContext.Request.Query["lang"], false);
                else
                    CultureInfo.CurrentUICulture = new CultureInfo("fr", false);

                Input = new InputModel
                {
                    Code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code)),
                    Translation = new TranslationModel()
                    {
                        Message = _localizer["Message"],
                        Email = _localizer["Email"],
                        Password = _localizer["Password"],
                        ConfirmPassword = _localizer["ConfirmPassword"],
                        ResetPassword = _localizer["ResetPassword"]
                    }
                };
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var language = HttpContext.Request.Query.ContainsKey("lang") ? HttpContext.Request.Query["lang"].ToString() : "fr";

            CultureInfo.CurrentUICulture = new CultureInfo(language, false);

            Input.Translation = new TranslationModel()
            {
                Message = _localizer["Message"],
                Email = _localizer["Email"],
                Password = _localizer["Password"],
                ConfirmPassword = _localizer["ConfirmPassword"],
                ResetPassword = _localizer["ResetPassword"]

            };

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToPage("./ResetPasswordConfirmation", new { lang = language });
            }

            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                return RedirectToPage("./ResetPasswordConfirmation", new { lang = language });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return Page();
        }
    }
}
