using System;
using System.Collections.Generic;
using MediatR;
using SuitAlterations.Application.SuitAlterations;
using SuitAlterations.Domain.Customers;

namespace SuitAlterations.Application.Customers.GetCustomerSuitAlterations
{
	public class GetCustomerSuitAlterationsQuery : IRequest<IReadOnlyList<SuitAlterationDto>>
	{
		public CustomerId CustomerId { get; }

		public GetCustomerSuitAlterationsQuery(Guid customerId)
		{
			CustomerId = new CustomerId(customerId);
		}
	}
}