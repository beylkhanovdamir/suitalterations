using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuitAlterations.Core.Data {
	public interface IGenericRepository<T> where T : class, IEntity {
		Task<List<T>> GetAll();
		Task<T> GetBy(int id);
		Task<T> Create(T entity);
		Task<T> Update(T entity);
	}
}