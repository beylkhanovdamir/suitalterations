using System.Threading;
using System.Threading.Tasks;

namespace SuitAlterations.Domain.SeedWork
{
	public interface IUnitOfWork
	{
		Task CommitAsync(CancellationToken cancellationToken = default);
	}
}