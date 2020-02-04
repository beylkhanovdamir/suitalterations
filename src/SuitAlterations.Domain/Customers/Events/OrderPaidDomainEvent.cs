using SuitAlterations.Domain.Customers.SuitAlterations;
using SuitAlterations.Domain.SeedWork;

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