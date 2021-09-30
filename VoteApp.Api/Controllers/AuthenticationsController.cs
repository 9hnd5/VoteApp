using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VoteApp.Application.Features.Authentications.Commands.Login;
using VoteApp.Application.Features.Authentications.Commands.Register;

namespace VoteApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationsController: ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login_Command request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(Register_Command request)
        {
            
            return Ok(await _mediator.Send(request));
        }
    }
}
