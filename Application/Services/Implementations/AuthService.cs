using Application.Services.Interfaces;
using Application.Settings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common.Errors;
using Common.Extensions;
using Data;
using Data.Repositories.Interfaces;
using Domain.Constants;
using Domain.Models.Authentications;
using Domain.Models.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services.Implementations
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly AppSettings _appSettings;

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IOptions<AppSettings> appSettings) : base(unitOfWork, mapper)
        {
            _appSettings = appSettings.Value;
            _userRepository = unitOfWork.User;
        }

        public async Task<IActionResult> AuthenticateForAdmin(CertificateModel certificate)
        {
            try
            {
                var admin = await _userRepository.Where(x => x.Username.Equals(certificate.Username)
                && x.Password.Equals(certificate.Password) && x.Role.Equals(UserRoles.ADMIN))
                    .ProjectTo<AuthModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                if (admin == null)
                {
                    return AppErrors.INVALID_CERTIFICATE.BadRequest();
                }

                // Generate access token for admin
                var accessToken = GenerateJwtToken(admin);

                // Return access token and admin info
                return new AuthViewModel
                {
                    AccessToken = accessToken,
                    User = new UserViewModel
                    {
                        Id = admin.Id,
                        Username = admin.Username,
                        Name = admin.Name,
                        AvatarUrl = admin.AvatarUrl
                    }
                }.Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> AuthenticateForUser(CertificateModel certificate)
        {
            try
            {
                var user = await _userRepository.Where(x => x.Username.Equals(certificate.Username)
                && x.Password.Equals(certificate.Password) && x.Role.Equals(UserRoles.USER))
                    .ProjectTo<AuthModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return AppErrors.INVALID_CERTIFICATE.BadRequest();
                }

                // Generate access token for user
                var accessToken = GenerateJwtToken(user);

                // Return access token and user info
                return new AuthViewModel
                {
                    AccessToken = accessToken,
                    User = new UserViewModel
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Name = user.Name,
                        AvatarUrl = user.AvatarUrl
                    }
                }.Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> GetUserInformation(Guid id)
        {
            try
            {
                var user = await _userRepository.Where(st => st.Id.Equals(id))
                    .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                return user != null ? user.Ok() : AppErrors.USER_NOT_EXIST.NotFound();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<AuthModel> GetUser(Guid id)
        {
            try
            {
                var user = await _userRepository
                    .Where(st => st.Id.Equals(id))
                    .ProjectTo<AuthModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync();
                // Return user or null if not found any user
                return user != null ? user : null!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GenerateJwtToken(AuthModel auth)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", auth.Id.ToString()),
                    new Claim("role", auth.Role.ToString()),
                }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
