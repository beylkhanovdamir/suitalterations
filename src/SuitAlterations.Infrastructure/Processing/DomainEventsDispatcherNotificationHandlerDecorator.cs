using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SuitAlterations.Domain.SeedWork;

namespace SuitAlterations.Infrastructure.Processing
{
	public class DomainEventsDispatcherNotificationHandlerDecorator<T> : INotificationHandler<T> where T : INotification
	{
		private readonly INotificationHandler<T> _decoratedNotificationHandler;
		private readonly IDomainEventsDispatcher _domainEventsDispatcher;

		public DomainEventsDispatcherNotificationHandlerDecorator(
			IDomainEventsDispatcher domainEventsDispatcher, 
			INotificationHandler<T> decorated)
		{
			_domainEventsDispatcher = domainEventsDispatcher;
			_decoratedNotificationHandler = decorated;
		}

		public async Task Handle(T notification, CancellationToken cancellationToken)
		{
			await _decoratedNotificationHandler.Handle(notification, cancellationToken);

			await _domainEventsDispatcher.DispatchEventsAsync();
		}
		
	}
}