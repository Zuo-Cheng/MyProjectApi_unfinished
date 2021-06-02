using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectApi.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;


namespace ProjectApi.Infrastructure.EntityConfiguration
{
    public class ProjectEntityConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects")
                .HasKey(t => t.Id);
        }
    }
}
