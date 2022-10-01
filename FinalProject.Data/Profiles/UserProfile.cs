using AutoMapper;
using FinalProject.Data.Entities;
using FinalProject.Data.Models;

namespace FinalProject.Data.Profiles;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserCreateDto, User>();
    }
}
