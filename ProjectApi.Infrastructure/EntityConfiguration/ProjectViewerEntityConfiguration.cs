using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectApi.Domain.AggregatesModel;

namespace ProjectApi.Infrastructure.EntityConfigurations
{
    public class ProjectViewerEntityConfiguration : IEntityTypeConfiguration<ProjectViewer>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregatesModel.ProjectViewer> builder)
        {
            builder.ToTable("ProjectViewers").HasKey(p => p.Id);
        }
    }
}