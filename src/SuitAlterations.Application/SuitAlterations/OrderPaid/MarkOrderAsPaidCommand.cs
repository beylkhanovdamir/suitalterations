using MediatR;
using SuitAlterations.Domain.SuitAlterations;

namespace SuitAlterations.Application.SuitAlterations.OrderPaid
{
	public class MarkOrderAsPaidCommand : IRequest
	{
		public SuitAlterationId SuitAlterationId { get; }

		public MarkOrderAsPaidCommand(SuitAlterationId suitAlterationId)
		{
			SuitAlterationId = suitAlterationId;
		}
	}
}