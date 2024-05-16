using Domain.Models.Authentications;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IActionResult> AuthenticateForAdmin(CertificateModel certificate);
        Task<IActionResult> AuthenticateForUser(CertificateModel certificate);
        Task<IActionResult> GetUserInformation(Guid id);
        Task<AuthModel> GetUser(Guid id);
    }
}
