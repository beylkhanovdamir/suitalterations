using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SuitAlterations.Domain.Customers.Events;

namespace SuitAlterations.Application.SuitAlterations.PlaceCustomerOrder
{
	public class OrderPlacedDomainEventHandler : INotificationHandler<OrderPlacedDomainEvent>
	{
		private readonly INotifierService _notifierService;

		public OrderPlacedDomainEventHandler(INotifierService notifierService)
		{
			_notifierService = notifierService;
		}

		public async Task Handle(OrderPlacedDomainEvent notification, CancellationToken cancellationToken)
		{
			await _notifierService.OnOrderPlacedNotification(notification.SuitAlterationId);
		}
	}
}