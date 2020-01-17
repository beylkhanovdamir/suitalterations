using SuitAlterations.Core.Entities;

namespace SuitAlterations.Core.Data {
	public class CustomerRepository : GenericRepository<Customer, ApplicationDbContext> {
		public CustomerRepository(ApplicationDbContext context) : base(context) { }
	}
}