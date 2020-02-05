using System;
using System.Collections.Generic;
using System.Linq;
using SuitAlterations.Domain.SeedWork;
using SuitAlterations.Domain.SuitAlterations;

namespace SuitAlterations.Domain.Customers
{
	/// <summary>
	///     Customer Aggregate root
	/// </summary>
	public class Customer : Entity, IAggregateRoot
	{
		public CustomerId Id { get; }
		public string FirstName { get; }
		public string LastName { get; }

		private readonly List<SuitAlteration> _suitAlterations;
		public IEnumerable<SuitAlteration> SuitAlterations => _suitAlterations?.ToList();

		private Customer()
		{
		}

		public Customer(string firstName, string lastName)
		{
			Id = new CustomerId(Guid.NewGuid());
			FirstName = firstName;
			LastName = lastName;
			_suitAlterations = new List<SuitAlteration>();
		}

		public static Customer CreateNew(string firstName, string lastName)
		{
			return new Customer(firstName, lastName);
		}

		public void PlaceOrder(int leftSleeveLength, int rightSleeveLength, int leftTrouserLength, int rightTrouserLength)
		{
			var order = SuitAlteration.CreateNew(leftSleeveLength, rightSleeveLength, leftTrouserLength, rightTrouserLength);
			
			_suitAlterations.Add(order);
		}
	}
}