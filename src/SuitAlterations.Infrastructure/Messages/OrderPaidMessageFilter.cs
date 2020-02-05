using SuitAlterations.Domain.Messages;

namespace SuitAlterations.Infrastructure.Messages
{
	public class OrderPaidMessageFilter : ITopicMessageFilter
	{
		public OrderPaidMessageFilter()
		{
			Value = "OrderPaid";
		}

		public string Value { get; }
	}
}