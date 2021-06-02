using ProjectApi.Domain.AggregatesModel;
using ProjectApi.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjectApi.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private  ProjectContext _context;
        public ProjectRepository(ProjectContext context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork
        {
            get => _context;
            set { _context = (ProjectContext)value; }
        }
       

        public Task<Project> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateAsync(Project project)
        {
            throw new NotImplementedException();
        }

        Task<Project> IProjectRepository.AddAsync(Project project)
        {
            throw new NotImplementedException();
        }
    }
}
