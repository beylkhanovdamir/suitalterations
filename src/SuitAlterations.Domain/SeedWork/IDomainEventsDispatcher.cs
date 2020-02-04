using System.Threading.Tasks;

namespace SuitAlterations.Domain.SeedWork
{
	public interface IDomainEventsDispatcher
	{
		Task DispatchEventsAsync();
	}
}