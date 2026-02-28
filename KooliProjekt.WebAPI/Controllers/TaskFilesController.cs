using KooliProjekt.Application.Features.TaskFiles;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class TaskFilesController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public TaskFilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _mediator.Send(new GetTaskFileQuery { Id = id });
            return Result(response);
        }

        [HttpGet("List")]
        public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _mediator.Send(new ListTaskFilesQuery
            {
                Page = page,
                PageSize = pageSize
            });
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