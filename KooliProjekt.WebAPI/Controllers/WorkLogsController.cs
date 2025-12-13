using KooliProjekt.Application.Features.WorkLogs;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebAPI.Controllers
{
    public class WorkLogsController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        public WorkLogsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetWorkLogQuery { Id = id };
            var response = await _mediator.Send(query);

            return Result(response);
        }

        [HttpPost("Save")]
        public async Task<IActionResult> Save([FromBody] SaveWorkLogCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(DeleteWorkLogCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }
    }
}
