using SuitAlterations.Domain.SeedWork;
using SuitAlterations.Domain.SuitAlterations;

namespace SuitAlterations.Domain.Customers.Events
{
	public class OrderPaidDomainEvent : BaseDomainEvent
	{
		public OrderPaidDomainEvent(SuitAlterationId suitAlterationId)
		{
			SuitAlterationId = suitAlterationId;
		}

		public SuitAlterationId SuitAlterationId { get; }
	}
}