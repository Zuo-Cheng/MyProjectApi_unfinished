using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi.Application.Service
{
    /// <summary>
    /// 检查项目是否在推荐列表
    /// </summary>
    public interface IRecommendService
    {
        Task<bool> IsProjectRecommend(int projectId,int userId);
    }
}
