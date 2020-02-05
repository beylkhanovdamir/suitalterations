using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SuitAlterations.Application.SuitAlterations.OrderPaid;
using SuitAlterations.Domain.SuitAlterations;
using SuitAlterations.Infrastructure.Configuration;
using SuitAlterations.Infrastructure.Messages;

namespace SuitAlterations.Infrastructure.IntegrationTests.Messages
{
	[TestFixture]
	public class OrderPaidMessageReceiverTests
	{
		private readonly OrderPaidMessageFilter _orderPaidMessageFilter = new OrderPaidMessageFilter();

		private readonly OrderPaidNotification _orderPaidNotification =
			new OrderPaidNotification(new SuitAlterationId(Guid.NewGuid()));

		private TopicClient _topicClient;
		private IConfiguration _configuration;
		private AzureServiceBusTopicSubscriptionConfiguration _azureServiceBusTopicSubscriptionConfiguration;

		private Mock<IMediator> _mediatorMock;
		private OrderPaidMessageReceiver _messageReceiver;

		[OneTimeSetUp]
		public void SetUp()
		{
			_mediatorMock = new Mock<IMediator>();

			IConfigurationBuilder builder = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", true, true)
				.AddUserSecrets(typeof(OrderPaidMessageReceiverTests).Assembly);

			_configuration = builder.Build();

			_azureServiceBusTopicSubscriptionConfiguration = _configuration.GetSection("AzureServiceBusTopicSubscription")
				.Get<AzureServiceBusTopicSubscriptionConfiguration>();

			_topicClient = new TopicClient(_azureServiceBusTopicSubscriptionConfiguration.ConnectionString,
				_azureServiceBusTopicSubscriptionConfiguration.TopicPath);
		}

		[SetUp]
		public void Init()
		{
			_messageReceiver = new OrderPaidMessageReceiver(_azureServiceBusTopicSubscriptionConfiguration, _mediatorMock.Object);
		}

		/// <summary>
		///     Emulate the sending of the PaidSuitAlteration message to Azure topic by the POS terminal
		/// </summary>
		private async Task SendPaidSuitAlterationMessageToAzureTopic()
		{
			var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_orderPaidNotification)))
			{
				Label = _orderPaidMessageFilter.Value
			};
			await _topicClient.SendAsync(message);
		}

		[Test]
		[Ignore("Since the sending a message to topic is paid, please run this test if necessary")]
		public async Task IgnoresReceivingAlterationFinishedMessageByUsingFilterInAzureTopic()
		{
			var messageBody =
				Encoding.UTF8.GetBytes(
					JsonConvert.SerializeObject("A message with AlterationFinished label would be ignored by the current receiver"));
			var alterationFinishedMessage = new Message(messageBody)
			{
				Label = "AlterationFinished"
			};
			await _topicClient.SendAsync(alterationFinishedMessage);

			await _messageReceiver.RegisterMessageReceivingAsync(new OrderPaidMessageFilter());

			FreezeThread();

			_mediatorMock.VerifyNoOtherCalls();

			await _messageReceiver.StopReceivingMessagesAsync();
			await _topicClient.CloseAsync();
		}

		/// <summary>
		///     we need to freeze current thread for that min timeout to allow Azure subscription receiver to handle message during
		///     this timeout
		/// </summary>
		private static void FreezeThread()
		{
			Thread.Sleep(5000);
		}

		[Test]
		[Ignore("Since the sending a message to topic is paid, please run this test if necessary")]
		public async Task ReceivesOrderPaidMessageByUsingAccordingFilterInAzureTopic()
		{
			await SendPaidSuitAlterationMessageToAzureTopic();

			OrderPaidNotification expectedNotification = null;
			_mediatorMock.Setup(x => x.Publish(It.IsAny<OrderPaidNotification>(), It.IsAny<CancellationToken>()))
				.Callback((OrderPaidNotification result, CancellationToken token) => { expectedNotification = result; });

			await _messageReceiver.RegisterMessageReceivingAsync(new OrderPaidMessageFilter());

			FreezeThread();

			_mediatorMock.Verify(x => x.Publish(expectedNotification, It.IsAny<CancellationToken>()), Times.Once);
			expectedNotification.Should().BeEquivalentTo(new OrderPaidNotification(_orderPaidNotification.SuitAlterationId));

			await _messageReceiver.StopReceivingMessagesAsync();
			await _topicClient.CloseAsync();
		}
	}
}