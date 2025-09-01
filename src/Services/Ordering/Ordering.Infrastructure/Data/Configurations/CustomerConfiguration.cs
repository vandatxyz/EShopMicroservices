using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasConversion
                (customerId => customerId.Value,
                dbId => CustomerId.Of(dbId));

            builder.Property(c => c.Name).IsRequired().HasMaxLength(256);

            builder.Property(c => c.Email).IsRequired().HasMaxLength(256);

            builder.HasIndex(c => c.Email).IsUnique();
        }
    }
}
