using System;
using MediatR;
using SuitAlterations.Domain.Customers;

namespace SuitAlterations.Application.SuitAlterations.PlaceCustomerOrder
{
	public class PlaceCustomerOrderCommand : IRequest
	{
		public int LeftSleeveLength { get; }

		public int RightSleeveLength { get; }

		public int LeftTrouserLength { get; }

		public int RightTrouserLength { get; }

		public CustomerId CustomerId { get; }

		public PlaceCustomerOrderCommand(
			int leftSleeveLength,
			int rightSleeveLength,
			int leftTrouserLength,
			int rightTrouserLength,
			Guid customerId)
		{
			LeftSleeveLength = leftSleeveLength;
			RightSleeveLength = rightSleeveLength;
			LeftTrouserLength = leftTrouserLength;
			RightTrouserLength = rightTrouserLength;
			CustomerId = new CustomerId(customerId);
		}
	}
}