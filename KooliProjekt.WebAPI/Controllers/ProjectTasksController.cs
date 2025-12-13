using KooliProjekt.Application.Features.ProjectTasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebAPI.Controllers
{
    public class ProjectTasksController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public ProjectTasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetProjectTaskQuery { Id = id };
            var response = await _mediator.Send(query);

            return Result(response);
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] SaveProjectTaskCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(DeleteProjectTaskCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }
    }
}
