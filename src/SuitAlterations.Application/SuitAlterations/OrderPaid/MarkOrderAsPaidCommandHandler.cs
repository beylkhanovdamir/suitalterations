using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SuitAlterations.Domain.SuitAlterations;

namespace SuitAlterations.Application.SuitAlterations.OrderPaid
{
	public class MarkOrderAsPaidCommandHandler : IRequestHandler<MarkOrderAsPaidCommand>
	{
		private readonly ISuitAlterationRepository _suitAlterationRepository;

		public MarkOrderAsPaidCommandHandler(ISuitAlterationRepository suitAlterationRepository)
		{
			_suitAlterationRepository = suitAlterationRepository;
		}

		public async Task<Unit> Handle(MarkOrderAsPaidCommand request, CancellationToken cancellationToken)
		{
			SuitAlteration suitAlteration = await _suitAlterationRepository.GetByIdAsync(request.SuitAlterationId);

			suitAlteration.MarkOrderAsPaid();

			return Unit.Value;
		}
	}
}