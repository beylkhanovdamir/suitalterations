using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using SuitAlterations.ServiceBusTopic.Notifications;

namespace SuitAlterations.ServiceBusTopic {
	public class SuitAlterationTopicSubscription : ISuitAlterationTopicSubscription {
		private readonly ISuitAlterationNotificationService _suitAlterationNotificationService;
		public readonly ISubscriptionClient SubscriptionClient;

		public SuitAlterationTopicSubscription(ISuitAlterationNotificationService suitAlterationNotificationService, AzureServiceBusConfiguration azureServiceBusConfiguration) {
			_suitAlterationNotificationService = suitAlterationNotificationService;

			SubscriptionClient = new SubscriptionClient(
				azureServiceBusConfiguration.ConnectionString,
				azureServiceBusConfiguration.Topic.Path,
				azureServiceBusConfiguration.Topic.SubscriptionName) { PrefetchCount = 3 };
		}

		public void RegisterMessageReceivingHandler() {
			var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler) {
				MaxConcurrentCalls = 1,
				AutoComplete = false
			};

			SubscriptionClient.RegisterMessageHandler(HandleMessage, messageHandlerOptions);
		}

		public async Task StopReceivingMessages() {
			await SubscriptionClient.CloseAsync();
		}

		private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg) {
			return Task.CompletedTask;
		}

		private async Task HandleMessage(Message message, CancellationToken cancellationToken) {
			var suitAlterationMessage =
				JsonConvert.DeserializeObject<PaidSuitAlteration>(Encoding.UTF8.GetString(message.Body));

			await _suitAlterationNotificationService.Publish(suitAlterationMessage, cancellationToken);

			await SubscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
		}
	}
}