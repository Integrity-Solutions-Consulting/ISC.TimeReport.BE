using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isc.time.report.be.domain.Models.Request.Auth;
using isc.time.report.be.domain.Models.Response.Auth;

namespace isc.time.report.be.application.Interfaces.Service.Auth
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginRequest registerRequest);
        Task<RegisterResponse> Register(RegisterRequest registerRequest);
    }
}
