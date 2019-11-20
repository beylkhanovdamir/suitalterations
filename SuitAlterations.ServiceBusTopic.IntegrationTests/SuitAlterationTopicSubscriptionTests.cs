using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SuitAlterations.ServiceBusTopic.Notifications;

namespace SuitAlterations.ServiceBusTopic.IntegrationTests {
	[TestFixture]
	public class SuitAlterationTopicSubscriptionTests {
		private TopicClient _topicClient;
		private IConfiguration _configuration;

		private Mock<ISuitAlterationNotificationService> _suitAlterationsNotificationServiceMock;

		private readonly PaidSuitAlteration _paidSuitAlteration = new PaidSuitAlteration { AlterationId = 100 };

		[OneTimeSetUp]
		public void SetUp() {
			_suitAlterationsNotificationServiceMock = new Mock<ISuitAlterationNotificationService>();

			IConfigurationBuilder builder = new ConfigurationBuilder()
				.AddJsonFile("appsettings.servicebus.json", true, true);

			builder.AddUserSecrets<AzureServiceBusConfiguration>();

			_configuration = builder.Build();

			var azureServiceBusConfiguration =
				_configuration.GetSection("AzureServiceBus").Get<AzureServiceBusConfiguration>();

			_topicClient = new TopicClient(azureServiceBusConfiguration.ConnectionString,
			                               azureServiceBusConfiguration.Topic.Path);
		}

		/// <summary>
		///     Emulate the sending of the PaidSuitAlteration message to Azure topic by the POS terminal
		/// </summary>
		private async Task SendPaidSuitAlterationMessageToAzureTopic() {
			var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_paidSuitAlteration)));
			await _topicClient.SendAsync(message);
		}

		[Test]
		[Ignore("Since the sending a message to topic is paid, we will run this test it as required")]
		public async Task ReceivesMessageFromAzureTopicSubscriptionWhenSuitAlterationWasPaid() {
			await SendPaidSuitAlterationMessageToAzureTopic();

			var topicSubscription = new SuitAlterationTopicSubscription(_suitAlterationsNotificationServiceMock.Object);
			PaidSuitAlteration expectedAlteration = null;
			_suitAlterationsNotificationServiceMock.Setup(x => x.Publish(It.IsAny<PaidSuitAlteration>(), It.IsAny<CancellationToken>()))
			                                       .Callback((PaidSuitAlteration result, CancellationToken token) => { expectedAlteration = result; });

			topicSubscription.RegisterMessageReceivingHandler();
			Thread.Sleep(5000); // we need to allocate timeout to allow Azure subscription receiver to handle message during this timeout

			expectedAlteration.Should().BeEquivalentTo(new PaidSuitAlteration { AlterationId = _paidSuitAlteration.AlterationId });

			await topicSubscription.StopReceivingMessages();
		}
	}
}