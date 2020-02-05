using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SuitAlterations.Application.SuitAlterations;
using SuitAlterations.Domain.SuitAlterations;

namespace SuitAlterations.Application.Customers.GetCustomerSuitAlterations
{
	public class GetCustomerSuitAlterationsQueryHandler : IRequestHandler<GetCustomerSuitAlterationsQuery,
		IReadOnlyList<SuitAlterationDto>>
	{
		private readonly ISuitAlterationRepository _suitAlterationRepository;
		private readonly IMapper _mapper;

		public GetCustomerSuitAlterationsQueryHandler(ISuitAlterationRepository suitAlterationRepository, IMapper mapper)
		{
			_suitAlterationRepository = suitAlterationRepository;
			_mapper = mapper;
		}

		public async Task<IReadOnlyList<SuitAlterationDto>> Handle(GetCustomerSuitAlterationsQuery request,
			CancellationToken cancellationToken)
		{
			var customerSuitAlterations =
				await _suitAlterationRepository.GetByCustomerIdAsync(request.CustomerId, cancellationToken);

			return customerSuitAlterations.Select(_mapper.Map<SuitAlterationDto>).OrderBy(x => x.Status).ToList();
		}
	}
}