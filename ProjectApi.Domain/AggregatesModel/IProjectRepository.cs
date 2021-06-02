using ProjectApi.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApi.Domain.AggregatesModel
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<Project> GetAsync(int id);
        Task<Project> AddAsync(Project project);

        void UpdateAsync(Project project);
    }
}
