using Microsoft.EntityFrameworkCore;
using SuitAlterations.Core.Entities;

namespace SuitAlterations.Core.Data {
	public class ApplicationDbContext : DbContext {
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<SuitAlteration> SuitAlteration { get; set; }

		public DbSet<Customer> Customer { get; set; }
	}
}