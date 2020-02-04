using System;
using SuitAlterations.Domain.Customers;
using SuitAlterations.Domain.Customers.Events;
using SuitAlterations.Domain.SeedWork;

namespace SuitAlterations.Domain.SuitAlterations
{
	public class SuitAlteration : Entity
	{
		public int LeftSleeveLength { get; }
		public int RightSleeveLength { get; }
		public int LeftTrouserLength { get; }
		public int RightTrouserLength { get; }
		public SuitAlterationStatus Status { get; private set; }
		public CustomerId CustomerId { get; }

		private SuitAlteration()
		{
		}

		public SuitAlterationId Id { get; }

		internal SuitAlteration(
			int leftSleeveLength,
			int rightSleeveLength,
			int leftTrouserLength,
			int rightTrouserLength,
			CustomerId customerId)
		{
			Id = new SuitAlterationId(Guid.NewGuid());
			LeftSleeveLength = leftSleeveLength;
			RightSleeveLength = rightSleeveLength;
			LeftTrouserLength = leftTrouserLength;
			RightTrouserLength = rightTrouserLength;
			CustomerId = customerId;

			Status = SuitAlterationStatus.Created;

			AddDomainEvent(new OrderPlacedDomainEvent(Id));
		}

		public static SuitAlteration CreateNew(
			int leftSleeveLength,
			int rightSleeveLength,
			int leftTrouserLength,
			int rightTrouserLength,
			CustomerId customerId)
		{
			return new SuitAlteration(
				leftSleeveLength,
				rightSleeveLength,
				leftTrouserLength,
				rightTrouserLength,
				customerId);
		}

		public void MarkOrderAsPaid()
		{
			if (Status != SuitAlterationStatus.Created)
			{
				throw new BusinessRuleValidationException($"Order has inconsistent status to be marked as paid. Status - {Status.ToString()}");
			}

			Status = SuitAlterationStatus.Paid;

			AddDomainEvent(new OrderPaidDomainEvent(Id));
		}
	}
}