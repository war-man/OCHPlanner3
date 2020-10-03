using OCHPlanner3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OCHPlanner3.Services.Interfaces
{
    public interface IUserService
    {
        UserCredentials GetCurrentUserCredentials();
    }
}
