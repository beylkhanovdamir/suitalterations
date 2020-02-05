using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SuitAlterations.Domain.Customers;

namespace SuitAlterations.Infrastructure.Domain.Customers
{
	/// <summary>
	///     Setting up a Customer entity and its own SuitAlteration entity for model creating within DbContext
	/// </summary>
	public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
	{
		public void Configure(EntityTypeBuilder<Customer> builder)
		{
			builder.ToTable("Customers", "dbo");

			builder.HasKey(b => b.Id);

			builder.Property(p => p.Id)
				.HasConversion(new ValueConverter<CustomerId, Guid>(id => id.Value,
					guid => new CustomerId(guid)));

			builder.Property(p => p.FirstName).HasMaxLength(50);
			builder.Property(p => p.LastName).HasMaxLength(50);
		}
	}
}