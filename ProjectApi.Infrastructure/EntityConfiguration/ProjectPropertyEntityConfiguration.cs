using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectApi.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectApi.Infrastructure.EntityConfigurations
{   

    public class ProjectPropertyEntityConfiguration : IEntityTypeConfiguration<ProjectProperty>
    {
        public void Configure(EntityTypeBuilder<Domain.AggregatesModel.ProjectProperty> builder)
        {
            builder.ToTable("ProjectProperties")
                .HasKey( p=> new { p.ProjectId ,p.Key,p.Value });
            builder.Property(b => b.Key).HasMaxLength(100);
            builder.Property(b => b.Value).HasMaxLength(100);
        }
    }
}
