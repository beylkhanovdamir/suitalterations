using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SuitAlterations.Infrastructure.Database;

namespace SuitAlterations.Application.UnitTests
{
	/// <summary>
	///     Entity Framework Core provides in-memory data provider and we no need to mock our DbContext or repositories.
	///     Moreover, we can reach more testability of our logic by this way
	/// </summary>
	public class DbContextBuilder
	{
		public static ApplicationDbContext BuildDbContext()
		{
			DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
				.Options;

			return new ApplicationDbContext(options);
		}
	}
}