using Application.Services.Interfaces;
using Common.Extensions;
using Domain.Constants;
using Domain.Models.Authentications;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("users")]
        public async Task<IActionResult> AuthenticateForUser([FromBody] CertificateModel certificate)
        {
            try
            {
                return await _authService.AuthenticateForUser(certificate);
            }
            catch (Exception e)
            {
                return e.Message.InternalServerError();
            }
        }

        [HttpPost]
        [Route("admins")]
        public async Task<IActionResult> AuthenticateForAdmin([FromBody] CertificateModel certificate)
        {
            try
            {
                return await _authService.AuthenticateForAdmin(certificate);
            }
            catch (Exception e)
            {
                return e.Message.InternalServerError();
            }
        }

        [HttpPost]
        [Route("users/sign-in-with-token")]
        [Authorize(UserRoles.USER)]
        public async Task<IActionResult> GetUserInformation()
        {
            try
            {
                var auth = this.GetAuthenticatedUser();
                return await _authService.GetUserInformation(auth.Id);
            }
            catch (Exception e)
            {
                return e.Message.InternalServerError();
            }
        }

        [HttpPost]
        [Route("admins/sign-in-with-token")]
        [Authorize(UserRoles.ADMIN)]
        public async Task<IActionResult> GetAdminInformation()
        {
            try
            {
                var auth = this.GetAuthenticatedUser();
                return await _authService.GetUserInformation(auth.Id);
            }
            catch (Exception e)
            {
                return e.Message.InternalServerError();
            }
        }
    }
}
