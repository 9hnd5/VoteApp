using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VoteApp.Application.Commons.Interfaces;
using VoteApp.Application.Commons.Services.Authentication;
using VoteApp.Application.Features.Authentications.Commands.Login;
using VoteApp.Application.Features.Authentications.Commands.Models;
using VoteApp.Domain.Entities;

namespace VoteApp.Application.Features.Authentications.Commands.Register
{
    public class Register_Command : IRequest<LoginOrRegister_VM>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public class Hander : IRequestHandler<Register_Command, LoginOrRegister_VM>
        {
            private readonly IAuthenticationService _authService;
            private readonly IMapper _mapper;
            private readonly IAppDbContext _dbContext;
            private IMediator _mediator;

            public Hander(IAuthenticationService authService, IMapper mapper, IAppDbContext dbContext, IMediator mediator)
            {
                _authService = authService;
                _mapper = mapper;
                _dbContext = dbContext;
                _mediator = mediator;
            }

            public async Task<LoginOrRegister_VM> Handle(Register_Command request, CancellationToken cancellationToken)
            {
                var userEntity = new User() { Email = request.Email, Password = _authService.HasingPassword(request.Password) };
                _dbContext.Users.Add(userEntity);
                _dbContext.SaveChanges();
                return await _mediator.Send(new Login_Command() { Email = request.Email, Password = request.Password });
            }
        }
    }
}
