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
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(SignInManager<IdentityUser> userIdentity,
            IGarageFactory garageFactory,
            UserManager<IdentityUser> userManager,
            IHttpContextAccessor contextAccessor)
        {
            _userIdentity = userIdentity;
            _contextAccessor = contextAccessor;
            _garageFactory = garageFactory;
            _userManager = userManager;
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

        public async Task<int> GetRemainingUsers(int garageId)
        {
            var garage = await _garageFactory.GetGarage(garageId);
            var currentUsers = await GetUsersForClaim("GarageId", garageId.ToString());

            if (garage == null)
                throw new ApplicationException($"Garage id {garageId} not found in database");

            var maxUserCount = garage.NbrUser;
            var currentUserCount = currentUsers.Count();

            return (maxUserCount - currentUserCount) < 0 ? 0 : (maxUserCount - currentUserCount);
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

        private async Task<IEnumerable<IdentityUser>> GetUsersForClaim(string userClaimType, string claimValue)
        {
            var claim = new Claim(userClaimType, claimValue);
            var users = await _userManager.GetUsersForClaimAsync(claim);
            return users;
        }

    }
}
