using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using SuitAlterations.Domain.SeedWork;
using SuitAlterations.Infrastructure.Database;

namespace SuitAlterations.Infrastructure.Domain
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _dbContext;
		private readonly IDomainEventsDispatcher _domainEventsDispatcher;
		private IDbContextTransaction _transaction;

		public UnitOfWork(ApplicationDbContext dbContext, IDomainEventsDispatcher domainEventsDispatcher)
		{
			_dbContext = dbContext;
			_domainEventsDispatcher = domainEventsDispatcher;
		}

		public async Task CommitAsync(CancellationToken cancellationToken = default)
		{
			try
			{
				_transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

				await _dbContext.SaveChangesAsync(cancellationToken);

				//committing before dispatching
				await _transaction.CommitAsync(cancellationToken);

				//we publish here events which are not requiring committing transaction in accordance with DDD best practice
				await _domainEventsDispatcher.DispatchEventsAsync();
			}
			catch (Exception)
			{
				await RollbackAsync(cancellationToken);
				throw;
			}
		}

		private async Task RollbackAsync(CancellationToken cancellationToken = default)
		{
			if (_transaction != null)
			{
				await _transaction.RollbackAsync(cancellationToken);
				await _transaction.DisposeAsync();
			}
		}
	}
}