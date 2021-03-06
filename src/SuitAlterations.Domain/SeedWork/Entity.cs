using System.Collections.Generic;
using SuitAlterations.Domain.Messages;

namespace SuitAlterations.Domain.SeedWork
{
	public abstract class Entity
	{
		private List<IDomainEvent> _domainEvents;

		/// <summary>
		/// Based on a Better Domain Events pattern
		/// </summary>
		public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

		protected void AddDomainEvent(IDomainEvent domainEvent)
		{
			_domainEvents ??= new List<IDomainEvent>();
			_domainEvents.Add(domainEvent);
		}

		public void ClearDomainEvents()
		{
			_domainEvents?.Clear();
		}
	}
}