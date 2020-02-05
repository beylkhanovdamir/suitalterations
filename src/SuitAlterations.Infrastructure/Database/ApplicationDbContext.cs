using Microsoft.EntityFrameworkCore;
using SuitAlterations.Domain.Customers;
using SuitAlterations.Domain.SuitAlterations;
using SuitAlterations.Infrastructure.Domain.Customers;
using SuitAlterations.Infrastructure.Domain.SuitAlterations;

namespace SuitAlterations.Infrastructure.Database
{
	public sealed class ApplicationDbContext : DbContext
	{
		public DbSet<Customer> Customers { get; set; }
		public DbSet<SuitAlteration> SuitAlterations { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
			Database.EnsureCreated();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new CustomerEntityTypeConfiguration());
			modelBuilder.ApplyConfiguration(new SuitAlterationEntityTypeConfiguration());
		}
	}
}