using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuitAlterations.Domain.SeedWork;

namespace SuitAlterations.Infrastructure.Domain
{
	public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity>
		where TEntity : Entity
		where TContext : DbContext
	{
		private readonly TContext _context;

		protected GenericRepository(TContext context)
		{
			_context = context;
		}

		public async Task<List<TEntity>> GetAllAsync()
		{
			return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
		}

		public async Task<TEntity> GetByIdAsync<TId>(TId id)
		{
			var entity = await _context.Set<TEntity>().FindAsync(id);
			if (entity == null)
			{
				NotFound(id);
			}

			return entity;
		}

		public async Task AddAsync(TEntity entity)
		{
			await _context.Set<TEntity>().AddAsync(entity);
		}

		protected static void NotFound<TId>(TId id)
		{
			throw new EntityNotFoundException($"Entity with id={id} not found");
		}
	}
}