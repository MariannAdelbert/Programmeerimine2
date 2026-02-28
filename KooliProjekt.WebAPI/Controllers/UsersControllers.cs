using KooliProjekt.Application.Features.User;
using KooliProjekt.Application.Features.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebAPI.Controllers
{
    public class UsersController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _mediator.Send(new GetUserQuery { Id = id });
            return Result(response);
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _mediator.Send(new ListUsersQuery
            {
                Page = page,
                PageSize = pageSize
            });
            return Result(response);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var command = new DeleteUserCommand { Id = id };
            var response = await _mediator.Send(command);
            return Result(response);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save([FromBody] SaveUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Result(result);
        }
    }
}