﻿using isc.time.report.be.application.Interfaces.Service.Auth;
using isc.time.report.be.domain.Models.Request.Auth;
using isc.time.report.be.domain.Models.Response.Auth;
using isc.time.report.be.domain.Models.Response.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace isc.time.report.be.api.Controllers.v1.Auth
{
    [ApiController]
    [Route("api/[controller]")]    

    public class AuthController : ControllerBase
    {
        
            private readonly IAuthService authService;
            public AuthController(IAuthService authService)
            {
                this.authService = authService;
            }

            [HttpPost("login")]
            public async Task<ActionResult<SuccessResponse<LoginResponse>>> Login(LoginRequest loginRequest)
            {
                var login = await authService.Login(loginRequest);

                return Ok(new SuccessResponse<LoginResponse>(200, "Login èxitoso.", login));
            }

            [HttpPost("register")]
            public async Task<ActionResult<SuccessResponse<RegisterResponse>>> Register(RegisterRequest registerRequest)
            {
                var register = await authService.Register(registerRequest);

                return Ok(new SuccessResponse<RegisterResponse>(200, "Registro èxitoso.", register));
            }
    }
}
