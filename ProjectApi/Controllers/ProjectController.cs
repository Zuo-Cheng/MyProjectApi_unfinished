using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectApi.Application.Commands;
using ProjectApi.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectController : BaseController
    {
        /// <summary>
        /// MediatR专门负责发送的接口
        /// </summary>
        private readonly IMediator _mediator;
        public ProjectController(
            ILogger<ProjectController> logger,
            IMediator mediator)
            : base(logger)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreatProject([FromBody]Project project)
        {
            var command = new CreateProjectCmmand() { Project = project };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        [Route("view/{projectId}")]
        public async Task<IActionResult> ViewProject(int projectId)
        {
            var command = new ViewProjectCommand()
            {
                UserId = UserIdentity.UserId,
                UserName = UserIdentity.Name,
                Avatar = UserIdentity.Avatar,
                ProjectId = projectId
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        [Route("join/{projectId}")]
        public async Task<IActionResult> JoinProject([FromBody]ProjectContributor contributor)
        {
            var command = new JoinProjectCommand()
            {
                Contributor = contributor
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
