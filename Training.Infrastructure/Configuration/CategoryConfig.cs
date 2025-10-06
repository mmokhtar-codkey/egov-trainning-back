using Training.Domain.Entities.Lookups;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Training.Infrastructure.Configuration
{
    public class CategoryConfig : BaseConfig<Category, Guid>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);
            builder.Property(e => e.Name).HasMaxLength(350);
        }
    }
}
