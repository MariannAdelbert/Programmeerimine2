using KooliProjekt.Application.Features.ProjectTasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectTasksController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public ProjectTasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(DeleteProjectTaskCommand command)
        {
            var response = await _mediator.Send(command);
            return Result(response);
        }

        // GET api/ProjectTasks/Get/5
        [HttpGet]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetProjectTaskQuery { Id = id };
            var response = await _mediator.Send(query);
            return Result(response);
        }

        // GET api/ProjectTasks/List?projectId=1&page=1&pageSize=10
        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] int projectId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = new ListProjectTasksQuery
            {
                ProjectId = projectId,
                Page = page,
                PageSize = pageSize
            };
            var response = await _mediator.Send(query);
            return Result(response);
        }
    }
}