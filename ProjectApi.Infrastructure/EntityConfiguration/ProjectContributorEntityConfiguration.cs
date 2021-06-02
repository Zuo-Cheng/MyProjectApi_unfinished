using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectApi.Domain.AggregatesModel;

namespace ProjectApi.Infrastructure.EntityConfigurations
{
    public class ProjectContributorEntityConfiguration : IEntityTypeConfiguration<ProjectContributor>
    {
        public void Configure(EntityTypeBuilder<ProjectContributor> builder)
        {
            builder.ToTable("ProjectContributors").HasKey(p => p.Id);
        }
    }
}