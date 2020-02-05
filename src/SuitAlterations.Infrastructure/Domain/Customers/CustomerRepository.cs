using System.Threading.Tasks;
using SuitAlterations.Domain.Customers;
using SuitAlterations.Infrastructure.Database;

namespace SuitAlterations.Infrastructure.Domain.Customers
{
	public class CustomerRepository : GenericRepository<Customer, ApplicationDbContext>, ICustomerRepository
	{
		private readonly ApplicationDbContext _context;

		public CustomerRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task AddAsync(Customer customer)
		{
			await _context.Customers.AddAsync(customer);
		}
	}
}