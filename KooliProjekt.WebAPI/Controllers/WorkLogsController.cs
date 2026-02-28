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

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _mediator.Send(new GetWorkLogQuery { Id = id });
            return Result(response);
        }

        [HttpGet]
        [Route("List")]
        public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var response = await _mediator.Send(new ListWorkLogsQuery
            {
                Page = page,
                PageSize = pageSize
            });
            return Result(response);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(DeleteWorkLogCommand command)
        {
            var response = await _mediator.Send(command);
            return Result(response);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveWorkLogCommand command)
        {
            var response = await _mediator.Send(command);
            return Result(response);
        }
    }
}