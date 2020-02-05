using SuitAlterations.Domain.Customers;
using SuitAlterations.Infrastructure.Database;

namespace SuitAlterations.Infrastructure.Domain.Customers
{
	public class CustomerRepository : GenericRepository<Customer, ApplicationDbContext>, ICustomerRepository
	{
		public CustomerRepository(ApplicationDbContext context) : base(context)
		{
		}
	}
}