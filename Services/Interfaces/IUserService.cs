using Microsoft.AspNetCore.Identity;
using OCHPlanner3.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OCHPlanner3.Services.Interfaces
{
    public interface IUserService
    {
        UserCredentials GetCurrentUserCredentials();
        Task<IList<Claim>> GetUserClaims(string userId);
        Task<IdentityUser> GetUserById(string userId);
        Task<int> GetRemainingUsers(int garageId);
    }
}
