using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuitAlterations.Domain.Customers;
using SuitAlterations.Domain.SuitAlterations;
using SuitAlterations.Infrastructure.Database;

namespace SuitAlterations.Infrastructure.Domain.SuitAlterations
{
	public class SuitAlterationRepository : GenericRepository<SuitAlteration, ApplicationDbContext>,
		ISuitAlterationRepository
	{
		private readonly ApplicationDbContext _context;

		public SuitAlterationRepository(ApplicationDbContext context) : base(context)
		{
			_context = context;
		}

		public async Task<IReadOnlyList<SuitAlteration>> GetByCustomerIdAsync(CustomerId customerId, CancellationToken cancellationToken = default)
		{
			return await _context.SuitAlterations.Where(x => x.CustomerId == customerId).ToListAsync(cancellationToken);
		}
	}
}