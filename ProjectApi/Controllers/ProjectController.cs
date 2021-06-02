using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjectApi.Application.Commands;
using ProjectApi.Application.Queries;
using ProjectApi.Application.Service;
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

        private readonly IRecommendService _recommendService;

        private readonly IProjectQueries _projectQueries;
        public ProjectController(
            ILogger<ProjectController> logger,
            IMediator mediator,
            IRecommendService recommendService,
            IProjectQueries projectQueries)
            : base(logger)
        {
            _mediator = mediator;
            _recommendService = recommendService;
            _projectQueries = projectQueries;
        }

        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetProjectsAsync()
        {
            var result = await _projectQueries.GetProjectsByUserId(UserIdentity.UserId);
            return Ok(result);
        }

        [Route("my/{projectId}")]
        [HttpGet]
        public async Task<IActionResult> GetMyProjectDetailAsync(int projectId)
        {
           var result = await _projectQueries.GetProjectDetail(projectId);
            if(result.UserId == UserIdentity.UserId)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("你没有权限查看");
            }
        }

        [Route("recommend/{projectId}")]
        [HttpGet]
        public async Task<IActionResult> GetRecommendProjectDetailAsync(int projectId)
        {
            var isRecommend = await _recommendService.IsProjectRecommend(projectId,UserIdentity.UserId);
            if (isRecommend)
            {
                var result =  _projectQueries.GetProjectDetail(projectId);
                return Ok(result);
            }
            else
            {
                return BadRequest("你没有权限查看");
            }
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

            if(await _recommendService.IsProjectRecommend(projectId, UserIdentity.UserId))
            {
                return BadRequest("没有查看该项目的权限");
            }

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
        public async Task<IActionResult> JoinProject(int projectId,[FromBody]ProjectContributor contributor)
        {

            if (await _recommendService.IsProjectRecommend(projectId,UserIdentity.UserId))
            {
                return BadRequest("没有查看该项目的权限");
            }

            var command = new JoinProjectCommand()
            {
                Contributor = contributor
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
