using System;
using SuitAlterations.Domain.SeedWork;
using SuitAlterations.Domain.SuitAlterations;

namespace SuitAlterations.Domain.Customers
{
	/// <summary>
	///     Customer Aggregate
	/// </summary>
	public class Customer : Entity, IAggregateRoot
	{
		public CustomerId Id { get; }
		public string FirstName { get; }
		public string LastName { get; }

		private Customer()
		{
		}

		public Customer(string firstName, string lastName)
		{
			Id = new CustomerId(Guid.NewGuid());
			FirstName = firstName;
			LastName = lastName;
		}

		public static Customer CreateNew(string firstName, string lastName)
		{
			return new Customer(firstName, lastName);
		}

		public SuitAlteration PlaceOrder(int leftSleeveLength, int rightSleeveLength, int leftTrouserLength, int rightTrouserLength)
		{
			return SuitAlteration.CreateNew(leftSleeveLength, rightSleeveLength, leftTrouserLength, rightTrouserLength, Id);
		}
	}
}