using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OCHPlanner3.Helper;
using OCHPlanner3.Models;
using OCHPlanner3.Services.Interfaces;

namespace OCHPlanner3.Controllers
{
    [Authorize]
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class ProgramController : BaseController
    {
        public ProgramController(IHttpContextAccessor httpContextAccessor,
           IUserService userService) : base(httpContextAccessor, userService)
        {
           
        }

        [Route("/{lang:lang}/Program/{id}")]
        public IActionResult Index(int id)
        {
            var model = new ProgramManagementViewModel()
            {
                SelectedGarageId = id
            };

            return View();
        }
    }
}
