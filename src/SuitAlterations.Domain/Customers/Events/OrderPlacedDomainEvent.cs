using SuitAlterations.Domain.Customers.SuitAlterations;
using SuitAlterations.Domain.SeedWork;

namespace SuitAlterations.Domain.Customers.Events
{
	public class OrderPlacedDomainEvent : BaseDomainEvent
	{
		public OrderPlacedDomainEvent(SuitAlterationId suitAlterationId)
		{
			SuitAlterationId = suitAlterationId;
		}

		public SuitAlterationId SuitAlterationId { get; }
	}
}