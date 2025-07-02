using isc.time.report.be.domain.Entity.Auth;
using isc.time.report.be.domain.Models.Request.Users;
using isc.time.report.be.domain.Models.Response.Users;
using isc.time.report.be.domain.Models.Response.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isc.time.report.be.application.Interfaces.Service.Users
{
    public interface IUserServices
    {
        Task<UserResponse> UpdateUserAsync(int id, UpdateUserRequest request);

        Task<UserResponse> SuspendUser(int id);
        Task<UserResponse> UnSuspendUser(int id);
        Task<UserResponse> UpdatePassword(int id, UpdatePasswordRequest request);
        Task<List<GetAllUsersResponse>> GetAllUsersAsync();
        Task AssignRolesToUser(AssignRolesToUserRequest request);
        Task AssignModulesToUser(AssignModuleToUserRequest request);
    }
}
