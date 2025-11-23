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

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(DeleteProjectTaskCommand command)
        {
            var response = await _mediator.Send(command);
            return Result(response);
        }
    }
}
