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
using SuitAlterations.Application.SuitAlterations;
using SuitAlterations.Application.SuitAlterations.PlaceCustomerOrder;
using SuitAlterations.Domain.Messages;
using SuitAlterations.Infrastructure.Configuration;
using SuitAlterations.Infrastructure.Database;
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

			services.AddScoped<INotifierService, NotifierService>();
			
			services.AddTransient<IValidator<PlaceCustomerOrderCommand>, PlaceCustomerOrderCommandValidator>();
			services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));
			services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
			services.AddScoped(typeof(IPipelineBehavior<,>), typeof(CommandValidationBehavior<,>));

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