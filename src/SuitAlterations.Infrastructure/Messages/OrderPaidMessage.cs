using SuitAlterations.Domain.SuitAlterations;

namespace SuitAlterations.Infrastructure.Messages
{
	public class OrderPaidMessage
	{
		public SuitAlterationId SuitAlterationId { get; }

		public OrderPaidMessage(SuitAlterationId suitAlterationId)
		{
			SuitAlterationId = suitAlterationId;
		}
	}
}