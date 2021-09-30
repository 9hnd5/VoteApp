using AutoMapper;
using VoteApp.Application.Commons.Mappings;
using VoteApp.Domain.Entities;

namespace VoteApp.Application.Features.Authentications.Commands.Models
{
    public class LoginOrRegister_VM : IMapFrom<User>
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public void Mapping(Profile p)
        {
            p.CreateMap<User, LoginOrRegister_VM>();
        }
    }
}
