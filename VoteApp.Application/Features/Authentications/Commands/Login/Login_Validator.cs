using FluentValidation;

namespace VoteApp.Application.Features.Authentications.Commands.Login
{
    public class Login_Validator : AbstractValidator<Login_Command>
    {
        public Login_Validator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
        }
    }
}
