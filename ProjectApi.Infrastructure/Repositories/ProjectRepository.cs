using Microsoft.EntityFrameworkCore;
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
       

        public async Task<Project> GetAsync(int id)
        {
           var entity = await  _context.Projects
                .Include(t => t.Properties)
                .Include(t => t.Viewers)
                .Include(t => t.Contributors)
                .Include(t => t.VisibleRule)
                .SingleOrDefaultAsync();
            return entity;
        }

        public void Update(Project project)
        {
           _context.Projects.Update(project);

        }

        public Project Add(Project project)
        {
            if (project.IsTransient())
            {

                return _context.Projects.Add(project).Entity;
            }
            else
            {
                return project;
            }

        }
    }
}
