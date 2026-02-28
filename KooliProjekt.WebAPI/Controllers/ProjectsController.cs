using KooliProjekt.Application.Features.Projects;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebAPI.Controllers
{
    public class ProjectsController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] ListProjectsQuery query)
        {
            var result = await _mediator.Send(query);

            return Result(result);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetProjectQuery { Id = id };
            var response = await _mediator.Send(query);

            return Result(response);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveProjectCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var command = new DeleteProjectCommand { Id = id };
            var response = await _mediator.Send(command);
            return Result(response);
        }
    }
}
