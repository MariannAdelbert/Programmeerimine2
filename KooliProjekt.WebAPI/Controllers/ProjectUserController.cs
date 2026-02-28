using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Features.ProjectUsers;
using KooliProjekt.Application.Features.TaskFiles;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectUsersController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectUsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET api/ProjectUsers/Get/5
        [HttpGet]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetProjectUserQuery { Id = id };
            var response = await _mediator.Send(query); // response.Value on ProjectUserDto
            return Result(response);
        }

        // GET api/ProjectUsers/List?page=1&pageSize=10
        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] int projectId)
        {
            var query = new ListProjectUsersQuery { ProjectId = projectId };
            var response = await _mediator.Send(query); // response: OperationResult<List<ProjectUserDto>>
            return Result(response);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int projectId, [FromQuery] int userId)
        {
            var command = new DeleteProjectUserCommand
            {
                ProjectId = projectId,
                UserId = userId
            };
            var response = await _mediator.Send(command);
            return Result(response);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveProjectUserCommand command)
        {
            var response = await _mediator.Send(command);
            return Result(response);
        }
    }
}