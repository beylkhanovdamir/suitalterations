using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SuitAlterations.Domain.SeedWork;
using SuitAlterations.Domain.SuitAlterations;

namespace SuitAlterations.Domain.Customers
{
	public interface ICustomerRepository : IGenericRepository<Customer>
	{
		
	}
}