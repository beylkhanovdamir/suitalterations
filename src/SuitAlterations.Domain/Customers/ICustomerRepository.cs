using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SuitAlterations.Domain.Customers.SuitAlterations;
using SuitAlterations.Domain.SeedWork;

namespace SuitAlterations.Domain.Customers
{
	public interface ICustomerRepository : IGenericRepository<Customer>
	{
		Task<IReadOnlyList<SuitAlteration>> GetCustomerSuitAlterationsAsync(CustomerId customerId,
			CancellationToken cancellationToken = default);
	}
}