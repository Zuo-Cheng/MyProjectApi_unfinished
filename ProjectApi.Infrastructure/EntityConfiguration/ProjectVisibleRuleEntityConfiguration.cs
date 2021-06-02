using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectApi.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectApi.Infrastructure.EntityConfigurations
{
   
    public class ProjectVisibleRuleEntityConfiguration : IEntityTypeConfiguration<ProjectVisibleRule>
    {
        public void Configure(EntityTypeBuilder<ProjectVisibleRule> builder)
        {
            builder.ToTable("ProjectVisibleRules").HasKey(p => p.Id);
        }
    }
}
