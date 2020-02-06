using Autofac;
using MediatR;
using SuitAlterations.Domain.SeedWork;

namespace SuitAlterations.Infrastructure.Processing
{
	public class ProcessingModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<DomainEventsDispatcher>()
				.As<IDomainEventsDispatcher>()
				.InstancePerLifetimeScope();

			builder.RegisterGenericDecorator(
				typeof(DomainEventsDispatcherCommandHandlerDecorator<>),
				typeof(IRequestHandler<,>));
			
			builder.RegisterGenericDecorator(
				typeof(DomainEventsDispatcherNotificationHandlerDecorator<>), 
				typeof(INotificationHandler<>));
		}
	}
}