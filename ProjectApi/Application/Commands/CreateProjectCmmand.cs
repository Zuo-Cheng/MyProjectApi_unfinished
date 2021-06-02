using MediatR;
using ProjectApi.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi.Application.Commands
{
    /// <summary>
    /// 构建消息请求
    /// </summary>
    public class CreateProjectCmmand:IRequest<Project>
    {
        public Project Project { get; set; }
    }
}
