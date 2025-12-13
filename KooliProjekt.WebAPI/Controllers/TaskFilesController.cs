using KooliProjekt.Application.Features.TaskFiles;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebAPI.Controllers
{
    public class TaskFilesController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public TaskFilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetTaskFileQuery { Id = id };
            var response = await _mediator.Send(query);

            return Result(response);
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] SaveTaskFileCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(DeleteTaskFileCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }
    }
}
