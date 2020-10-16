using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OCHPlanner3.Data.Interfaces;
using OCHPlanner3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OCHPlanner3.Services.Interfaces
{
    public class UserService : IUserService
    {
        private readonly SignInManager<IdentityUser> _userIdentity;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IGarageFactory _garageFactory;
        public UserService(SignInManager<IdentityUser> userIdentity,
            IGarageFactory garageFactory,
            IHttpContextAccessor contextAccessor)
        {
            _userIdentity = userIdentity;
            _contextAccessor = contextAccessor;
            _garageFactory = garageFactory;
        }
        public UserCredentials GetCurrentUserCredentials()
        {
            var user = new UserCredentials()
            {
                GarageId = GetGarageId(),
                Claims = GetCurrentUserClaims(),
                GarageSetting = GetGarageSetting().Result,
            };

            return user;
        }

        private int GetGarageId()
        {
            var garageId = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "GarageId")?.Value;
            return Convert.ToInt32(garageId);
        }

        private IEnumerable<Claim> GetCurrentUserClaims()
        {
            return _contextAccessor.HttpContext.User.Claims;
        }

        private async Task<GarageViewModel> GetGarageSetting()
        {
            var garageId = _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "GarageId")?.Value;

            if (string.IsNullOrWhiteSpace(garageId)) return new GarageViewModel();

            var garage = await _garageFactory.GetGarage(Convert.ToInt32(garageId));
            return garage.Adapt<GarageViewModel>();

        }
        //private string GetCurrentUserId()
        //{
        //    return _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        //}

        //private string GetCurrentUserEmail()
        //{
        //    return _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        //}

        //private string GetCurrentUserRole()
        //{
        //    var result = new StringBuilder();
        //    _contextAccessor.HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Role).ToList().ForEach(p =>
        //    {
        //        result.Append(p.Value);
        //        result.Append("<br>");
        //    });

        //    return result.ToString();
        //}

        //private string GetCurrentLanguage()
        //{
        //    return _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Language")?.Value;
        //}

        //private string GetCurrentFirstName()
        //{
        //    return _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "FirstName")?.Value;
        //}

        //private string GetCurrentLastName()
        //{
        //    return _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "LastName")?.Value;
        //}

        //private string GetCurrentUsername()
        //{
        //    return _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        //}
    }
}
