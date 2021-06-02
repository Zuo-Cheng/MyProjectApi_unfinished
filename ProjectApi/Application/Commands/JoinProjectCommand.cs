using MediatR;
using ProjectApi.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi.Application.Commands
{
    public class JoinProjectCommand:IRequest
    {
        public ProjectContributor Contributor { get; set; }
    }
}
