using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using SuitAlterations.Application.SuitAlterations.OrderPaid;
using SuitAlterations.Domain.Messages;
using SuitAlterations.Infrastructure.Configuration;

namespace SuitAlterations.Infrastructure.Messages
{
	public class OrderPaidMessageReceiver : ISubscriptionMessageReceiver<OrderPaidMessageFilter>
	{
		private readonly ISubscriptionClient _subscriptionClient;
		private readonly IMediator _mediator;

		public OrderPaidMessageReceiver(AzureServiceBusTopicSubscriptionConfiguration serviceBusTopicSubscription, IMediator mediator)
		{
			_mediator = mediator;

			_subscriptionClient = new SubscriptionClient(
				serviceBusTopicSubscription.ConnectionString,
				serviceBusTopicSubscription.TopicPath,
				serviceBusTopicSubscription.SubscriptionName) {PrefetchCount = 3};
		}

		public async Task RegisterMessageReceivingAsync(OrderPaidMessageFilter filter)
		{
			var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
			{
				MaxConcurrentCalls = 1,
				AutoComplete = false
			};

			var rules = (await _subscriptionClient.GetRulesAsync()).ToList();

			if (rules.Any())
			{
				await _subscriptionClient.RemoveRuleAsync(RuleDescription.DefaultRuleName);
			}

			await _subscriptionClient.AddRuleAsync(new RuleDescription()
			{
				Filter = new CorrelationFilter
				{
					CorrelationId = filter.Value
				}
			});

			_subscriptionClient.RegisterMessageHandler(HandleMessage, messageHandlerOptions);
		}

		public async Task StopReceivingMessagesAsync()
		{
			await _subscriptionClient.CloseAsync();
		}

		private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
		{
			return Task.CompletedTask;
		}

		private async Task HandleMessage(Message message, CancellationToken cancellationToken)
		{
			var orderPaidNotification =
				JsonConvert.DeserializeObject<OrderPaidMessage>(Encoding.UTF8.GetString(message.Body));

			await _mediator.Send(new MarkOrderAsPaidCommand(orderPaidNotification.SuitAlterationId), cancellationToken);

			await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
		}
	}
}