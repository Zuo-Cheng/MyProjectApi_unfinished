using MediatR;
using ProjectApi.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectApi.Application.Commands
{
    /// <summary>
    /// 构建消息处理
    /// </summary>
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCmmand, Project>
    {

        private  IProjectRepository _projectRepository;

        public CreateProjectCommandHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task<Project> Handle(CreateProjectCmmand request, CancellationToken cancellationToken)
        {
             _projectRepository.Add(request.Project);
            await _projectRepository.UnitOfWork.SaveEntitiesAsync();
            return request.Project;
        }
    }
}
