using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SuitAlterations.Domain.Customers;
using SuitAlterations.Domain.Customers.Events;
using SuitAlterations.Domain.SuitAlterations;

namespace SuitAlterations.Application.SuitAlterations.OrderPaid
{
	public class OrderPaidDomainEventHandler : INotificationHandler<OrderPaidDomainEvent>
	{
		private readonly ISuitAlterationRepository _suitAlterationRepository;
		private readonly INotifierService _notifierService;

		public OrderPaidDomainEventHandler(ISuitAlterationRepository suitAlterationRepository, INotifierService notifierService)
		{
			_suitAlterationRepository = suitAlterationRepository;
			_notifierService = notifierService;
		}

		public async Task Handle(OrderPaidDomainEvent notification, CancellationToken cancellationToken)
		{
			var order = await _suitAlterationRepository.GetByIdAsync(notification.SuitAlterationId);
			// e.g. we can send payment info or whatever to the customer email 
			await _notifierService.OnOrderPaidNotification(order.Id);
		}
	}
}