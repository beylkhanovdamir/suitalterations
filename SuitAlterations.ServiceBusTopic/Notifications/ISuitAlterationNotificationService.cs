using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace SuitAlterations.ServiceBusTopic.Notifications {
	public interface ISuitAlterationNotificationService {
		Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification;
	}
}