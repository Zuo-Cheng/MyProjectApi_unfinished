using MediatR;
using ProjectApi.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectApi.Domain.Events
{
    public class ProjectJoinedEvent : INotification
    {
        public ProjectContributor Contributor { get; set; }
    }
}
