using System;
using System.Collections.Generic;
using System.Linq;
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
    [Authorize(Roles = "SuperAdmin")]
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class RoleController : BaseController
    {
        private readonly SignInManager<IdentityUser> _userIdentity;
        private RoleManager<IdentityRole> _roleManager;

        public RoleController(SignInManager<IdentityUser> signInManager,
            IHttpContextAccessor httpContextAccessor,
            RoleManager<IdentityRole> roleManager,
            IUserService userService) : base(httpContextAccessor, userService)
        {
            _userIdentity = signInManager;
            _roleManager = roleManager;
        }

        [Route("/{lang:lang}/Roles")]
        public async Task<IActionResult> Index()
        {
            var model = new RoleListViewModel()
            {
                Roles = await GetRoles(),
                RootUrl = BaseRootUrl
            };

            return View(model);
        }

        private async Task<List<RoleViewModel>> GetRoles()
        {
            var result = new List<RoleViewModel>();

            var RolesList = _roleManager.Roles.OrderBy(x => x.Name).ToList();

            var roles = RolesList.Adapt<IEnumerable<RoleViewModel>>();

            foreach (var role in roles)
            {
                var RolesUserlist = await _userIdentity.UserManager.GetUsersInRoleAsync(role.Name);
                result.Add(new RoleViewModel()
                {
                    Id = role.Id,
                    Name = role.Name,
                    UserCount = RolesUserlist.Count
                });
            }

            return result; ;
        }

        [HttpPost("/{lang:lang}/roles/[action]")]
        public async Task<IActionResult> CreateRole(string name)
        {
            try
            {
                var role = new IdentityRole(name);
                var result = await _roleManager.CreateAsync(role);

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

        [HttpPost("/{lang:lang}/roles/[action]")]
        public async Task<IActionResult> UpdateRole(string id, string name)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                    return NotFound("Role not found.");

                role.Name = name;

                var result = await _roleManager.UpdateAsync(role);
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

        [HttpGet("/{lang:lang}/roles/list")]
        public async Task<IActionResult> GetRoleList()
        {
            var model = new RoleListViewModel() { Roles = await GetRoles() };
            return PartialView("_roles", model);
        }

        [HttpDelete("/{lang:lang}/roles/[action]")]
        public async Task<ActionResult> DeleteRole(string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                    return NotFound("Role not found.");

                var result = await _roleManager.DeleteAsync(role);
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
    }
}