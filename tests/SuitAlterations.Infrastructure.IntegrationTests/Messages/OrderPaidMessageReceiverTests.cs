using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SuitAlterations.Application.SuitAlterations.OrderPaid;
using SuitAlterations.Domain.SeedWork;
using SuitAlterations.Domain.SuitAlterations;
using SuitAlterations.Infrastructure.Configuration;
using SuitAlterations.Infrastructure.Messages;

namespace SuitAlterations.Infrastructure.IntegrationTests.Messages
{
	[TestFixture]
	[Ignore("Since the sending a message to topic is paid, please run these tests if necessary")]
	public class OrderPaidMessageReceiverTests
	{
		private readonly OrderPaidMessageFilter _orderPaidMessageFilter = new OrderPaidMessageFilter();

		private readonly OrderPaidMessage _orderPaidMessage =
			new OrderPaidMessage(new SuitAlterationId(Guid.NewGuid()));

		private TopicClient _topicClient;
		private IConfiguration _configuration;
		private AzureServiceBusTopicSubscriptionConfiguration _azureServiceBusTopicSubscriptionConfiguration;
		private OrderPaidMessageReceiver _messageReceiver;

		private Mock<IMediator> _mediatorMock;

		[OneTimeSetUp]
		public async Task SetUp()
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

			// sends a test message to the Azure Topic
			await SendPaidSuitAlterationMessageToAzureTopic();
		}

		[SetUp]
		public void Init()
		{
			_messageReceiver = new OrderPaidMessageReceiver(_azureServiceBusTopicSubscriptionConfiguration, _mediatorMock.Object);
		}

		/// <summary>
		///     Emulates the real sending of the PaidSuitAlteration message to Azure topic as it does the POS-terminal
		/// </summary>
		private async Task SendPaidSuitAlterationMessageToAzureTopic()
		{
			var message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_orderPaidMessage)))
			{
				CorrelationId = _orderPaidMessageFilter.Value
			};

			await _topicClient.SendAsync(message);
		}

		[Test]
		public async Task IgnoresReceivingAlterationFinishedMessageByUsingFilterInAzureTopic()
		{
			var messageBody =
				Encoding.UTF8.GetBytes(
					JsonConvert.SerializeObject("A message with AlterationFinished label would be ignored by the current receiver"));
			var alterationFinishedMessage = new Message(messageBody)
			{
				CorrelationId = "AlterationFinished"
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
		public async Task ReceivesSentOrderPaidMessageByUsingAccordingFilterInAzureTopicAndUpdatesCustomerOrderStatusToPaid()
		{
			MarkOrderAsPaidCommand command = new MarkOrderAsPaidCommand(_orderPaidMessage.SuitAlterationId);

			_mediatorMock.Setup(x => x.Send(command, It.IsAny<CancellationToken>()))
				.ReturnsAsync(() => Unit.Value);

			await _messageReceiver.RegisterMessageReceivingAsync(new OrderPaidMessageFilter());

			FreezeThread();

			_mediatorMock.Verify(x => x.Send(It.Is<MarkOrderAsPaidCommand>(x => x.SuitAlterationId == command.SuitAlterationId),
				It.IsAny<CancellationToken>()), Times.Once);

			await _messageReceiver.StopReceivingMessagesAsync();
			await _topicClient.CloseAsync();
		}
	}
}