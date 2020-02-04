using System.Collections.Generic;
using SuitAlterations.Domain.Messages;

namespace SuitAlterations.Domain.SeedWork
{
	public abstract class Entity
	{
		private List<IDomainEvent> _domainEvents;

		/// <summary>
		///     Domain events occurred.
		/// </summary>
		public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

		/// <summary>
		///     Add domain event.
		/// </summary>
		/// <param name="domainEvent"></param>
		protected void AddDomainEvent(IDomainEvent domainEvent)
		{
			_domainEvents ??= new List<IDomainEvent>();
			_domainEvents.Add(domainEvent);
		}

		/// <summary>
		///     Clead domain events.
		/// </summary>
		public void ClearDomainEvents()
		{
			_domainEvents?.Clear();
		}
	}
}