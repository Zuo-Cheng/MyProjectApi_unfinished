﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi.Application.Service
{
    public class TestRecommendService : IRecommendService
    {
        public Task<bool> IsProjectRecommend(int projectId, int userId)
        {
            return Task.FromResult(true);
        }
    }
}
