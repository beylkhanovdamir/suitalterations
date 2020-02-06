using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SuitAlterations.Domain.SeedWork;
using SuitAlterations.Infrastructure.Database;

namespace SuitAlterations.Infrastructure.Processing
{
	public class DomainEventsDispatcher : IDomainEventsDispatcher
	{
		private readonly IMediator _mediator;
		private readonly ApplicationDbContext _applicationDbContext;

		public DomainEventsDispatcher(ApplicationDbContext applicationDbContext, IMediator mediator)
		{
			_applicationDbContext = applicationDbContext;
			_mediator = mediator;
		}

		public async Task DispatchEventsAsync()
		{
			var domainEntities = _applicationDbContext.ChangeTracker
				.Entries<Entity>()
				.Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any()).ToList();

			var domainEvents = domainEntities
				.SelectMany(x => x.Entity.DomainEvents)
				.ToList();

			// we need to clear entities events right here to avoid circular dependency and further SO by domain events dispatchers
			domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());
			
			var tasks = domainEvents
				.Select(async (domainEvent) => { await _mediator.Publish(domainEvent); });

			await Task.WhenAll(tasks);

		}
	}
}