using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VoteApp.Application.Features.Items.Commands.VoteItemByUser;
using VoteApp.Application.Features.Items.Queries.GetItemById;
using VoteApp.Application.Features.Items.Queries.GetItems;

namespace VoteApp.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IMediator _mediator;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        public ItemsController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public async Task<IActionResult> GetItems([FromQuery] GetItems_Query request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItems(int id)
        {
            var request = new GetItemById_Query() { Id = id };
            return Ok(await _mediator.Send(request));
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> PartialEditItem(int id, [FromBody] JsonPatchDocument<VoteItemByUser_Command> patchDocument)
        {
            var request = new VoteItemByUser_Command();
            patchDocument.ApplyTo(request);
            if (id != request.Id) return BadRequest();

            request.UserId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirst("USER_ID").Value);
            request.Id = id;

            await _mediator.Send(request);
            return NoContent();
        }
    }
}
