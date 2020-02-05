using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SuitAlterations.Domain.Customers.Events;

namespace SuitAlterations.Application.SuitAlterations.PlaceCustomerOrder
{
	public class OrderPlacedDomainEventHandler : INotificationHandler<OrderPlacedDomainEvent>
	{
		public OrderPlacedDomainEventHandler()
		{
		}

		public Task Handle(OrderPlacedDomainEvent notification, CancellationToken cancellationToken)
		{
			// todo: send push notification to SignalR hub
			return Task.CompletedTask;
		}
	}
}