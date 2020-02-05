using MediatR;
using SuitAlterations.Domain.SuitAlterations;

namespace SuitAlterations.Application.SuitAlterations.OrderPaid
{
	public class OrderPaidNotification : INotification
	{
		public SuitAlterationId SuitAlterationId { get; }

		public OrderPaidNotification(SuitAlterationId suitAlterationId)
		{
			SuitAlterationId = suitAlterationId;
		}
	}
}