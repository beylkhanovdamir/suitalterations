using Autofac;
using SuitAlterations.Domain.Customers;
using SuitAlterations.Domain.SeedWork;
using SuitAlterations.Domain.SuitAlterations;
using SuitAlterations.Infrastructure.Domain;
using SuitAlterations.Infrastructure.Domain.Customers;
using SuitAlterations.Infrastructure.Domain.SuitAlterations;

namespace SuitAlterations.Infrastructure.Database
{
	public class DataAccessModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<UnitOfWork>()
				.As<IUnitOfWork>()
				.InstancePerLifetimeScope();

			builder.RegisterType<CustomerRepository>()
				.As<ICustomerRepository>()
				.InstancePerLifetimeScope();

			builder.RegisterType<SuitAlterationRepository>()
				.As<ISuitAlterationRepository>()
				.InstancePerLifetimeScope();
		}
	}
}