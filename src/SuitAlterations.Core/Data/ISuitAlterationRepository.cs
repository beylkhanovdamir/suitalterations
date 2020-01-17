using System.Collections.Generic;
using System.Threading.Tasks;
using SuitAlterations.Core.Entities;

namespace SuitAlterations.Core.Data {
	public interface ISuitAlterationRepository : IGenericRepository<SuitAlteration> {
		Task<IReadOnlyList<SuitAlteration>> GetSuitAlterationsBy(int customerId);
	}
}