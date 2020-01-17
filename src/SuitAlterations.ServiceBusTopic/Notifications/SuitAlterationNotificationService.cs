using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SuitAlterations.ServiceBusTopic.Notifications {
	public class SuitAlterationNotificationService : ISuitAlterationNotificationService {
		private readonly IMediator _mediator;

		public SuitAlterationNotificationService(IMediator mediator) {
			_mediator = mediator;
		}

		public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification {
			await _mediator.Publish(notification, cancellationToken);
		}
	}
}