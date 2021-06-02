using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectApi.Domain.SeedWork;
using ProjectApi.Infrastructure.EntityConfiguration;
using ProjectApi.Infrastructure.EntityConfigurations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectApi.Infrastructure
{

    public class ProjectContext : DbContext, IUnitOfWork
    {
        private IMediator _mediatR;

        public DbSet<Domain.AggregatesModel.Project> Projects { get; set; }



        public ProjectContext(
            DbContextOptions<ProjectContext> options, 
            IMediator mediator) 
            : base(options)
        {
            _mediatR = mediator;
        }
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _mediatR.DispatchDomainEventsAsync(this);
            await base.SaveChangesAsync();
            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProjectEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectContributorEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectViewerEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectPropertyEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectVisibleRuleEntityConfiguration());

        }

    }
}
