using Exceptionless;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OCHPlanner3.Helper;
using OCHPlanner3.Models;
using OCHPlanner3.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace OCHPlanner3.Controllers
{
    [Authorize]
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class ProgramController : BaseController
    {
        public readonly IProgramService _programService;

        public ProgramController(IHttpContextAccessor httpContextAccessor,
           IProgramService programService,
           IUserService userService) : base(httpContextAccessor, userService)
        {
            _programService = programService;
        }

        [Route("/{lang:lang}/Program/{id}")]
        public async Task<IActionResult> Index(int id)
        {
            try
            {
                var model = new ProgramManagementViewModel()
                {
                    RootUrl = BaseRootUrl,
                    SelectedGarageId = id,
                    Programs = await _programService.GetPrograms(id)
                };

                return View(model);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpPost("/{lang:lang}/Program/Create")]
        public async Task<IActionResult> Create(ProgramViewModel model)
        {
            try
            {
                var result = await _programService.Create(model);
                return Ok();
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpDelete("/{lang:lang}/Program/Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _programService.Delete(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }

        [HttpPost("/{lang:lang}/Program/Update")]
        public async Task<IActionResult> Update(ProgramViewModel program)
        {
            try
            {
                var result = await _programService.Update(program);
                return Ok(result);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }


        [Route("/{lang:lang}/ProgramList/{id}")]
        public async Task<IActionResult> GetProgramList(int id)
        {
            try
            {
                if (id == 0)
                    throw new ApplicationException("ProgramList - Id should ne be set to 0");

                var model = new ProgramManagementViewModel() { Programs = await _programService.GetPrograms(id) };
                return PartialView("_programs", model);
            }
            catch (Exception ex)
            {
                ex.ToExceptionless().Submit();
                return BadRequest();
            }
        }
    }
}
