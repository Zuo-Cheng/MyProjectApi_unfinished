using ProjectApi.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectApi.Domain.AggregatesModel
{
    public class ProjectVisibleRule:Entity
    {
        public int ProjectId { get; set; }

        public bool Visible { get; set; }

        public string Tags { get; set; }
    }
}
