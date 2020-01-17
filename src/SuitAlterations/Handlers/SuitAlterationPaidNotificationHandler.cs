using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SuitAlterations.Core.Services;
using SuitAlterations.ServiceBusTopic.Notifications;

namespace SuitAlterations.Handlers {
	public class SuitAlterationPaidNotificationHandler : INotificationHandler<PaidSuitAlteration> {
		private readonly ISuitAlterationsService _suitAlterationsService;

		public SuitAlterationPaidNotificationHandler(ISuitAlterationsService suitAlterationsService) {
			_suitAlterationsService = suitAlterationsService;
		}

		public async Task Handle(PaidSuitAlteration notification, CancellationToken cancellationToken) {
			await _suitAlterationsService.SetSuitAlterationAsPaid(notification.AlterationId);
		}
	}
}