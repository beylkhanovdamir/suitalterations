using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SuitAlterations.Core.Data {
	public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity>
		where TEntity : class, IEntity
		where TContext : DbContext {
		private readonly TContext _context;

		public GenericRepository(TContext context) {
			_context = context;
		}

		public async Task<TEntity> Create(TEntity entity) {
			_context.Set<TEntity>().Add(entity);
			await _context.SaveChangesAsync();
			return entity;
		}

		public async Task<TEntity> GetBy(int id) {
			return await _context.Set<TEntity>().FindAsync(id);
		}

		public async Task<List<TEntity>> GetAll() {
			return await _context.Set<TEntity>().ToListAsync();
		}

		public async Task<TEntity> Update(TEntity entity) {
			_context.Entry(entity).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			return entity;
		}
	}
}