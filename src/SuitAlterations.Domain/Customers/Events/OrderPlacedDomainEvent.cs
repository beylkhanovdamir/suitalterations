using SuitAlterations.Domain.SeedWork;
using SuitAlterations.Domain.SuitAlterations;

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