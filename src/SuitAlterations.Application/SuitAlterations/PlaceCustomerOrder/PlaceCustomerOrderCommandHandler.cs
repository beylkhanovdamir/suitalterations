using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SuitAlterations.Domain.Customers;
using SuitAlterations.Domain.SeedWork;

namespace SuitAlterations.Application.SuitAlterations.PlaceCustomerOrder
{
	public class PlaceCustomerOrderCommandHandler : IRequestHandler<PlaceCustomerOrderCommand>
	{
		private readonly ICustomerRepository _customerRepository;
		private readonly IUnitOfWork _unitOfWork;

		public PlaceCustomerOrderCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
		{
			_customerRepository = customerRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<Unit> Handle(PlaceCustomerOrderCommand request,
			CancellationToken cancellationToken)
		{
			var customer = await _customerRepository.GetByIdAsync(request.CustomerId);

			customer.PlaceOrder(request.LeftSleeveLength,
				request.RightSleeveLength,
				request.LeftTrouserLength,
				request.RightTrouserLength);

			await _unitOfWork.CommitAsync(cancellationToken);

			return Unit.Value;
		}
	}
}