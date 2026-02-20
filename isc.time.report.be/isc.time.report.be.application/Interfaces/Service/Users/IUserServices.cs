using isc.time.report.be.domain.Models.Request.Users;
using isc.time.report.be.domain.Models.Response.Users;

namespace isc.time.report.be.application.Interfaces.Service.Users
{
    public interface IUserServices
    {
        Task<UserResponse> UpdateUserAsync(int id, UpdateUserRequest request);

        Task<UserResponse> SuspendUser(int id);
        Task<UserResponse> UnSuspendUser(int id);
        Task<string> UpdatePassword(int id, UpdatePasswordRequest request);
        Task<List<GetAllUsersResponse>> GetAllUsersAsync();
        Task AssignRolesToUser(AssignRolesToUserRequest request);
        Task AssignModulesToUser(AssignModuleToUserRequest request);
        Task<GetRolesOfUserResponse> GetRolesOfUser(int userId);
        Task<GetModulesOfUserResponse> GetModulesOfUser(int userId);
    }
}
