using System.Collections.Generic;
using MediatR;

namespace SuitAlterations.Application.Customers.GetAllCustomers
{
	public class GetAllCustomersQuery : IRequest<IReadOnlyList<CustomerDto>>
	{
	}
}