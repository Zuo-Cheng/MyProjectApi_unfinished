using MediatR;
using ProjectApi.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectApi.Application.Commands
{
    public class JoinProjectCommandHandler : IRequestHandler<JoinProjectCommand>
    {
        private readonly IProjectRepository _projectRepository;

        public JoinProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<Unit> Handle(JoinProjectCommand request, CancellationToken cancellationToken)
        {
           var project =  await _projectRepository.GetAsync(request.Contributor.ProjectId);
            if(project == null)
            {
                throw new Domain.Exceptions.ProjectDomainException($"project 没有找到：{request.Contributor.ProjectId}");
            }
            project.AddContributor(request.Contributor);
            await _projectRepository.UnitOfWork.SaveEntitiesAsync();
            return default(Unit);
        }
    }
}
