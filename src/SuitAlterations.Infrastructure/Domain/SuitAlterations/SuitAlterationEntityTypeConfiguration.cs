using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SuitAlterations.Domain.Customers;
using SuitAlterations.Domain.SuitAlterations;

namespace SuitAlterations.Infrastructure.Domain.SuitAlterations
{
	public class SuitAlterationEntityTypeConfiguration : IEntityTypeConfiguration<SuitAlteration>
	{
		public void Configure(EntityTypeBuilder<SuitAlteration> builder)
		{
			builder.ToTable("SuitAlterations", "dbo");

			builder.HasKey(b => b.Id);

			builder.Property(p => p.Id)
				.HasConversion(new ValueConverter<SuitAlterationId, Guid>(id => id.Value,
					guid => new SuitAlterationId(guid)));

			builder.Property(p => p.CustomerId)
				.HasConversion(new ValueConverter<CustomerId, Guid>(id => id.Value,
					guid => new CustomerId(guid)))
				.IsRequired();

			builder.Property(p => p.LeftSleeveLength)
				.IsRequired();
			builder.Property(p => p.RightSleeveLength)
				.IsRequired();
			builder.Property(p => p.LeftTrouserLength)
				.IsRequired();
			builder.Property(p => p.RightTrouserLength)
				.IsRequired();
			builder.Property(p => p.Status)
				.HasConversion(new EnumToNumberConverter<SuitAlterationStatus, byte>())
				.IsRequired();
		}
	}
}