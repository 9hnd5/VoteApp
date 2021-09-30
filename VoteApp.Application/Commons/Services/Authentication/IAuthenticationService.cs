using VoteApp.Domain.Entities;

namespace VoteApp.Application.Commons.Services.Authentication
{
    public interface IAuthenticationService
    {
        public string HasingPassword(string password);
        public string GenerateToken(User user);
    }
}
