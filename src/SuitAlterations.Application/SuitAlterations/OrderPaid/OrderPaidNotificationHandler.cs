using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SuitAlterations.Domain.SeedWork;
using SuitAlterations.Domain.SuitAlterations;

namespace SuitAlterations.Application.SuitAlterations.OrderPaid
{
	public class OrderPaidNotificationHandler : INotificationHandler<OrderPaidNotification>
	{
		private readonly ISuitAlterationRepository _suitAlterationRepository;
		private readonly IUnitOfWork _unitOfWork;

		public OrderPaidNotificationHandler(ISuitAlterationRepository suitAlterationRepository, IUnitOfWork unitOfWork)
		{
			_suitAlterationRepository = suitAlterationRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task Handle(OrderPaidNotification notification, CancellationToken cancellationToken)
		{
			SuitAlteration suitAlteration = await _suitAlterationRepository.GetByIdAsync(notification.SuitAlterationId);

			suitAlteration.MarkOrderAsPaid();

			await _unitOfWork.CommitAsync(cancellationToken);
		}
	}
}