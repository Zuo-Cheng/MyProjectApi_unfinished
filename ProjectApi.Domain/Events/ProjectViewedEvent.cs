using MediatR;
using ProjectApi.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectApi.Domain.Events
{
    public class ProjectViewedEvent : INotification
    {
        public ProjectViewer Viewer { get; set; }
    }
}
