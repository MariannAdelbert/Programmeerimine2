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

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(DeleteTaskFileCommand command)
        {
            var response = await _mediator.Send(command);
            return Result(response);
        }
    }
}
