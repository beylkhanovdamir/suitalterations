using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuitAlterations.Core.Entities;
using SuitAlterations.Core.Services;
using SuitAlterations.Dto;

namespace SuitAlterations.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class SuitAlterationsController : ControllerBase {
		private readonly ISuitAlterationsService _suitAlterationsService;
		private readonly IMapper _mapper;

		public SuitAlterationsController(ISuitAlterationsService suitAlterationsService, IMapper mapper) {
			_suitAlterationsService = suitAlterationsService;
			_mapper = mapper;
		}

		[HttpGet("customers")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public async Task<IActionResult> Customers() {
			IReadOnlyList<Customer> customers = await _suitAlterationsService.GetAllCustomers();

			List<CustomerDto> result = customers.Select(_mapper.Map<CustomerDto>).ToList();

			return Ok(result);
		}
	}
}