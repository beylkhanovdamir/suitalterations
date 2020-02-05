using System;
using SuitAlterations.Domain.Customers;
using SuitAlterations.Domain.Customers.Events;
using SuitAlterations.Domain.SeedWork;

namespace SuitAlterations.Domain.SuitAlterations
{
	/// <summary>
	/// Suit Alteration Aggregate root
	/// </summary>
	public class SuitAlteration : Entity, IAggregateRoot
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
			int rightTrouserLength)
		{
			Id = new SuitAlterationId(Guid.NewGuid());
			LeftSleeveLength = leftSleeveLength;
			RightSleeveLength = rightSleeveLength;
			LeftTrouserLength = leftTrouserLength;
			RightTrouserLength = rightTrouserLength;

			Status = SuitAlterationStatus.Created;

			AddDomainEvent(new OrderPlacedDomainEvent(Id));
		}

		public static SuitAlteration CreateNew(
			int leftSleeveLength,
			int rightSleeveLength,
			int leftTrouserLength,
			int rightTrouserLength)
		{
			return new SuitAlteration(
				leftSleeveLength,
				rightSleeveLength,
				leftTrouserLength,
				rightTrouserLength);
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