using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SuitAlterations.Domain.SeedWork;

namespace SuitAlterations.Infrastructure.Processing
{
	public class DomainEventsDispatcherCommandHandlerDecorator<T> : IRequestHandler<T, Unit> where T:IRequest
	{
		private readonly IRequestHandler<T, Unit> _decoratedCommandHandler;
		private readonly IUnitOfWork _unitOfWork;

		public DomainEventsDispatcherCommandHandlerDecorator(
			IRequestHandler<T, Unit> decoratedCommandHandler, 
			IUnitOfWork unitOfWork)
		{
			_decoratedCommandHandler = decoratedCommandHandler;
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(T command, CancellationToken cancellationToken)
		{
			await _decoratedCommandHandler.Handle(command, cancellationToken);

			await _unitOfWork.CommitAsync(cancellationToken);

			return Unit.Value;
		}
	}
}