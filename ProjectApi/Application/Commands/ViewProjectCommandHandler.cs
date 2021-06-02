using MediatR;
using ProjectApi.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectApi.Application.Commands
{
    public class ViewProjectCommandHandler:IRequestHandler<ViewProjectCommand>
    {
        private readonly IProjectRepository _projectRepository;

        public ViewProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }



        public async Task<Unit> Handle(ViewProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetAsync(request.ProjectId);
            if (project == null)
            {
                throw new Domain.Exceptions.ProjectDomainException($"project 没有找到：{request.ProjectId}");
            }

            project.AddViewer(request.UserId, request.UserName, request.Avatar);
            await _projectRepository.UnitOfWork.SaveEntitiesAsync();
            return default(Unit);
        }

    }
}
