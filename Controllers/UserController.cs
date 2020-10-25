using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OCHPlanner3.Helper;
using OCHPlanner3.Models;
using OCHPlanner3.Services.Interfaces;

namespace OCHPlanner3.Controllers
{
    [Authorize(Roles = "SuperAdmin, Administrator")]
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class UserController : BaseController
    {
        private readonly SignInManager<IdentityUser> _userIdentity;
        private readonly IUserService _userService;
        private readonly IGarageService _garageService;
        private readonly IReferenceService _referenceService;
        private readonly Dictionary<string, string> _roles;
        private readonly UserManager<IdentityUser> _userManager;

        private const string DefaultNewUserPassword = "Soleil123!";

        public UserController(SignInManager<IdentityUser> signInManager,
           IUserService userService,
           IGarageService garageService,
           IReferenceService referenceService,
           UserManager<IdentityUser> userManager,
           IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, userService)
        {
            _userIdentity = signInManager;
            _userService = userService;
            _garageService = garageService;
            _referenceService = referenceService;
            _roles = ((UserClaimsPrincipalFactory<IdentityUser, IdentityRole>)_userIdentity.ClaimsFactory).RoleManager.Roles.ToDictionary(r => r.Id, r => r.Name);
            _userManager = userManager;
        }

        [Route("/{lang:lang}/Users")]
        public async Task<IActionResult> Index()
        {
            //Get current user
            var user = await GetCurrentUserAsync();
            var claims = await _userService.GetUserClaims(user.Id);

            var language = claims.Any(c => c.Type == "Language") ? claims.FirstOrDefault(c => c.Type == "Language")?.Value : "FR";


            var model = new UserListViewModel()
            {
                RootUrl = BaseRootUrl,
                Users = await GetUsers(),
                GarageSelector = new GarageSelectorViewModel
                {
                    Garages = await _garageService.GetGaragesSelectList()
                },
            };

            ViewBag.Roles = _roles;

            return View(model);
        }

        [HttpPost("/{lang:lang}/[action]")]
        public async Task<IActionResult> CreateUser(string userName, string firstName, string lastName, string selectedGarageId, string email, string[] roles)
        {
            try
            {
                var fullName = $"{firstName} {lastName}";
                var user = new IdentityUser { Email = email.ToLower(), UserName = userName.ToLower() };

                var result = await _userIdentity.UserManager.CreateAsync(user, DefaultNewUserPassword);
                if (result.Succeeded)
                {
                    if (firstName != null)
                        await _userIdentity.UserManager.AddClaimAsync(user, new Claim("FirstName", firstName));

                    if (lastName != null)
                        await _userIdentity.UserManager.AddClaimAsync(user, new Claim("LastName", lastName));

                    if (selectedGarageId != null)
                        await _userIdentity.UserManager.AddClaimAsync(user, new Claim("GarageId", selectedGarageId));

                    foreach (string role in roles)
                        await _userIdentity.UserManager.AddToRoleAsync(user, role);

                    //Send email to a new user, for the password initialization
                    //await NewUserCompleteRegistration(user, fullName);

                    return NoContent();
                }

                return BadRequest(result.Errors.First().Description);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("/{lang:lang}/[action]")]
        public async Task<IActionResult> UpdateUser(string id, string email, string firstName, string lastName, string selectedGarageId, string locked, string[] roles)
        {
            try
            {
                var user = await _userIdentity.UserManager.FindByIdAsync(id);
                if (user == null)
                    return NotFound("User not found.");

                user.Email = email;
                user.LockoutEnd = locked == null ? default(DateTimeOffset?) : DateTimeOffset.MaxValue;

                var result = await _userIdentity.UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    var userRoles = await _userIdentity.UserManager.GetRolesAsync(user);

                    foreach (string role in roles.Except(userRoles))
                    {
                        await _userIdentity.UserManager.AddToRoleAsync(user, role);
                    }

                    foreach (string role in userRoles.Except(roles))
                    {
                        await _userIdentity.UserManager.RemoveFromRoleAsync(user, role);
                    }

                    var userClaims = await _userIdentity.UserManager.GetClaimsAsync(user);

                    if (firstName != null)
                    {
                        if (!userClaims.Any(c => c.Type == "FirstName")) //Add
                        {
                            await _userIdentity.UserManager.AddClaimAsync(user, new Claim("FirstName", firstName));
                        }
                        else if (userClaims.Any(c => c.Type == "FirstName") && userClaims.First(c => c.Type == "FirstName").Value != firstName)
                        {
                            var originalFirstname = userClaims.First(c => c.Type == "FirstName").Value;
                            await _userIdentity.UserManager.RemoveClaimAsync(user, new Claim("FirstName", originalFirstname));
                            await _userIdentity.UserManager.AddClaimAsync(user, new Claim("FirstName", firstName));
                        }

                    }

                    if (lastName != null)
                    {
                        if (!userClaims.Any(c => c.Type == "LastName")) //Add
                        {
                            await _userIdentity.UserManager.AddClaimAsync(user, new Claim("LastName", lastName));
                        }
                        else if (userClaims.Any(c => c.Type == "LastName") && userClaims.First(c => c.Type == "LastName").Value != lastName)
                        {
                            var originalLastname = userClaims.First(c => c.Type == "LastName").Value;
                            await _userIdentity.UserManager.RemoveClaimAsync(user, new Claim("LastName", originalLastname));
                            await _userIdentity.UserManager.AddClaimAsync(user, new Claim("LastName", lastName));
                        }

                    }

                    if (selectedGarageId != null)
                    {
                        if (!userClaims.Any(c => c.Type == "GarageId"))
                        {
                            await _userIdentity.UserManager.AddClaimAsync(user, new Claim("GarageId", selectedGarageId));
                        }
                        else if (userClaims.Any(c => c.Type == "GarageId") && userClaims.First(c => c.Type == "GarageId").Value != selectedGarageId)
                        {
                            var originalGarageId = userClaims.First(c => c.Type == "GarageId").Value;
                            await _userIdentity.UserManager.RemoveClaimAsync(user, new Claim("GarageId", originalGarageId));
                            await _userIdentity.UserManager.AddClaimAsync(user, new Claim("GarageId", selectedGarageId));
                        }
                    }
                                        
                    return NoContent();
                }

                return BadRequest(result.Errors.First().Description);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("/{lang:lang}/[action]")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                var user = await _userIdentity.UserManager.FindByIdAsync(id);
                if (user == null)
                    return NotFound("User not found.");

                var result = await _userIdentity.UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                     return NoContent();
                }
                else
                    return BadRequest(result.Errors.First().Description);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("/{lang:lang}/[action]")]
        public async Task<IActionResult> ResetPassword(string id, string password, string verify)
        {
            try
            {
                if (password != verify)
                    return BadRequest("Passwords entered do not match.");

                var user = await _userIdentity.UserManager.FindByIdAsync(id);
                if (user == null)
                    return NotFound("User not found.");

                if (await _userIdentity.UserManager.HasPasswordAsync(user))
                    await _userIdentity.UserManager.RemovePasswordAsync(user);

                var result = await _userIdentity.UserManager.AddPasswordAsync(user, password);
                if (result.Succeeded)
                {
                     return NoContent();
                }

                return BadRequest(result.Errors.First().Description);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("/{lang:lang}/users/list")]
        public async Task<IActionResult> GetUserList()
        {
            var model = new UserListViewModel() { Users = await GetUsers() };
            return PartialView("_users", model);
        }

        private Task<IdentityUser> GetCurrentUserAsync() => _userIdentity.UserManager.GetUserAsync(HttpContext.User);

        private async Task<IEnumerable<UserViewModel>> GetUsers()
        {
            var result = new List<UserViewModel>();
            var userList = _userIdentity.UserManager.Users.ToList();

            var users = userList.Adapt<IEnumerable<UserViewModel>>();

            foreach (var user in users)
            {
                IdentityUser identityUser = await _userIdentity.UserManager.FindByIdAsync(user.Id);
                IList<Claim> claims = await _userIdentity.UserManager.GetClaimsAsync(identityUser);
                IList<string> roles = await _userIdentity.UserManager.GetRolesAsync(identityUser);
                var garageName = await _garageService.GetGarage(claims.Any(c => c.Type == "GarageId") ? Convert.ToInt32(claims.FirstOrDefault(c => c.Type == "GarageId")?.Value) : 0);

                result.Add(new UserViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    LockedOut = user.LockoutEnd != null,
                    Roles = roles,
                    Clients = claims.FirstOrDefault(c => c.Type == "Clients")?.Value.Split(",").ToList(),
                    ClientsIdList = claims.Any(c => c.Type == "Clients") ? claims.FirstOrDefault(c => c.Type == "Clients")?.Value : string.Empty,
                    FirstName = claims.Any(c => c.Type == "FirstName") ? claims.FirstOrDefault(c => c.Type == "FirstName")?.Value : string.Empty,
                    LastName = claims.Any(c => c.Type == "LastName") ? claims.FirstOrDefault(c => c.Type == "LastName")?.Value : string.Empty,
                    UserName = user.UserName,
                    GarageId = claims.Any(c => c.Type == "GarageId") ? Convert.ToInt32(claims.FirstOrDefault(c => c.Type == "GarageId")?.Value) : 0,
                    GarageName = garageName.Name,  
                });
            }

            return result;
        }

        private async Task<string> BuildClientToDisplay(IList<string> clients)
        {
            var builder = new StringBuilder();

            if (!clients.Any()) return string.Empty;

            clients.ToList().ForEach(async c =>
            {
                builder.Append(c);
                builder.Append("<br />");
            });

            return builder.Remove(builder.Length - 6, 6).ToString();
        }

        private async Task NewUserCompleteRegistration(IdentityUser user, string userFullName)
        {
            // For more information on how to enable account confirmation and password reset please 
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            token = System.Web.HttpUtility.UrlEncode(token);
            string urlDomain = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            string callbackUrl = $"{urlDomain}/Identity/Account/CompleteRegistration?token={token}";

            //TODO
            //await _emailService.SendEmailCompleteRegistration(user.Email, userFullName, urlDomain, token, callbackUrl);
        }
    }
}