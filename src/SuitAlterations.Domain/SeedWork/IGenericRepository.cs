using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuitAlterations.Domain.SeedWork
{
	public interface IGenericRepository<T> where T : Entity, IAggregateRoot
	{
		Task<List<T>> GetAllAsync();
		Task<T> GetByIdAsync<TId>(TId id);
	}
}