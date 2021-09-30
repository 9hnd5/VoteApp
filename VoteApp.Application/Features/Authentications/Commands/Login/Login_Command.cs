using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoteApp.Application.Commons.Exceptions;
using VoteApp.Application.Commons.Interfaces;
using VoteApp.Application.Commons.Services.Authentication;
using VoteApp.Application.Features.Authentications.Commands.Models;
using VoteApp.Domain.Entities;
using Failure = FluentValidation.Results.ValidationFailure;

namespace VoteApp.Application.Features.Authentications.Commands.Login
{
    public class Login_Command : IRequest<LoginOrRegister_VM>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public class Handler : IRequestHandler<Login_Command, LoginOrRegister_VM>
        {
            private readonly IAuthenticationService _authService;
            private readonly IMapper _mapper;
            private readonly IAppDbContext _dbContext;

            public Handler(IAuthenticationService authService, IMapper mapper, IAppDbContext dbContext)
            {
                _authService = authService;
                _mapper = mapper;
                _dbContext = dbContext;
            }

            public async Task<LoginOrRegister_VM> Handle(Login_Command request, CancellationToken cancellationToken)
            {
                var user = await _dbContext.Users
                .FirstOrDefaultAsync(x => x.Email == request.Email);
                if (user != null && user.Password == _authService.HasingPassword(request.Password))
                {
                    var response = _mapper.Map<User, LoginOrRegister_VM>(user);
                    response.Token = _authService.GenerateToken(user);
                    return response;
                }
                var failures = new List<Failure>() {
                new Failure("email", "Email or password incorrect"),
                new Failure("password", "Email or password incorrect"),
            };
                throw new ValidateException(failures);
            }
        }
    }
}
