using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SuitAlterations.Core.Entities;

namespace SuitAlterations.Core.Data {
	public class SuitAlterationRepository : GenericRepository<SuitAlteration, ApplicationDbContext>, ISuitAlterationRepository {
		private readonly ApplicationDbContext _context;

		public SuitAlterationRepository(ApplicationDbContext context) : base(context) {
			_context = context;
		}

		public async Task<IReadOnlyList<SuitAlteration>> GetSuitAlterationsBy(int customerId) {
			return await _context.Set<SuitAlteration>().Where(x => x.CustomerId == customerId).ToListAsync();
		}
	}
}