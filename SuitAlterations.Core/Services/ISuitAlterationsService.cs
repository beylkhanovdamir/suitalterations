using System.Threading.Tasks;

namespace SuitAlterations.Core.Services {
	public interface ISuitAlterationsService {
		Task SetSuitAlterationAsPaid(int alterationId);
	}
}