using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OCHPlanner3.Services.Email;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace OCHPlanner3.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IStringLocalizer<ForgotPasswordModel> _localizer;

        public ForgotPasswordModel(UserManager<IdentityUser> userManager,
            IEmailSender emailSender,
            IStringLocalizer<ForgotPasswordModel> localizer)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _localizer = localizer;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            public TranslationModel Translation { get; set; }
        }

        public class TranslationModel
        {
            public string Message { get; set; }
            public string Email { get; set; }
            public string Submit { get; set; }
            public string Cancel { get; set; }
            public string ConfirmationTitle { get; set; }
            public string ConfirmationMessage { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (HttpContext.Request.Query.ContainsKey("lang"))
                CultureInfo.CurrentUICulture = new CultureInfo(HttpContext.Request.Query["lang"], false);
            else
                CultureInfo.CurrentUICulture = new CultureInfo("fr", false);

            Input = new InputModel()
            {
                Translation = new TranslationModel()
                {
                    Message = _localizer["Message"],
                    Email = _localizer["Email"],
                    Submit = _localizer["Submit"],
                    Cancel = _localizer["Cancel"],
                    ConfirmationTitle = _localizer["ConfirmationTitle"],
                    ConfirmationMessage = _localizer["ConfirmationMessage"]
                }
            };

        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var language = HttpContext.Request.Query.ContainsKey("lang") ? HttpContext.Request.Query["lang"].ToString() : "fr";

                    CultureInfo.CurrentUICulture = new CultureInfo(language, false);

                    var user = await _userManager.FindByEmailAsync(Input.Email);
                    if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                    {
                        // Don't reveal that the user does not exist or is not confirmed
                        return RedirectToPage("./ForgotPasswordConfirmation");
                    }

                    string urlDomain = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";

                    // For more information on how to enable account confirmation and password reset please 
                    // visit https://go.microsoft.com/fwlink/?LinkID=532713
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ResetPassword",
                        pageHandler: null,
                        values: new { area = "Identity", code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(
                        Input.Email,
                        _localizer["ForgotPasswordEmailTitle"],
                        GetMessageForgotPassword(urlDomain, callbackUrl + $"&lang={language}", language));

                    return Redirect($"./ForgotPassword?lang={language}&Confirmation=true");
                }
                return Page();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetMessageForgotPassword(string urlDomain, string callbackUrl, string language)
        {
            CultureInfo.CurrentUICulture = new CultureInfo(language, false);

            string emailMsg = _localizer["ForgotPasswordEmail"];

            emailMsg = emailMsg.Replace("{URL_DOMAIN}", urlDomain);

            string url = HtmlEncoder.Default.Encode(callbackUrl);
            emailMsg = emailMsg.Replace("{URL_TAG}", url);

            return emailMsg;
        }
    }
}
