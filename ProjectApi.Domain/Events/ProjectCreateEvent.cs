using MediatR;
using ProjectApi.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectApi.Domain.Events
{
    public  class ProjectCreateEvent:INotification
    {

        public Project Project { get; set; }
    }
}
