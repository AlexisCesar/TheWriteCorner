using AuthAPI.Data.DTOs;
using AuthAPI.Models;
using AutoMapper;

namespace AuthAPI.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDto, User>();
        }
    }
}
