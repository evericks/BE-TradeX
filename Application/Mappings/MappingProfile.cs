using AutoMapper;
using Domain.Entities;
using Domain.Models.Authentications;
using Domain.Models.Views;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Default Data Type
            CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<double?, double>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<Guid?, Guid>().ConvertUsing((src, dest) => src ?? dest);
            CreateMap<DateTime?, DateTime>().ConvertUsing((src, dest) => src ?? dest);

            // Auth
            CreateMap<User, AuthModel>();

            // User
            CreateMap<User, UserViewModel>();
        }
    }
}
