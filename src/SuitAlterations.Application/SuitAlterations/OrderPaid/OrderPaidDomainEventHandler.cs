using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SuitAlterations.Domain.Customers.Events;

namespace SuitAlterations.Application.SuitAlterations.OrderPaid
{
	public class OrderPaidDomainEventHandler : INotificationHandler<OrderPaidDomainEvent>
	{
		public OrderPaidDomainEventHandler()
		{
		}

		public async Task Handle(OrderPaidDomainEvent notification, CancellationToken cancellationToken)
		{
			// todo: send push notification to SignalR hub
		}
	}
}