using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Threading.Tasks;

namespace OCHPlanner3.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ResetPasswordConfirmationModel : PageModel
    {
        private readonly IStringLocalizer<ResetPasswordConfirmationModel> _localizer;

        public ResetPasswordConfirmationModel(IStringLocalizer<ResetPasswordConfirmationModel> localizer)
        {
            _localizer = localizer;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
           public TranslationModel Translation { get; set; }
        }

        public class TranslationModel
        {
            public string Message { get; set; }
            public string Cancel { get; set; }
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Request.Query.ContainsKey("lang"))
                CultureInfo.CurrentUICulture = new CultureInfo(HttpContext.Request.Query["lang"], false);
            else
                CultureInfo.CurrentUICulture = new CultureInfo("fr", false);

            Input = new InputModel
            {
                Translation = new TranslationModel()
                {
                    Message = _localizer["Message"],
                    Cancel = _localizer["Cancel"]
                }
            };

            return Page();
        }
    }
}
