using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using VoteApp.Application.Commons.Interfaces;

namespace VoteApp.Application.Features.Authentications.Commands.Register
{
    public class Register_Validator : AbstractValidator<Register_Command>
    {
        private IAppDbContext _dbContext;
        public Register_Validator(IAppDbContext dbContext)
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .MustAsync(UniqueEmail).WithMessage("Email already exist");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
            _dbContext = dbContext;
        }

        public async Task<bool> UniqueEmail(string email, CancellationToken cancellationToken)
        {
            return await _dbContext.Users.AllAsync(x => x.Email != email);
        }
    }
}
