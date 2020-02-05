using System.Threading.Tasks;
using SuitAlterations.Domain.SeedWork;

namespace SuitAlterations.Domain.Customers
{
	public interface ICustomerRepository : IGenericRepository<Customer>
	{
		Task AddAsync(Customer customer);
	}
}