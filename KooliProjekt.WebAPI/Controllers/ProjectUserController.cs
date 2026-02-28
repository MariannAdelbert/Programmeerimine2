using KooliProjekt.Application.Features.ProjectUsers;
using KooliProjekt.Application.Dto;
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
        public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = new ListProjectUsersQuery
            {
                Page = page,
                PageSize = pageSize
            };
            var response = await _mediator.Send(query); // response.Value on PagedResult<ProjectUserDto>
            return Result(response);
        }
    }
}