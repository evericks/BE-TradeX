using Application.Services.Interfaces;
using AutoMapper;
using Data;
using Data.Repositories.Interfaces;

namespace Application.Services.Implementations
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
            _userRepository = unitOfWork.User;
        }
    }
}
