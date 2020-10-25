using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using OCHPlanner3.Data.Interfaces;
using OCHPlanner3.Models;
using OCHPlanner3.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OCHPlanner3.Services
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

        public async Task<IList<Claim>> GetUserClaims(string userId)
        {
            var user = await GetUserById(userId);
            return await _userIdentity.UserManager.GetClaimsAsync(user);
        }

        public async Task<IdentityUser> GetUserById(string userId)
        {
            return await _userIdentity.UserManager.FindByIdAsync(userId);
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

       
    }
}
