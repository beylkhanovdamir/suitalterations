using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SuitAlterations.Domain.Customers;

namespace SuitAlterations.Application.Customers.GetAllCustomers
{
	public class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, IReadOnlyList<CustomerDto>>
	{
		private readonly ICustomerRepository _customerRepository;
		private readonly IMapper _mapper;

		public GetAllCustomersQueryHandler(ICustomerRepository customerRepository, IMapper mapper)
		{
			_customerRepository = customerRepository;
			_mapper = mapper;
		}

		public async Task<IReadOnlyList<CustomerDto>> Handle(GetAllCustomersQuery request,
			CancellationToken cancellationToken)
		{
			var customers = await _customerRepository.GetAllAsync();
			return customers.Select(_mapper.Map<CustomerDto>).OrderBy(x => x.FullName).ToList();
		}
	}
}