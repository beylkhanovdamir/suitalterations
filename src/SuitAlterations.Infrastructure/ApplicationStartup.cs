using AutoMapper;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SuitAlterations.Application.Configuration;
using SuitAlterations.Application.Configuration.Validation;
using SuitAlterations.Application.SuitAlterations.PlaceCustomerOrder;
using SuitAlterations.Domain.Customers;
using SuitAlterations.Domain.Messages;
using SuitAlterations.Domain.SeedWork;
using SuitAlterations.Domain.SuitAlterations;
using SuitAlterations.Infrastructure.Configuration;
using SuitAlterations.Infrastructure.Database;
using SuitAlterations.Infrastructure.Domain;
using SuitAlterations.Infrastructure.Domain.Customers;
using SuitAlterations.Infrastructure.Domain.SuitAlterations;
using SuitAlterations.Infrastructure.Messages;

namespace SuitAlterations.Infrastructure
{
	public static class ApplicationStartup
	{
		public static void RegisterApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddAutoMapper(typeof(ApplicationMappingProfile));
			services.AddMediatR(typeof(ApplicationMappingProfile));
			services.AddDbContextPool<ApplicationDbContext>(opts =>
				opts.UseSqlServer(configuration["Database:ConnectionString"]));

			services.AddTransient<IValidator<PlaceCustomerOrderCommand>, PlaceCustomerOrderCommandValidator>();
			services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));
			services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
			services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandValidationBehavior<,>));

			services.AddScoped<ISuitAlterationRepository, SuitAlterationRepository>();
			services.AddScoped<ICustomerRepository, CustomerRepository>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<IDomainEventsDispatcher, DomainEventsDispatcher>();

			services.Configure<AzureServiceBusTopicSubscriptionConfiguration>(configuration.GetSection("AzureServiceBusTopicSubscription"));
			services.AddSingleton(sp => sp.GetRequiredService<IOptions<AzureServiceBusTopicSubscriptionConfiguration>>().Value);

			services.AddSingleton<ISubscriptionMessageReceiver<OrderPaidMessageFilter>, OrderPaidMessageReceiver>();

			services.AddApplicationInsightsTelemetry();
		}

		public static void RegisterMessageSubscribers(this IApplicationBuilder builder)
		{
			var subscriptionReceiver = builder.ApplicationServices.GetService<ISubscriptionMessageReceiver<OrderPaidMessageFilter>>();
			subscriptionReceiver.RegisterMessageReceivingAsync(new OrderPaidMessageFilter());
		}
	}
}