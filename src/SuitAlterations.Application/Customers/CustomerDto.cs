using SuitAlterations.Domain.Customers;

namespace SuitAlterations.Application.Customers
{
	public class CustomerDto
	{
		public CustomerId Id { get; set; }
		public string FullName { get; set; }
	}
}