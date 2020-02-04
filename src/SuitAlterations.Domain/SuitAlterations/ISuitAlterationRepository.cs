using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SuitAlterations.Domain.Customers;
using SuitAlterations.Domain.SeedWork;

namespace SuitAlterations.Domain.SuitAlterations
{
	public interface ISuitAlterationRepository : IGenericRepository<SuitAlteration>
	{
		Task<IReadOnlyList<SuitAlteration>> GetCustomerSuitAlterationsAsync(CustomerId customerId,
			CancellationToken cancellationToken = default);
	}
}