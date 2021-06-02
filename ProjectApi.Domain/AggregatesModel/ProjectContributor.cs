using ProjectApi.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectApi.Domain.AggregatesModel
{
    /// <summary>
    /// 项目参加者，贡献者
    /// </summary>
    public class ProjectContributor:Entity
    {

        public int ProjectId { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Avatar { get; set; }


        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 关闭者
        /// </summary>
        public bool IsCloser { get; set; } 


        public int ContributorType { get; set; }
    }
}
