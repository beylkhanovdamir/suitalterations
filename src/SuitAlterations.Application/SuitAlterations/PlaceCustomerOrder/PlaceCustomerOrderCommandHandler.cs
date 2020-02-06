using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SuitAlterations.Domain.Customers;

namespace SuitAlterations.Application.SuitAlterations.PlaceCustomerOrder
{
	public class PlaceCustomerOrderCommandHandler : IRequestHandler<PlaceCustomerOrderCommand>
	{
		private readonly ICustomerRepository _customerRepository;

		public PlaceCustomerOrderCommandHandler(ICustomerRepository customerRepository)
		{
			_customerRepository = customerRepository;
		}

		public async Task<Unit> Handle(PlaceCustomerOrderCommand request,
			CancellationToken cancellationToken)
		{
			var customer = await _customerRepository.GetByIdAsync(request.CustomerId);

			customer.PlaceOrder(request.LeftSleeveLength,
				request.RightSleeveLength,
				request.LeftTrouserLength,
				request.RightTrouserLength);

			return Unit.Value;
		}
	}
}